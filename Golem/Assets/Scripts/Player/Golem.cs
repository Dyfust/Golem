using UnityEngine;
using GolemStates;
using FSM;

public class Golem : Player, IRequireInput
{
	public delegate void GolemEventHandler(Golem golem);
	public static event GolemEventHandler OnGolemActive;

	// --------------------------------------------------------------
	[CustomHeader("Movement")]
	[SerializeField] private CharacterControllerSettings _characterControllerSettings;
	[SerializeField] private float _angularSpeed = 0;

	[CustomHeader("Interact Settings")]
	[SerializeField] private float _interactionDistance;

	[CustomHeader("Block Interaction")]
	[SerializeField] private LayerMask _blockLayer;
	[SerializeField] private float _offsetFromBlock;
	[SerializeField] private float _liftingVerticalOffset;

	[CustomHeader("Orb Attachment")]
	[SerializeField] private GameObject _orbMesh;
	[SerializeField] private Vector3 _attachmentOffset; public Vector3 attachmentOffset => _attachmentOffset;

	[CustomHeader("Animation Blending")]
	[SerializeField] private float _pushingBlendSpeed;

	[CustomHeader("States")]
	[SerializeField] private State _dormantState;

	[CustomHeader("VFX")]
	[SerializeField] private EmissionFade _emissionFade; 
	
	[CustomHeader("References")]
	[SerializeField] private Animator _anim;
	// --------------------------------------------------------------
	private PlayerInputData _inputData;

	private Transform _cameraTransform;
	private Vector3 _forwardRelativeToCamera;
	private Vector3 _forwardRelativeToCharacter;
	private Vector3 _rightRelativeToCamera;
	private Vector3 _currentHeading;
	private Quaternion _targetRotation;

	private Block _block;
	private Vector3 _blockNormal;

	private FSM.FSM _fsm;
	private bool _dormant = true;
	private bool _isIdle = true; 

	private Rigidbody _rb;
	private Transform _thisTransform;

	private CharacterController _controller;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();

		_thisTransform = transform;
		_cameraTransform = Camera.main.transform;

		_controller = new CharacterController(_rb, _characterControllerSettings);

		_dormant = true; 
	}

	private void Start()
	{
		InitaliseFSM();

		if (_orbMesh != null)
			_orbMesh.SetActive(false);

		DebugWindow.AddPrintTask(() => "Golem Grounded: " + _controller.IsGrounded().ToString());
		DebugWindow.AddPrintTask(() => "Golem Ground Normal: " + _controller.GetCollisionNormal().ToString());
		DebugWindow.AddPrintTask(() => "Golem State: " + _fsm.GetCurrentState().debugName);
	}


	float idleTimestamp;
	float idleDt = 0.1f;
	private void Update()
	{
		ComputeAxes();

		_fsm.HandleTransitions();
		_fsm.UpdateLogic();	

		// Cleanup required.
		if (_rb.velocity.sqrMagnitude >= 0.1f)
			idleTimestamp = Time.time;

		_isIdle = _rb.velocity.sqrMagnitude < 0.1f && Time.time > idleTimestamp + idleDt;
		_anim.SetBool("Dormant", _dormant); 
		_anim.SetBool("Idle", _isIdle);
		_anim.SetFloat("Speed", _rb.velocity.sqrMagnitude);

	}

	private void FixedUpdate()
	{
		_controller.FixedUpdate();
		_fsm.UpdatePhysics();
	}

	private void OnCollisionStay(Collision collision)
	{
		_controller.OnCollisionStay(collision);
	}

	private void InitaliseFSM()
	{
		_fsm = new FSM.FSM();

		_dormantState = new DormantState(this);
		State idleState = new IdleState(this);
		State walkingState = new WalkingState(this);
		State pushingState = new PushingState(this);

		_fsm.AddTransition(_dormantState, idleState, () => { return !_dormant; });

		_fsm.AddTransition(idleState, _dormantState, () => { return _dormant; });
		_fsm.AddTransition(idleState, walkingState, () => { return _currentHeading != Vector3.zero; });

		_fsm.AddTransition(walkingState, _dormantState, () => _dormant);
		_fsm.AddTransition(walkingState, idleState, () =>
		{
			Vector3 vel = _rb.velocity;
			vel.y = 0f;
			return _currentHeading == Vector3.zero && vel == Vector3.zero;
		});

		// Pushing
		_fsm.AddTransition(idleState, pushingState, () =>
		{
			if (_inputData.pushButtonPressedThisFrame)
				return BeginPushing();

			return false;
		});

		_fsm.AddTransition(walkingState, pushingState, () =>
		{
			if (_inputData.pushButtonPressedThisFrame)
				return BeginPushing();

			return false;
		});

		_fsm.AddTransition(pushingState, idleState, () =>
		{
			if (_inputData.pushButtonPressedThisFrame)
				StopPushing();

			return _block == null;
		});

		_fsm.AddTransition(pushingState, idleState, () =>
		{
			if (_dormant)
				StopPushing();

			return _block == null;
		});

		_fsm.SetDefaultState(idleState);
	}

	private void ComputeAxes()
	{
		float _angle = _cameraTransform.rotation.eulerAngles.y;
		_forwardRelativeToCamera = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
		_rightRelativeToCamera = Vector3.Cross(Vector3.up, _forwardRelativeToCamera);
		_forwardRelativeToCharacter = _thisTransform.forward;
		_currentHeading = _inputData.normalizedAxes.x * _rightRelativeToCamera + _inputData.normalizedAxes.y * _forwardRelativeToCamera;
	}

	public void Move()
	{
		_controller.Move(_currentHeading);
	}

	private void Orientate(Quaternion targetRotation)
	{
		_thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, _targetRotation, _angularSpeed * Time.fixedDeltaTime);
	}

	public void OrientateToCamera()
	{
		if (_currentHeading != Vector3.zero)
		{
			_targetRotation = Quaternion.LookRotation(_currentHeading, Vector3.up);
			Orientate(_targetRotation);
		}
	}

	public void Enter()
	{
		if (_orbMesh != null)
			_orbMesh.SetActive(true);

		OnGolemActive?.Invoke(this);
		_emissionFade.OnActivate(); 
		_dormant = false;
	}

	public void Exit()
	{
		if (_orbMesh != null)
			_orbMesh.SetActive(false);

		_dormant = true;
		_emissionFade.OnDeactivate(); 
	}

	#region Pushing
	public bool BeginPushing()
	{
		RaycastHit hit;
		if (Physics.Raycast(_thisTransform.position + Vector3.up * 1.0f, _forwardRelativeToCharacter, out hit, _interactionDistance, LayerMap.block))
		{
			_block = hit.collider.GetComponent<Block>();

			_blockNormal = hit.normal;

			Vector3 newGolemPos = _block.transform.position + (_blockNormal * _offsetFromBlock);
			newGolemPos.y = _thisTransform.position.y;
			_thisTransform.position = newGolemPos;
			_thisTransform.rotation = Quaternion.LookRotation(-_blockNormal);
			_block.BeginPushing(this, _blockNormal, _characterControllerSettings.maxSpeed);
			_anim.SetBool("Pushing", true);
			return true;
		}

		return false;
	}

	float time;
	float dt = 0.1f;
	public void Push()
	{
		bool _blockCentered = Physics.Raycast(transform.position + Vector3.up * 0.85f, transform.forward, 2.0f, _blockLayer);

		if (_blockCentered == false)
		{
			StopPushing();
			return;
		}

		// Cleanup required
		if (_inputData.axes.y != 0f)
			time = Time.time;

		bool pushingIdle = _inputData.axes.y == 0f && Time.time > time + dt;

		_anim.SetFloat("Direction", _inputData.axes.y);
		_anim.SetBool("Pushing Idle", pushingIdle);
		_controller.Move(_inputData.axes.y * -_blockNormal / _block.mass);
		_block.Move(_rb.velocity * Time.fixedDeltaTime, _inputData.axes.y);
	}

	public void StopPushing()
	{
		if (_block != null)
			_block.StopPushing();
		_block = null;
		_anim.SetBool("Pushing", false);
	}
	#endregion

	public void SetAnimatorBool(string name, bool value)
	{
		_anim.SetBool(name, value);
	}

	public void SetInputData(PlayerInputData data)
	{
		_inputData = data;
	}

	public override bool IsActive()
	{
		return !_dormant;
	}

	public Vector3 GetVelocity()
	{
		return _rb.velocity; 
	}

	public bool GetDormant()
	{
		return _dormant; 
	}

	public bool GetIdle()
	{
		return _isIdle; 
	}
}

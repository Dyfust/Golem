using UnityEngine;
using GolemStates;
using FSM;

public class Golem : Player, IRequireInput, IReset
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

	[CustomHeader("States")]
	[SerializeField] private State _dormantState;

	[CustomHeader("References")]
	[SerializeField] private Animator _anim;

	[CustomHeader("Emission Anim")]
	[SerializeField] private GolemBloom _emissionRef; 
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

	private Rigidbody _rb;
	private Transform _thisTransform;

	private CharacterController _controller;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();

		_thisTransform = transform;
		_cameraTransform = Camera.main.transform;

		_controller = new CharacterController(_rb, _characterControllerSettings);
	}

	private void Start()
	{
		InitaliseFSM();
		_initPos = _thisTransform.position;

		if (_orbMesh != null)
			_orbMesh.SetActive(false);

		DebugWindow.AddPrintTask(() => "Golem Grounded: " + _controller.IsGrounded().ToString());
		DebugWindow.AddPrintTask(() => "Golem Ground Normal: " + _controller.GetCollisionNormal().ToString());
	}

	private void Update()
	{
		ComputeAxes();

		_fsm.HandleTransitions();
		_fsm.UpdateLogic();

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
		State liftingState = new LiftingState(this);

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

		// Lifting
		_fsm.AddTransition(idleState, liftingState, () =>
		{
			if (_inputData.liftButtonPressedThisFrame)
				return BeginLifting();

			return false;
		});

		_fsm.AddTransition(liftingState, idleState, () =>
		{
			return _inputData.liftButtonPressedThisFrame;
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
		_dormant = false;
		_emissionRef.OnActivate(); 
	}

	public void Exit()
	{
		if (_orbMesh != null)
			_orbMesh.SetActive(false);

		_dormant = true;
		_emissionRef.OnDeactivate(); 
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

	public void Push()
	{
		bool _blockCentered = Physics.Raycast(transform.position + Vector3.up * 0.85f, transform.forward, 2.0f, _blockLayer);

		if (_blockCentered == false)
		{
			StopPushing();
			return;
		}
		_anim.SetFloat("Direction", _inputData.axes.y);
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

	#region Lifting
	public bool BeginLifting()
	{
		RaycastHit hit;
		if (Physics.Raycast(_thisTransform.position + Vector3.up * 0.5f, _forwardRelativeToCamera, out hit, _interactionDistance, _blockLayer))
		{
			//_block = hit.collider.GetComponent<InteractableCube>(); 
			_block = hit.collider.GetComponent<Block>();
			_block.BeginLift();
			_blockNormal = hit.normal;

			_block.transform.position = this.transform.position + new Vector3(0, _liftingVerticalOffset, 0);

			//Vector3 newGolemPos = _block.transform.position + (_blockNormal * _distFromBlock);
			//newGolemPos.y = _thisTransform.position.y;
			//
			//_thisTransform.position = newGolemPos;
			//_thisTransform.rotation = Quaternion.LookRotation(-_blockNormal);
			//
			//_block.transform.position = _thisTransform.position + new Vector3(0, 2.5f, 0);

			return true;
		}

		return false;
	}

	public void Lift()
	{
		//_block.transform.position = _handJoint.position;
	}

	public void StopLifting()
	{
		_block.StopLift();
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

	private Vector3 _initPos;

	void IReset.Reset()
	{
		_fsm.MoveTo(_dormantState);
		_thisTransform.position = new Vector3(_initPos.x, _initPos.y, _initPos.z);
	}

	void IReset.OnEnter(Vector3 checkpointPos)
	{

	}

	public bool IsActive()
	{
		return !_dormant;
	}
}

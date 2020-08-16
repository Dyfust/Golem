using UnityEngine;
using GolemStates;
using FSM;

public class Golem : MonoBehaviour, IRequireInput
{
	[SerializeField] private float _angularSpeed = 0;
	[SerializeField] private CharacterControllerSettings _controllerSettings;
	private IMovementController _controller;

	private InputData _inputData;

	private Vector3 _forwardRelativeToCamera;
	private Vector3 _rightRelativeToCamera;
	private Vector3 _heading;

	private FSM.FSM _fsm;

	[SerializeField] private LayerMask _blockLayer;
	[SerializeField] private float _blockInteractionDistance;
	[SerializeField] private float _distFromBlock;
	[SerializeField] private Transform _handJoint;
	private InteractableCube _block;
	private Rigidbody _blockRigidbody;
	private FixedJoint _blockJoint;
	private Vector3 _blockNormal;
	private Vector3 _blockInitialPos;

	private bool _dormant = true;

	private Rigidbody _rb;
	[SerializeField] private Animator _anim;

	private Transform _thisTransform;
	private Transform _cameraTransform;
	[SerializeField] private GameObject _CMVirtualCamera;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();

		_thisTransform = transform;
		_cameraTransform = Camera.main.transform;

		_controller = new CharacterController(_rb, _controllerSettings);
	}

	private void Start()
	{
		InitaliseFSM();

		DebugWindow.AddPrintTask(() => { return "Golem State: " + _fsm.GetCurrentState().debugName; });
		DebugWindow.AddPrintTask(() => { return "Golem Heading: " + _heading.ToString(); });
		DebugWindow.AddPrintTask(() => { return "Golem Speed: " + _rb.velocity.magnitude.ToString(); });
		DebugWindow.AddPrintTask(() => { return "Golem Velocity: " + _rb.velocity.ToString(); });
		DebugWindow.AddPrintTask(() => { return "Orb Grounded: " + _controller.IsGrounded().ToString(); });
	}

	private void Update()
	{
		ComputeAxes();

		_fsm.UpdateLogic();
		_fsm.HandleTransitions();

		_anim.SetFloat("Speed", _rb.velocity.sqrMagnitude);
	}

	private void FixedUpdate()
	{
		_fsm.UpdatePhysics();
		_controller.FixedUpdate();
	}

	private void InitaliseFSM()
	{
		_fsm = new FSM.FSM();

		State dormantState = new DormantState(this);
		State idleState = new IdleState(this);
		State walkingState = new WalkingState(this);
		State pushingState = new PushingState(this);
		State liftingState = new LiftingState(this);

		_fsm.AddTransition(dormantState, idleState, () => { return !_dormant; });
		_fsm.AddTransition(idleState, dormantState, () => { return _dormant; });

		_fsm.AddTransition(idleState, walkingState, () => { return _heading != Vector3.zero; });
		_fsm.AddTransition(walkingState, idleState, () =>
		{
			Vector3 vel = _rb.velocity;
			vel.y = 0f;
			return _heading == Vector3.zero && vel == Vector3.zero;
		});

		// Pushing
		_fsm.AddTransition(idleState, pushingState, () =>
		{
			if (Input.GetKeyDown(KeyCode.E))
				return BeginPushing();

			return false;
		});

		_fsm.AddTransition(pushingState, idleState, () =>
		{
			return Input.GetKeyDown(KeyCode.E);
		});

		// Lifting
		_fsm.AddTransition(idleState, liftingState, () =>
		{
			if (Input.GetKeyDown(KeyCode.Q))
				return BeginLifting();

			return false;
		});

		_fsm.AddTransition(liftingState, idleState, () =>
		{
			return Input.GetKeyDown(KeyCode.Q);
		});

		_fsm.SetDefaultState(idleState);
	}

	private void ComputeAxes()
	{
		float _angle = _cameraTransform.rotation.eulerAngles.y;
		_forwardRelativeToCamera = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
		_rightRelativeToCamera = Vector3.Cross(Vector3.up, _forwardRelativeToCamera);

		_heading = _inputData.normalisedInput.x * _rightRelativeToCamera + _inputData.normalisedInput.y * _forwardRelativeToCamera;
	}

	public void Enter()
	{
		VirtualCameraManager.instance.ToggleVCam(_CMVirtualCamera);
		_dormant = false;
	}

	public void Exit()
	{
		_dormant = true;
	}

	public void Move()
	{
		_controller.Move(_heading);
	}

	Quaternion targetRotation = Quaternion.identity;
	public void Orientate()
	{
		if (_heading != Vector3.zero)
			targetRotation = Quaternion.LookRotation(_heading, Vector3.up);

		_thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);
	}

	public bool BeginPushing()
	{
		RaycastHit hit;
		if (Physics.Raycast(_thisTransform.position + Vector3.up * 0.5f, _forwardRelativeToCamera, out hit, _blockInteractionDistance, _blockLayer))
		{
			_block = hit.collider.GetComponent<InteractableCube>();

			_blockNormal = hit.normal;

			Vector3 newGolemPos = _block.transform.position + (_blockNormal * _distFromBlock);
			newGolemPos.y = _thisTransform.position.y;

			_thisTransform.position = newGolemPos;
			_thisTransform.rotation = Quaternion.LookRotation(-_blockNormal);
			_block.BeginPushing(_rb);
			return true;
		}

		return false;
	}

	public void Push()
	{
		_controller.Move(_inputData.input.y * -_blockNormal / _block.mass);
	}

	public void StopPushing()
	{
		_block.StopPushing();
		_block = null;
	}

	public bool BeginLifting()
	{
		RaycastHit hit;
		if (Physics.Raycast(_thisTransform.position + Vector3.up * 0.5f, _forwardRelativeToCamera, out hit, _blockInteractionDistance, _blockLayer))
		{
			_block = hit.collider.GetComponent<InteractableCube>(); 
			_blockNormal = hit.normal;

			//Vector3 newGolemPos = _blockRigidbody.position + (_blockNormal * _distFromBlock);
			//newGolemPos.y = _thisTransform.position.y;

			//_thisTransform.position = newGolemPos;
			//_thisTransform.rotation = Quaternion.LookRotation(-_blockNormal);

			//_blockRigidbody.position = _thisTransform.position + new Vector3(0, 2.5f, 0);

			_block.BeginLifting();
			return true;
		}

		return false;
	}

	public void Lift()
	{
		_block.transform.position = _handJoint.position;
	}

	public void StopLifting()
	{
		_block.StopLifing(); 
	}

	public void ResetState()
	{
		_controller.Move(Vector3.zero);
		//_rb.velocity = Vector3.zero;
	}

	public void SetAnimatorBool(string name, bool value)
	{
		_anim.SetBool(name, value);
	}

	// Interfaces
	public void SetInputData(InputData data)
	{
		_inputData = data;
	}

	private void OnCollisionStay(Collision collision)
	{
		_controller.OnCollisionEnter(collision);
	}
}
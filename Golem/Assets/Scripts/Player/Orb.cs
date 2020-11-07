using OrbStates;
using UnityEngine;
using FSM;
using System.Collections;

public class Orb : Player, IRequireInput, IReset
{
	public delegate void OrbEventHandler(Orb orb);
	public static event OrbEventHandler OnOrbActive;

	// --------------------------------------------------------------
	[CustomHeader("Movement")]
	[SerializeField] private CharacterControllerSettings _controllerSettings;

	[CustomHeader("Interact Settings")]
	[SerializeField] private float _interactionDistance;
	[SerializeField] private float _orientationSpeed; 
	[SerializeField] private Transform _meshTransform;

	[CustomHeader("Audio Emitters")]
	[SerializeField] private OneShotEmitter _golemAttachmentSFX;
	[SerializeField] private AudioEmitter _rollingSFX;

	[CustomHeader("References")]
	[SerializeField] private Animator _anim; 

	// --------------------------------------------------------------
	private State _idleState;

	private PlayerInputData _inputData;

	private Transform _cameraTransform;
	private Vector3 _forwardRelativeToCamera;
	private Vector3 _rightRelativeToCamera;
	private Vector3 _currentHeading;

	private Quaternion _targetRotation;

	private FSM.FSM _fsm;
	private Golem _currentGolem;
	private bool _isIdle;
	private const float _exitEnterCD = 1f;
	private float _exitEnterTimeStamp;

	// Resetting
	private Vector3 _checkpointPos;

	private Rigidbody _rb;
	private Transform _thisTransform;

	private CharacterController _controller;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();

		_thisTransform = transform;
		_cameraTransform = Camera.main.transform;

		_controller = new CharacterController(_rb, _controllerSettings);
	}

	private void Start()
	{
		InitialiseFSM();

		DebugWindow.AddPrintTask(() => "Orb State: " + _fsm.GetCurrentState().debugName);
		DebugWindow.AddPrintTask(() => "Orb Heading: " + _currentHeading.ToString());
		DebugWindow.AddPrintTask(() => "Orb Velocity: " + _rb.velocity.ToString());
		DebugWindow.AddPrintTask(() => "Orb Speed: " + _rb.velocity.magnitude.ToString());
		DebugWindow.AddPrintTask(() => "Orb Grounded: " + _controller.IsGrounded().ToString());
		DebugWindow.AddPrintTask(() => "Orb Ground Normal: " + _controller.GetCollisionNormal().ToString());
	}


	float idleTimeStamp;
	float idleDt = 0.1f; 
	private void Update()
	{
		ComputeAxes();

		//Clean up required. 
		if (_rb.velocity.sqrMagnitude >= 0.1f)
			idleTimeStamp = Time.time;

		_isIdle = _rb.velocity.sqrMagnitude < 0.1f && Time.time > idleTimeStamp + idleDt;
		if (_anim != null)
			_anim.SetBool("Idle", _isIdle);

		_fsm.HandleTransitions();
		_fsm.UpdateLogic();
	}

	private void FixedUpdate()
	{
		_fsm.UpdatePhysics();
		_controller.FixedUpdate();
	}

	private void InitialiseFSM()
	{
		_fsm = new FSM.FSM();

		_idleState = new IdleState(this);
		State rollingState = new RollingState(this, _rollingSFX);
		State mountedState = new MountedState(this);

		_fsm.AddTransition(_idleState, rollingState, IsRollingOnSurface);
		_fsm.AddTransition(rollingState, _idleState, IsNotRolling);

		_fsm.AddTransition(_idleState, mountedState, IsEnteringGolem);
		_fsm.AddTransition(rollingState, mountedState, IsEnteringGolem);

		_fsm.AddTransition(mountedState, _idleState, IsExitingGolem);

		_fsm.SetDefaultState(_idleState);
	}

	private bool IsRollingOnSurface()
	{
		return _currentHeading != Vector3.zero;
	}

	private bool IsNotRolling()
	{
		return !IsRollingOnSurface() && _controller.GetVelocity() == Vector3.zero;
	}

	private bool IsEnteringGolem()
	{
		if (_inputData.enterButtonPressedThisFrame && Time.time >= _exitEnterTimeStamp + _exitEnterCD)
		{
			return FindGolem();
		}

		return false;
	}

	private bool IsExitingGolem()
	{
		return _inputData.enterButtonPressedThisFrame && _currentGolem != null;
	}

	private void ComputeAxes()
	{
		float angle = _cameraTransform.rotation.eulerAngles.y;
		_forwardRelativeToCamera = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
		_rightRelativeToCamera = Vector3.Cross(Vector3.up, _forwardRelativeToCamera);
		_currentHeading = _inputData.axes.x * _rightRelativeToCamera + _inputData.axes.y * _forwardRelativeToCamera;
	}

	public void Roll()
	{
		Vector3 rotAxis = Vector3.Cross(Vector3.up, _controller.GetVelocity().normalized);
		float targetAngularVelocity = _controller.GetVelocity().magnitude / 0.5f;

		_meshTransform.Rotate(rotAxis, Mathf.Rad2Deg * targetAngularVelocity * Time.fixedDeltaTime, Space.World);
	}

	public void OrientateMeshToCamera()
	{
		Vector3 temp = _meshTransform.up;
		temp.x = 0;
		temp.z = 0;
		Quaternion end = Quaternion.LookRotation(-_cameraTransform.forward, Vector3.up);
		_meshTransform.rotation = Quaternion.RotateTowards(_meshTransform.rotation, end, Time.fixedDeltaTime * _orientationSpeed);
	}

	public void OrientateToGolem()
	{
		_targetRotation = Quaternion.LookRotation(_currentGolem.transform.forward, Vector3.up);
		_meshTransform.transform.rotation = _targetRotation;
	}

	public void Move()
	{
		_controller.Move(_currentHeading);
	}

	public void ResetMovement()
	{
		_controller.Move(Vector3.zero);
		_rb.velocity = Vector3.zero;
	}

	private bool FindGolem()
	{
		Collider[] golems = Physics.OverlapSphere(_thisTransform.position, _interactionDistance, LayerMap.golemLayer);

		for (int i = 0; i < golems.Length; i++)
		{
			Vector3 thisToGolem = (golems[i].transform.position + Vector3.up * 0.5f) - _thisTransform.position;
			if (Physics.Raycast(_thisTransform.position, thisToGolem.normalized, out RaycastHit hit, _interactionDistance, ~(LayerMap.orbLayer | LayerMap.pressurePlateLayer | LayerMap.invisRampLayer)))
			{
				if (hit.collider.CompareTag("Golem"))
				{
					_currentGolem = hit.collider.GetComponent<Golem>();
					return true;
				}
			}
		}

		return false;
	}

	public void EnterGolem()
	{
		_controller.Toggle(false);
		GetComponent<Collider>().enabled = false;
		_meshTransform.gameObject.SetActive(false);
		_currentGolem.Enter();

		_golemAttachmentSFX.Play();

		_exitEnterTimeStamp = Time.time;
	}

	public void ExitGolem()
	{
		_thisTransform.position = _currentGolem.transform.position + _currentGolem.attachmentOffset;
		GetComponent<Collider>().enabled = true;
		_meshTransform.gameObject.SetActive(true);

		_currentGolem.Exit();
		_currentGolem = null;

		OnOrbActive?.Invoke(this);

		_controller.Toggle(true);

		_exitEnterTimeStamp = Time.time;

	}

	// Interfaces
	void IRequireInput.SetInputData(PlayerInputData data)
	{
		_inputData = data;
	}

	void IReset.Reset()
	{
		_fsm.MoveTo(_idleState);
		_thisTransform.position = new Vector3(_checkpointPos.x, _checkpointPos.y, _checkpointPos.z);
	}

	void IReset.OnEnter(Vector3 checkpointPos)
	{
		_checkpointPos = checkpointPos;
	}

	private void OnCollisionStay(Collision collision)
	{
		_controller.OnCollisionStay(collision);
	}

	public override bool IsActive()
	{
		return true;
	}

	public bool IsGrounded() => _controller.IsGrounded();
	public float GetMaxSpeed() => _controller.GetMaxSpeed();
	public Vector3 GetVelocity() => _controller.GetVelocity();
}
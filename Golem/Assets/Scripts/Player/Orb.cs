using OrbStates;
using UnityEngine;
using FSM;

public class Orb : MonoBehaviour, IRequireInput, IReset
{
    public delegate void OrbEventHandler(Orb orb);
    public static event OrbEventHandler OnOrbActive;

    [SerializeField] private CharacterControllerSettings _controllerSettings;
    [SerializeField] private float _angularSpeed;
    private CharacterController _controller;

    private InputData _inputData;

    private float _angle;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _currentHeading; public Vector3 currentHeading => _currentHeading;
    private Quaternion _targetRotation;

    [SerializeField] private float _interactionRadius;
    [SerializeField] private Vector3 _attachmentOffset;
    private Golem _currentGolem;

    private Rigidbody _rb;
    private Transform _thisTransform;
    private Transform _cameraTransform;

    private FSM.FSM _fsm;

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
        DebugWindow.AddPrintTask(() => "Orb Grounded: " + _controller.IsGrounded().ToString());
        DebugWindow.AddPrintTask(() => "Orb Ground Normal: " + _controller.GetCollisionNormal().ToString());
    }

    private void Update()
    {
        ComputeAxes();

        _fsm.HandleTransitions();
        _fsm.UpdateLogic();

        _inputData.enterButtonPress = false;
    }

    private void FixedUpdate()
    {
        _fsm.UpdatePhysics();
    }

    private State _idleState;

    private void InitialiseFSM()
    {
        _fsm = new FSM.FSM();

        State idleState = new IdleState(this);
        _idleState = idleState;
        State rollingState = new RollingState(this);
        State mountedState = new MountedState(this);

        _fsm.AddTransition(idleState, rollingState, () =>
        {
            return _currentHeading != Vector3.zero;
        });

        _fsm.AddTransition(rollingState, idleState, () =>
        {
            return _currentHeading == Vector3.zero;
        });

        _fsm.AddTransition(idleState, mountedState, () =>
        {
            if (_inputData.enterButtonPress)
                return EnterGolem();

            return false;
        });

        _fsm.AddTransition(rollingState, mountedState, () =>
        {
            if (_inputData.enterButtonPress)
                return EnterGolem();

            return false;
        });

        _fsm.AddTransition(mountedState, idleState, () =>
        {
            return _inputData.enterButtonPress;
        });

        _fsm.SetDefaultState(idleState);
    }

    private void ComputeAxes()
    {
        _angle = _cameraTransform.rotation.eulerAngles.y;
        _forward = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
        _right = Vector3.Cross(Vector3.up, _forward);

        _currentHeading = _inputData.normalisedMovement.x * _right + _inputData.normalisedMovement.y * _forward;
    }

    public void UpdateController()
    {
        _controller.FixedUpdate();
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

    public void OrientateToGolem()
    {
        _targetRotation = Quaternion.LookRotation(_currentGolem.transform.forward, Vector3.up);
        Orientate(_targetRotation);
    }

    public void Move()
    {
        _controller.Move(_currentHeading);
    }

    public void ResetState()
    {
        _controller.Move(Vector3.zero);
    }

    public void ResetVelocity()
    {
        _rb.velocity = Vector3.zero;
    }

    private bool FindGolem()
    {
        Collider[] golems = Physics.OverlapSphere(_thisTransform.position, _interactionRadius, LayerMap.golemLayer);

        for (int i = 0; i < golems.Length; i++)
        {
            Vector3 thisToGolem = (golems[i].transform.position + Vector3.up * 0.5f) - _thisTransform.position;
            if (Physics.Raycast(_thisTransform.position, thisToGolem.normalized, out RaycastHit hit, _interactionRadius, ~(LayerMap.orbLayer | LayerMap.pressurePlateLayer | LayerMap.invisRampLayer)))
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

    public bool EnterGolem()
    {
        if (FindGolem())
        {
            _rb.useGravity = false;
            _rb.velocity = Vector3.zero;

            GetComponent<Collider>().enabled = false;

            _currentGolem.Enter();

            return true;
        }

        return false;
    }

    public void StickToGolem()
    {
        _rb.position = _currentGolem.transform.position + _attachmentOffset;
    }

    public void ExitGolem()
    {
        _currentGolem.Exit();
        _currentGolem = null;

        GetComponent<Collider>().enabled = true;
        _rb.useGravity = true;

        OnOrbActive?.Invoke(this);
    }

    // Interfaces
    public void SetInputData(InputData data)
    {
        _inputData = data;
    }

    private void OnCollisionStay(Collision collision)
    {
        _controller.OnCollisionStay(collision);
    }

    private Vector3 _checkpointPos;

    void IReset.Reset()
    {
        _fsm.MoveTo(_idleState);

        ResetVelocity();

        _thisTransform.position = new Vector3(_checkpointPos.x, _checkpointPos.y, _checkpointPos.z);
    }

    void IReset.OnEnter(Vector3 checkpointPos)
    {
        _checkpointPos = checkpointPos;
    }

    public void EnterthisGolem()
    {
        return;
    }
}
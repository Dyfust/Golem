using OrbStates;
using UnityEngine;
using FSM;

public class Orb : MonoBehaviour, IRequireInput
{
    [SerializeField] private CharacterControllerSettings _controllerSettings;
    [SerializeField] private float _angularSpeed;
    private IMovementController _controller;

    private InputData _inputData;

    private float _angle;
    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _currentHeading; public Vector3 currentHeading => _currentHeading;


    [SerializeField] private LayerMask _golemLayer;
    [SerializeField] private float _interactionRadius;
    [SerializeField] private Vector3 _attachmentOffset;
    private Golem _currentGolem;

    private Rigidbody _rb;
    private Transform _thisTransform;
    private Transform _cameraTransform;
    [SerializeField] private GameObject _CMVirtualCamera;

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
    }

    private void Update()
    {
        ComputeAxes();

        _fsm.HandleTransitions();
        _fsm.UpdateLogic();
    }

    private void FixedUpdate()
    {
        _fsm.UpdatePhysics();
    }

    private void InitialiseFSM()
    {
        _fsm = new FSM.FSM();

        State idleState = new IdleState(this);
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
            if (Input.GetKeyDown(KeyCode.F))
                return EnterGolem();

            return false;
        });

        _fsm.AddTransition(mountedState, idleState, () => 
        {
            return Input.GetKeyDown(KeyCode.F);
        });

        _fsm.SetDefaultState(idleState);
    }

    private void ComputeAxes()
    {
        _angle = _cameraTransform.rotation.eulerAngles.y;
        _forward = Quaternion.AngleAxis(_angle, Vector3.up) * Vector3.forward;
        _right = Vector3.Cross(Vector3.up, _forward);

        _currentHeading = _inputData.normalisedInput.x * _right + _inputData.normalisedInput.y * _forward;
    }

    public void UpdateController()
    {
        _controller.FixedUpdate();
    }

    public void Orientate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_forward, Vector3.up);
        _thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, targetRotation, _angularSpeed * Time.fixedDeltaTime);
    }

    public void Move()
    {
        _controller.Move(_currentHeading);
    }

    public void ResetState()
    {
        _controller.Move(Vector3.zero);
        //_rb.velocity = Vector3.zero;
    }

    public void ResetVelocity()
    {
        _rb.velocity = Vector3.zero;
    }
    
    private bool FindGolem()
    {
        Collider[] golems = Physics.OverlapSphere(_thisTransform.position, _interactionRadius, _golemLayer);

        for (int i = 0; i < golems.Length; i++)
        {
            Vector3 thisToGolem = (golems[i].transform.position + Vector3.up * 0.5f) - _thisTransform.position;
            if (Physics.Raycast(_thisTransform.position, thisToGolem.normalized, out RaycastHit hit, _interactionRadius, ~(LayerMap.orbLayer | LayerMap.pressurePlateLayer)))
            {
                Debug.Log("Yes");
                if (hit.collider.GetComponent<Golem>())
                {
                    Debug.Log("Yes");
                    _currentGolem = golems[i].GetComponent<Golem>();
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
        _rb.useGravity = true;

        VirtualCameraManager.instance.ToggleVCam(_CMVirtualCamera);
    }

    // Interfaces
    public void SetInputData(InputData data)
    {
        if (_currentGolem != null)
            _currentGolem.SetInputData(data);
        else
            _inputData = data;
    }

    private void OnCollisionStay(Collision collision)
    {
        _controller.OnCollisionStay(collision);
    }
}
using UnityEngine;
using GolemStates;
using FSM;

public class Golem : MonoBehaviour, IRequireInput
{
    public delegate void GolemEventHandler(Golem golem, Quaternion orientation);
    public static event GolemEventHandler OnGolemActive;

    [SerializeField] private float _angularSpeed = 0;
    [SerializeField] private CharacterControllerSettings _controllerSettings;

    private InputData _inputData;

    private Vector3 _forwardRelativeToCamera;
    private Vector3 _rightRelativeToCamera;
    private Vector3 _heading; public Vector3 heading => _heading;
    private Quaternion _targetRotation;

    [SerializeField] private LayerMask _blockLayer;
    [SerializeField] private float _blockInteractionDistance;
    [SerializeField] private float _distFromBlock;
    [SerializeField] private Transform _handJoint;
    private Block _block;
    private Vector3 _blockNormal;

    private FSM.FSM _fsm;

    private bool _dormant = true;

    [SerializeField] private Animator _anim;
    private Rigidbody _rb;
    private CharacterController _controller;

    private Transform _thisTransform;
    private Transform _cameraTransform;

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

        DebugWindow.AddPrintTask(() => "Orb Grounded: " + _controller.IsGrounded().ToString());
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
        _fsm.UpdatePhysics();
        _controller.FixedUpdate();
    }

    private void OnCollisionStay(Collision collision)
    {
        _controller.OnCollisionStay(collision);
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

        _fsm.AddTransition(walkingState, dormantState, () => _dormant);
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

        _fsm.AddTransition(walkingState, pushingState, () =>
        {
            if (Input.GetKeyDown(KeyCode.E))
                return BeginPushing();

            return false;
        });

        _fsm.AddTransition(pushingState, idleState, () =>
        {
            if (Input.GetKeyDown(KeyCode.E))
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

    public void Move()
    {
        _controller.Move(_heading);
    }

    private void Orientate(Quaternion targetRotation)
    {
        _thisTransform.rotation = Quaternion.Slerp(_thisTransform.rotation, _targetRotation, _angularSpeed * Time.fixedDeltaTime);
    }

    public void OrientateToCamera()
    {
        if (_heading != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(_heading, Vector3.up);
            Orientate(_targetRotation);
        }
    }

    public void Enter()
    {
        OnGolemActive?.Invoke(this, transform.rotation);
        _dormant = false;
    }

    public void Exit()
    {
        _dormant = true;
    }

    #region Pushing
    public bool BeginPushing()
    {
        RaycastHit hit;
        if (Physics.Raycast(_thisTransform.position + Vector3.up * 0.5f, _forwardRelativeToCamera, out hit, _blockInteractionDistance, LayerMap.block))
        {
            _block = hit.collider.GetComponent<Block>();

            _blockNormal = hit.normal;

            Vector3 newGolemPos = _block.transform.position + (_blockNormal * _distFromBlock);
            newGolemPos.y = _thisTransform.position.y;

            _thisTransform.position = newGolemPos;
            _thisTransform.rotation = Quaternion.LookRotation(-_blockNormal);
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

        _controller.Move(_inputData.input.y * -_blockNormal / _block.mass);
        _block.Move(_controller.GetVelocity() * Time.fixedDeltaTime, this);
    }

    public void StopPushing()
    {
        _block.StopPushing();
        _block = null;
    }
    #endregion

    #region Lifting
    public bool BeginLifting()
    {
        RaycastHit hit;
        if (Physics.Raycast(_thisTransform.position + Vector3.up * 0.5f, _forwardRelativeToCamera, out hit, _blockInteractionDistance, _blockLayer))
        {
            //_block = hit.collider.GetComponent<InteractableCube>(); 
            _block = hit.collider.GetComponent<Block>();
            _block.BeginLift();
            _blockNormal = hit.normal;

            _block.transform.position = this.transform.position + new Vector3(0, 3.2f, 0);

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

    public void SetInputData(InputData data)
    {
        _inputData = data;
    }
}
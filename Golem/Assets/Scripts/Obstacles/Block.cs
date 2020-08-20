using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float _mass; public float mass => _mass;

    private UnityEngine.CharacterController _cc;
	private Collider _coll; 

	private float _gravity = 10;

	private Vector3 _initialPos;

	private bool _isGrounded = false;
	private bool _isConnected = false;
	private bool _isLifted = false; 

	private RaycastHit _hit;

	private Golem _connectedGolem; 

	private void Start()
	{
		_cc = GetComponent<UnityEngine.CharacterController>();
		_coll = GetComponent<BoxCollider>(); 
	}

	private void Update()
	{
		if (_connectedGolem != null)
		{
			if (Vector3.Distance(this.transform.position, _connectedGolem.transform.position) > 3f)
			{
				_isConnected = false;
				Disconnect(); 
			}
		}
	}

	private void FixedUpdate()
	{
		_isGrounded = Physics.BoxCast(transform.position, _coll.bounds.size * 0.5f, Vector3.down, out _hit, Quaternion.identity, 0.1f, int.MaxValue, QueryTriggerInteraction.Ignore);

		Debug.Log(_isGrounded ? "Connected" : "Not Connected");

		//Applying gravity only if the block is not on the ground or not being lifted! 
		if (_isGrounded == false && _isLifted == false)
			_cc.Move(Vector3.down * _gravity * Time.fixedDeltaTime);
	}

	public void Move(Vector3 velocity, Golem golem)
	{
		_cc.Move(velocity);
		_isConnected = true;
		_connectedGolem = golem; 
	}

	public void StopPushing()
	{
		_isConnected = false;
		_connectedGolem = null; 
	}

	public void BeginLift()
	{
		_initialPos = transform.position; 
		_isLifted = true;
	}

	public void StopLift()
	{
		transform.position = _initialPos;
		_isLifted = false;
	}

	public void Disconnect()
	{
		_connectedGolem.StopPushing(); 
	}
}

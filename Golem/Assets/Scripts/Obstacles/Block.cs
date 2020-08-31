using UnityEngine;

public class Block : MonoBehaviour, IReset
{
    [SerializeField] private float _mass; public float mass => _mass;

    private UnityEngine.CharacterController _cc;
	private Collider _coll; 

	private float _gravity = 10;

	private Vector3 _preLiftPos;
	private Vector3 _startPos; 

	private bool _isGrounded = false;
	private bool _isConnected = false;
	private bool _isLifted = false; 

	private RaycastHit _hit;

	private Golem _connectedGolem; 

	private void Start()
	{
		_cc = GetComponent<UnityEngine.CharacterController>();
		_coll = GetComponent<BoxCollider>();
		_startPos = transform.position; 
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		_isGrounded = Physics.BoxCast(transform.position + Vector3.up * _coll.bounds.size.y * 0.5f, _coll.bounds.size * 0.5f, Vector3.down, out _hit, Quaternion.identity, 0.1f, int.MaxValue, QueryTriggerInteraction.Ignore);


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
		_preLiftPos = transform.position; 
		_isLifted = true;
	}

	public void StopLift()
	{
		transform.position = _preLiftPos;
		_isLifted = false;
	}

	void IReset.Reset()
	{
		_cc.enabled = false;
		transform.position = _startPos; 
		_isConnected = false;
		_isLifted = false;
		_cc.enabled = true;
	}

	void IReset.OnEnter()
	{
		_startPos = transform.position; 
	}
}

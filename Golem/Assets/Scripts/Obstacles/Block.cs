using UnityEngine;
using UnityEngine.ProBuilder;

public class Block : MonoBehaviour, IReset
{
    [SerializeField] private float _mass; public float mass => _mass;
	[SerializeField] ParticleSystem _particles;

	private float _maxEmissionRate; 
	private float _maxSpeed = 0;
	private ParticleSystem.EmissionModule _emissionModule;
	private ParticleSystem.MinMaxCurve _emissionCurve; 


	private UnityEngine.CharacterController _cc;
	private Collider _coll; 

	private float _gravity = 10;

	private Vector3 _preLiftPos;
	private Vector3 _startPos;
	private Vector3 _pushingNormal; 

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
		_particles.transform.position = this.transform.position;
		_emissionCurve = _particles.emission.rateOverTime; 
		_maxEmissionRate = _particles.emission.rateOverTime.constant;
		_emissionModule = _particles.emission;
		_particles.Stop(); 
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

	public void BeginPushing(Golem golem, Vector3 blockNormal, float maxSpeed)
	{
		_isConnected = true;
		_connectedGolem = golem;

		_maxSpeed = maxSpeed; 
		_particles.Play();

		_pushingNormal = blockNormal; 
	}

	public void Move(Vector3 velocity, float direction)
	{
		_cc.Move(velocity);

		if (direction != 0)
		{
			_particles.transform.rotation = Quaternion.LookRotation(_pushingNormal * direction, Vector3.up);
			_particles.transform.position = (this.transform.position + _coll.bounds.extents.x * (_pushingNormal * direction));
		}


		_emissionCurve.constant = ((velocity.magnitude/ Time.fixedDeltaTime) / _maxSpeed) * _maxEmissionRate;
		_emissionModule.rateOverTime = _emissionCurve; 
	}

	public void StopPushing()
	{
		_isConnected = false;
		_connectedGolem = null;
		_particles.Stop();
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

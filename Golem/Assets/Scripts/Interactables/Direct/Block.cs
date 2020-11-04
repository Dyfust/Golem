using UnityEngine;

/// <summary>
/// Intractable block. 
/// Attaches it self to an active Golem with interacted with. 
/// </summary>
public class Block : MonoBehaviour
{
	//-----------------------------------------------------------------
	[CustomHeader("Physics Properties")]
    [SerializeField] private float _mass; public float mass => _mass;

	[CustomHeader("Mesh")]
	[SerializeField] private GameObject _mesh;
	[SerializeField] private float _meshSmoothingSpeed; 

	[CustomHeader("VFX")]
	[SerializeField] private EmissionFade _emissionFade;

	[CustomHeader("Audio")]
	[SerializeField] private AudioEmitter _movingSound;

	[CustomHeader("Particles")]
	[SerializeField] private ParticleSystem _pebbles;
	//-----------------------------------------------------------------
	private float _maxEmissionRate; 
	private float _maxSpeed = 0;
	private ParticleSystem.EmissionModule _emissionModule;
	private ParticleSystem.MinMaxCurve _emissionCurve; 

	private UnityEngine.CharacterController _cc;
	private Collider _coll; 

	private float _gravity = 10;

	private Vector3 _startPos;
	private Vector3 _pushingNormal;
	private Vector3 _velocity; 

	private bool _isMoving = false;

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
		_pebbles.transform.position = this.transform.position;
		_emissionCurve = _pebbles.emission.rateOverTime; 
		_maxEmissionRate = _pebbles.emission.rateOverTime.constant;
		_emissionModule = _pebbles.emission;
		_pebbles.Stop();
	}

	private void Update()
	{
		_mesh.transform.position = Vector3.MoveTowards(_mesh.transform.position, this.transform.position - new Vector3(0, _hit.distance, 0), _meshSmoothingSpeed * Time.fixedDeltaTime);
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
		_pushingNormal = blockNormal;

		_emissionFade.OnActivate();
		_pebbles.Play();
	}

	public void Move(Vector3 velocity, float direction)
	{
		_cc.Move(velocity);

		if (_cc.velocity != Vector3.zero && !_isMoving)
			StartedMoving();
		else if (_cc.velocity == Vector3.zero && _isMoving)
			StoppedMoving();
		
		if (direction != 0)
		{
			_pebbles.transform.rotation = Quaternion.LookRotation(_pushingNormal * direction, Vector3.up);
			_pebbles.transform.position = (this.transform.position + _coll.bounds.extents.x * (_pushingNormal * direction));
		}

		_emissionCurve.constant = ((velocity.magnitude/ Time.fixedDeltaTime) / _maxSpeed) * _maxEmissionRate;
		_emissionModule.rateOverTime = _emissionCurve;
	}

	public void StopPushing()
	{
		_isConnected = false;
		_connectedGolem = null;
		_pebbles.Stop();

		_emissionFade.OnDeactivate();

		StoppedMoving();
	}

	private void StartedMoving()
	{
		_isMoving = true;
		_movingSound?.Play();
		Debug.Log("Started moving block");
	}

	private void StoppedMoving()
	{
		_isMoving = false;
		_movingSound?.Stop();
		Debug.Log("Stopped moving block");
	}

	public bool IsConnected()
	{
		return _isConnected; 
	}
}

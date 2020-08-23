using UnityEngine;

/// <summary>
/// Class interactions:
/// Push/pull has to be able to move 
/// listed has to be able to be lifted then hold its position above the player 
/// Interact with pressure plates 
/// Fall when it over open air 
/// Be immovable unless interacted with the Golem 
/// 
/// </summary>


public class InteractableCube : MonoBehaviour
{
	public enum STATE
	{
		PUSH,
		LIFT,
		FREE
	}
	[SerializeField] private float _mass; public float mass => _mass;
	[SerializeField] private LayerMask _groundLayer;
	//[SerializeField] private GameObject _mesh;

	private Golem _golem;

	private STATE _currentState = STATE.FREE;

	private FixedJoint _fj;
	private Rigidbody _rb;
	private BoxCollider _coll;

	private bool _isGrounded = false;

	private RaycastHit _hit;
	private float _rayPadding = 0.1f;

	private Vector3 _initalPos;

	void Start()
	{
		_fj = GetComponent<FixedJoint>();
		_rb = GetComponent<Rigidbody>();
		_coll = GetComponent<BoxCollider>();

		transform.position += Vector3.up * 0.25f;
	}

	private void OnDrawGizmos()
	{
		////Draw a Ray forward from GameObject toward the hit
		//Gizmos.DrawRay(transform.position, Vector3.down * _hit.distance);
		////Draw a cube that extends to where the hit exists
		//Gizmos.DrawWireCube(transform.position + Vector3.down * _hit.distance, transform.localScale);

		//float halfLength = transform.localScale.x / 2;
		//Debug.DrawRay(transform.position + new Vector3(halfLength, -halfLength, halfLength), Vector3.down * _rayPadding); 
		//Debug.DrawRay(transform.position + new Vector3(halfLength, -halfLength, -halfLength), Vector3.down * _rayPadding); 
		//Debug.DrawRay(transform.position + new Vector3(-halfLength, -halfLength, -halfLength), Vector3.down * _rayPadding); 
		//Debug.DrawRay(transform.position + new Vector3(-halfLength, -halfLength, halfLength), Vector3.down * _rayPadding);



	}


	private void FixedUpdate()
	{
		//_isGrounded = Physics.BoxCast(transform.position, _coll.bounds.size * 0.5f, Vector3.down, out _hit, Quaternion.identity, 0.1f, int.MaxValue, QueryTriggerInteraction.Ignore);
		//_mesh.transform.position = this.transform.position + Vector3.down * _hit.distance;


		float halfLength = transform.localScale.x / 2;
		bool _topRight = Physics.Raycast(transform.position + new Vector3(halfLength, -halfLength, halfLength), Vector3.down, _rayPadding, _groundLayer);
		bool _bottomRight = Physics.Raycast(transform.position + new Vector3(halfLength, -halfLength, -halfLength), Vector3.down, _rayPadding, _groundLayer);
		bool _bottomLeft = Physics.Raycast(transform.position + new Vector3(-halfLength, -halfLength, -halfLength), Vector3.down, _rayPadding, _groundLayer);
		bool _topLeft = Physics.Raycast(transform.position + new Vector3(-halfLength, -halfLength, halfLength), Vector3.down, _rayPadding, _groundLayer);

		//Debug.Log("TL" + _topLeft);
		//Debug.Log("TR" + _topRight);
		//Debug.Log("BL" + _bottomLeft);
		//Debug.Log("BR" + _bottomRight);

		if (!_topLeft || !_topRight || !_bottomLeft || !_bottomRight)
			_isGrounded = false;

		if (_topLeft || _topRight || _bottomLeft || _bottomRight)
			_isGrounded = true;


		UpdateState();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag.Equals("PressurePlate"))
		{
			Debug.Log("ONPRESSUREPLATE");
			_isGrounded = true; 
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		//if (collision.gameObject.tag.Equals("PressurePlate"))
		//	_rayPadding = 0.1f; 
	}

	public void BeginPushing(Rigidbody golemRb)
	{
		if (_fj == null)
			_fj = gameObject.AddComponent<FixedJoint>();

		_golem = golemRb.GetComponent<Golem>();
		_fj.connectedBody = golemRb;
		_currentState = STATE.PUSH; 
	}

	public void StopPushing()
	{
		_rb.velocity = Vector3.zero;
		if (_fj != null)
			Destroy(_fj);

		_rb.velocity = Vector3.zero;

		_golem = null;
		_currentState = STATE.FREE; 
	}

	public void BeginLifting()
	{
		_initalPos = transform.position;
		_coll.enabled = false;
		_currentState = STATE.LIFT;
	}

	public void StopLifing()
	{
		transform.position = _initalPos;
		_coll.enabled = true; 
		_currentState = STATE.FREE; 
	}

	private void UpdateState()
	{
		switch (_currentState)
		{
			case STATE.FREE:
				{
					if (_isGrounded)
					{
						_rb.isKinematic = true;
						_rb.velocity = Vector3.zero;
					}
					else
						_rb.isKinematic = false;
					break;
				}
			case STATE.PUSH:
				{
					if (_isGrounded)
					{
						_rb.isKinematic = false; 
					}
					else
					{
						_golem.StopPushing();
					}
					break; 
				}
			case STATE.LIFT:
				{
					if (_fj != null)
						Destroy(_fj);
					_rb.isKinematic = true; 
					break; 
				}
		}
	}
}

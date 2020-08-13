using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
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
	[SerializeField] private GameObject _mesh; 

	private STATE _currentState = STATE.FREE;

	private FixedJoint _fj;
	private Rigidbody _rb;
	private Collider _coll; 

	private ContactPoint[] _contacts;
	private bool _isGrounded = false;

	private RaycastHit _hit;

	private Vector3 _initalPos; 

	void Start()
	{
		_fj = GetComponent<FixedJoint>();
		_rb = GetComponent<Rigidbody>();
		_coll = GetComponent<Collider>(); 
	}

	void Update()
	{

	}

	private void OnDrawGizmos()
	{
		//Draw a Ray forward from GameObject toward the hit
		Gizmos.DrawRay(transform.position, Vector3.down * _hit.distance);
		//Draw a cube that extends to where the hit exists
		Gizmos.DrawWireCube(transform.position + Vector3.down * _hit.distance, transform.localScale);
	}
	private void FixedUpdate()
	{
		_isGrounded = Physics.BoxCast(transform.position, (transform.localScale * 0.5f), Vector3.down, out _hit, Quaternion.identity, 0.1f);
		_mesh.transform.position = this.transform.position + Vector3.down * _hit.distance; 

		//Debug.Log(_hit.distance);


		UpdateState();
		//Debug.Log(_isGrounded);
		_isGrounded = false;
	}

	public void BeginPushing(Rigidbody golemRb)
	{
		if (_fj == null)
			_fj = gameObject.AddComponent<FixedJoint>();
		_fj.connectedBody = golemRb;
		_currentState = STATE.PUSH; 
	}

	public void StopPushing()
	{
		if (_fj != null)
			Destroy(_fj);
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
						StopPushing(); 
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

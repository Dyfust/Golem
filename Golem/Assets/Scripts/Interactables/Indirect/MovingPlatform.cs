using UnityEngine;
using UnityEngine.InputSystem;

public class MovingPlatform : MonoBehaviour, IInteractable
{
	enum DoorType { VERTICAL, HORIZONTAL }

	[SerializeField] private Vector3 _openedOffset;
	[SerializeField] private Vector3 _closedOffset;
	[SerializeField] private float _time;
	[SerializeField] private bool _dontControllerRumble;

	[CustomHeader("VFX")]
	[SerializeField] private CompositeParticleEffect _particleEffect;

	[CustomHeader("Audio")]
	[SerializeField] private AudioEmitter _audioEmitter;

	private Vector3 _startPos;
	private Vector3 _closedPos;
	private Vector3 _openedPos;
	private bool _isMoving = false;
	private bool _open = false;

	private float _dist;
	private float _speed;

	private bool _startState;

	private Vector3 pos;

	void Start()
	{
		_startPos = transform.position;

		_openedPos = _startPos + _openedOffset;
		_closedPos = _startPos + _closedOffset;

		_dist = Vector3.Distance(_openedPos, _closedPos);
		_speed = _dist / _time;
	}

	private void Update()
	{
		Vector3 targetPos = _open ? _openedPos : _closedPos;
		transform.position = Vector3.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);
		pos = transform.position;

		if (Vector3.Distance(transform.position, targetPos) <= 0.01f && _isMoving)
		{
			OnStopMoving();
			_isMoving = false;
		}

		if (_isMoving && !_dontControllerRumble)
		{
			if (Gamepad.current != null)
				Gamepad.current.SetMotorSpeeds(0.75f, 0.75f);
		}
		else
		{
			if (Gamepad.current != null)
				Gamepad.current.SetMotorSpeeds(0.0f, 0.0f); 
		}
	}

	public void Interact()
	{
		_open = !_open;
		_isMoving = true;

		OnStartMoving();
	}

	private void OnStartMoving()
	{
		_particleEffect?.PlayEffect();
		_audioEmitter?.Play();
		DebugWrapper.Log(gameObject.name + " has started moving!");
	}

	private void OnStopMoving()
	{
		_particleEffect?.StopEffect();
		_audioEmitter?.Stop();
	}
	
	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.green;
		Gizmos.DrawCube(new Vector3(_openedOffset.x / transform.localScale.x, _openedOffset.y / transform.localScale.y, _openedOffset.z / transform.localScale.z), Vector3.one);

		Gizmos.color = Color.red;
		Gizmos.DrawCube(new Vector3(_closedOffset.x / transform.localScale.x, _closedOffset.y / transform.localScale.y, _closedOffset.z / transform.localScale.z), Vector3.one);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals("Orb") || other.gameObject.tag.Equals("Golem") || other.gameObject.tag.Equals("Block") || other.gameObject.layer.Equals("Ground"))
		{
			other.gameObject.transform.SetParent(transform);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag.Equals("Orb") || other.gameObject.tag.Equals("Golem") || other.gameObject.tag.Equals("Block") || other.gameObject.layer.Equals("Ground"))
		{
			other.gameObject.transform.SetParent(null);
		}
	}
}

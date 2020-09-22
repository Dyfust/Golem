using System;
using UnityEngine;

// This is a hold interactable, held state and not held state. Interact to alternate between.
public class Door : MonoBehaviour, IInteractable, IReset, IPlayAudio
{
	enum DoorType { VERTICAL, HORIZONTAL }

	[SerializeField] private Vector3 _openedOffset;
	[SerializeField] private Vector3 _closedOffset;
	[SerializeField] private float _time;
	[SerializeField] private AudioClip _openingDoor;
	[SerializeField] private AudioClip _stoneDragging;

	private Vector3 _closedPos;
	private Vector3 _openedPos;
	private Vector3 _startPos;
	private Vector3 _currentPos;
	private bool _open = false;

	private float _startTime;
	private float _dist;
	private float _speed;

	private bool _startState;
	private bool _isMoving;

	public event EventHandler<AudioClip> PlayLoopedAudio;
	public event EventHandler StopLoopedAudio;
	public event EventHandler<AudioClip> PlayAudioEffect;

	private void Start()
	{
		// Initialising positions of the pressure plate.
		_startPos = transform.position;
		_currentPos = _startPos;

		_openedPos = _startPos + _openedOffset;
		_closedPos = _startPos + _closedOffset;
		_startTime = Time.time;

		_dist = Vector3.Distance(_openedPos, _closedPos);
		_speed = _dist / _time;
	}

	private void Update()
	{
		float elapsedTime = (Time.time - _startTime);
		float fractionOfJourney = elapsedTime / _time;

		if (fractionOfJourney >= 1 && _isMoving == true)
		{
			StopLoopedAudio?.Invoke(this, EventArgs.Empty); 
			_isMoving = false; 
		}

		if (_open)
			transform.position = Vector3.Lerp(_currentPos, _openedPos, fractionOfJourney);
		else
			transform.position = Vector3.Lerp(_currentPos, _closedPos, fractionOfJourney);
	}

    public void Interact()
    {
		_open = !_open;
		Vector3 targetPos;
		if (_open)
			targetPos = _openedPos;
		else
			targetPos = _closedPos;

		_currentPos = transform.position;
		_startTime = Time.time;

		_dist = Vector3.Distance(_currentPos, targetPos);
		_time = _dist / _speed;

		_isMoving = true; 

		if (_open == true)
			PlayAudioEffect?.Invoke(this, _openingDoor);

		PlayLoopedAudio?.Invoke(this, _stoneDragging); 
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.green;
		Gizmos.DrawCube(new Vector3(_openedOffset.x / transform.localScale.x, _openedOffset.y / transform.localScale.y, _openedOffset.z / transform.localScale.z), Vector3.one);

		Gizmos.color = Color.red;
		Gizmos.DrawCube(new Vector3(_closedOffset.x / transform.localScale.x, _closedOffset.y / transform.localScale.y, _closedOffset.z / transform.localScale.z), Vector3.one);
	}

	void IReset.OnEnter(Vector3 checkpointPos)
	{
		_startState = _open;
		_startPos = transform.position; 
	}

	void IReset.Reset()
	{
		_open = _startState;
		_currentPos = _startPos; 
	}	
}
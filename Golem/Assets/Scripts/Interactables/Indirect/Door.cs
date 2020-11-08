﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

// This is a hold interactable, held state and not held state. Interact to alternate between.
public class Door : MonoBehaviour, IInteractable
{
	enum DoorType { VERTICAL, HORIZONTAL }

	[SerializeField] private Vector3 _openedOffset;
	[SerializeField] private Vector3 _closedOffset;
	[SerializeField] private float _time;
	[SerializeField] private bool _dontControllerRumble; 

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
			_isMoving = false; 
		}

		if (_isMoving == true && _dontControllerRumble == false)
		{
			if (Gamepad.current != null)
				Gamepad.current.SetMotorSpeeds(0.75f, 0.75f);
		}
		else
		{
			if (Gamepad.current != null)
				Gamepad.current.SetMotorSpeeds(0.0f, 0.0f); 
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
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.green;
		Gizmos.DrawCube(new Vector3(_openedOffset.x / transform.localScale.x, _openedOffset.y / transform.localScale.y, _openedOffset.z / transform.localScale.z), Vector3.one);

		Gizmos.color = Color.red;
		Gizmos.DrawCube(new Vector3(_closedOffset.x / transform.localScale.x, _closedOffset.y / transform.localScale.y, _closedOffset.z / transform.localScale.z), Vector3.one);
	}
}
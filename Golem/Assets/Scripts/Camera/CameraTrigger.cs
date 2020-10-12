using UnityEngine;
using System;
using System.Collections;

public class CameraTrigger : MonoBehaviour
{
	enum Target { ORB, GOLEM, BLOCK }

	[SerializeField] private Target _target;
	[SerializeField] private bool _oneTimeActivate = true;
	private bool _activated = false; 
	[SerializeField] private VirtualCamera _virtualCamera;
	private string _targetTag;

	[SerializeField] private float _timerDuration; 
	private float _timer;
	private bool _startTimer; 

	private void Awake()
	{
		switch (_target)
		{
			case Target.ORB:
				_targetTag = "Orb";
				break;

			case Target.GOLEM:
				_targetTag = "Golem";
				break;

			case Target.BLOCK:
				_targetTag = "Block";
				break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(_targetTag))
		{
			if (_oneTimeActivate == false || _activated == false)
			{
				VirtualCameraManager.instance.ToggleExternalCamera(_virtualCamera);
				GameManager.instance.SystemPause();
				_timer = 0;
				_startTimer = true;
				_activated = true; 
			}
		}
	}

	private void Update()
	{
		if (_startTimer == true)
		{
			_timer += Time.deltaTime;

			if (_timer > _timerDuration)
			{
				VirtualCameraManager.instance.TogglePlayerCamera(); 
				GameManager.instance.SystemResume();
				_startTimer = false;
			}
		}
	}
}

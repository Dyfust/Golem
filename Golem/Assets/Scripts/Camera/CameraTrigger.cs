using UnityEngine;
using System;
using System.Collections;
/// <summary>
/// Used to switch to a desired camera when the triggered is activated. 
/// </summary>
public class CameraTrigger : MonoBehaviour, IInteractable
{
	[SerializeField] private bool _backToPlayer = true; 
	[SerializeField] private bool _oneTimeActivate = true;
	[SerializeField] private float _timerDuration;
	[SerializeField] private VirtualCamera _virtualCamera;


	private bool _activated = false; 
	private string _targetTag;
	private float _timer;
	private bool _startTimer; 

	private void Awake()
	{

	}


	private void Update()
	{
		if (_startTimer == true)
		{
			_timer += Time.deltaTime;

			if (_timer > _timerDuration && _backToPlayer == true)
			{
				VirtualCameraManager.instance.TogglePlayerCamera(); 
				GameManager.instance.SystemResume();
				_startTimer = false;
			}
		}
	}

	public void Interact()
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

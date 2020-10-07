using UnityEngine;
using System;
using System.Collections;

public class CameraTrigger : MonoBehaviour
{
	enum Target { ORB, GOLEM }

	[SerializeField] private Target _target;
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
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(_targetTag))
		{
			StopAllCoroutines();
			StartCoroutine("PauseCam");
			GameManager.instance.SystemPause();
			_timer = 0;
			_startTimer = true; 
		}
	}

	private void Update()
	{
		if (_startTimer == true)
		{
			_timer += Time.deltaTime;

			if (_timer > _timerDuration)
			{
				StopAllCoroutines();
				StartCoroutine("ResumeCam");
				GameManager.instance.SystemResume();
				_startTimer = false;
			}
		}
	}

	IEnumerator PauseCam()
	{
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.OUT, false));
		VirtualCameraManager.instance.ToggleExternalCamera(_virtualCamera);
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.IN, false));
	}

	IEnumerator ResumeCam()
	{
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.OUT, false));
		VirtualCameraManager.instance.TogglePlayerCamera();
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.IN, false));
	}
}

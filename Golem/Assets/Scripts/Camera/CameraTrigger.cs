using UnityEngine;
using System;
using System.Collections;

public class CameraTrigger : MonoBehaviour
{
	enum Target { ORB, GOLEM }

	[SerializeField] private Target _target;
	[SerializeField] private VirtualCamera _virtualCamera;
	private string _targetTag;

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
			StopCoroutine("CameraFade"); 
			StartCoroutine("CameraFade");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag(_targetTag))
			VirtualCameraManager.instance.TogglePlayerCamera();
	}

	IEnumerator CameraFade()
	{
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.OUT, false));
		VirtualCameraManager.instance.ToggleCamera(_virtualCamera);
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.IN, false));

	}


}

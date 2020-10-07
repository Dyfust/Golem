using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCameraTrigger : MonoBehaviour
{
	[SerializeField] private VirtualCamera _pauseCamera; 

	void Pause()
	{
		StopAllCoroutines();
		StartCoroutine("PauseCam");
	}

	void Resume()
	{
		StopAllCoroutines();
		StartCoroutine("ResumeCam");
	}

	IEnumerator PauseCam()
	{
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.OUT, false));
		VirtualCameraManager.instance.ToggleExternalCamera(_pauseCamera);
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.IN, false));
	}

	IEnumerator ResumeCam()
	{
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.OUT, false));
		VirtualCameraManager.instance.TogglePlayerCamera(); 
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.IN, false));
	}
}

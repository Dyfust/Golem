using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTest : MonoBehaviour
{
	[SerializeField] private GameObject _CMVirtualCamera;

	[SerializeField] private GameObject _currentVCamera;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		VirtualCameraManager.instance.ToggleVCam(_CMVirtualCamera);
	}

	private void OnTriggerExit(Collider other)
	{
		VirtualCameraManager.instance.ToggleVCam(_currentVCamera); 
	}
}

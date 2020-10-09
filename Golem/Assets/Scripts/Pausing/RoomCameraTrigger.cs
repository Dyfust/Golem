using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraTrigger : MonoBehaviour
{
	[SerializeField] private VirtualCamera _pauseCamera;

	private void OnTriggerEnter(Collider other)
	{
		PauseManager.instance.SetCurrentCamera(_pauseCamera); 
	}
}

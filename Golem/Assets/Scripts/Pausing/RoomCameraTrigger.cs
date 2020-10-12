using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraTrigger : MonoBehaviour
{
	[SerializeField] private VirtualCamera _pauseCamera;
	private BoxCollider _coll;

	private void Awake()
	{
		_coll = this.GetComponent<BoxCollider>(); 
	}

	private void OnDrawGizmos()
	{
		if (_coll != null)
		{
			//Gizmos.color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, 1f);
			//Gizmos.DrawCube(_coll.center, _coll.bounds.extents);
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		PauseManager.instance.SetCurrentCamera(_pauseCamera); 
	}
}

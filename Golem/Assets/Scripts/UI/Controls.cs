using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Controls : MonoBehaviour
{
	[SerializeField] private Transform _lookAt;
	[SerializeField] private Vector3 _offset;

	[SerializeField] private Camera _currentCam; 

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 pos = _currentCam.WorldToScreenPoint(_lookAt.position + _offset);


		transform.position = pos; 
	}
}

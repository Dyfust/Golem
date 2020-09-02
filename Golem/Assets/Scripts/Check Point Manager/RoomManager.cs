using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	[SerializeField] private Collider _roomColldier;
	[SerializeField] private LayerMask _resetableObjectsLayer; 
	
	private Collider[] _roomObjects;
	private IReset[] _resetableObjectsReset;

	// Start is called before the first frame update
	void Start()
	{
		_roomObjects = Physics.OverlapBox(_roomColldier.bounds.center, _roomColldier.bounds.extents, Quaternion.identity, _resetableObjectsLayer);
		
		_resetableObjectsReset = new IReset[_roomObjects.Length];

		for (int i = 0; i < _roomObjects.Length; i++)
		{
			_resetableObjectsReset[i] = _roomObjects[i].GetComponent<IReset>();
			_resetableObjectsReset[i].OnEnter();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			for (int i = 0; i < _resetableObjectsReset.Length; i++)
			{
				_resetableObjectsReset[i].Reset(); 
			}
		}
	}
}

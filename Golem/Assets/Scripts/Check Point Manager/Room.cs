using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	[SerializeField] private Collider _roomColldier;
	[SerializeField] private LayerMask _resetableObjectsLayer;	
	[SerializeField] private Transform _playerSpawn;

	private RoomManager _roomManagerRef;

	private Collider[] _roomObjects;
	private IReset[] _resetableObjectsReset;

	// Start is called before the first frame update
	void Start()
	{
		_roomManagerRef = RoomManager.instance; 

		_roomObjects = Physics.OverlapBox(_roomColldier.bounds.center, _roomColldier.bounds.extents, Quaternion.identity, _resetableObjectsLayer);

		_resetableObjectsReset = new IReset[_roomObjects.Length];

		for (int i = 0; i < _roomObjects.Length; i++)
		{
			_resetableObjectsReset[i] = _roomObjects[i].GetComponent<IReset>();
			_resetableObjectsReset[i].OnEnter();
		}
	}


	public void Reset()
	{
		for (int i = 0; i < _resetableObjectsReset.Length; i++)
		{
			_resetableObjectsReset[i].Reset();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals("Orb") || other.gameObject.tag.Equals("Golem"))
			_roomManagerRef.SetCurrentRoom(this);
	}
}

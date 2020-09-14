using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	[SerializeField] private Collider _roomCollider;
	[SerializeField] private LayerMask _resetableObjectsLayer;
	[SerializeField] private Transform _playerSpawn;

	private RoomManager _roomManagerRef;

	private Collider[] _roomObjects;
	private IReset[] _resetableObjectsReset;

	// Start is called before the first frame update
	void Start()
	{
		_roomManagerRef = RoomManager.instance; 

		_roomObjects = Physics.OverlapBox(transform.position, _roomCollider.bounds.extents, Quaternion.identity, _resetableObjectsLayer);

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

    private void OnDrawGizmos()
    {
		Color color = Color.cyan;
		color.a = 0.15f;

		Gizmos.color = color;

		Gizmos.DrawCube(transform.position, _roomCollider.bounds.extents * 2f);
    }
}

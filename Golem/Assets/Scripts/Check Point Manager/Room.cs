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
	[SerializeField] private Orb _orb;
	private IReset _orbReset;


	// Start is called before the first frame update
	void Start()
	{
		_roomManagerRef = RoomManager.instance;

		_orbReset = _orb.GetComponent<IReset>();

		_roomObjects = Physics.OverlapBox(transform.position, _roomCollider.bounds.extents, Quaternion.identity, _resetableObjectsLayer);

		_resetableObjectsReset = new IReset[_roomObjects.Length];

		for (int i = 0; i < _roomObjects.Length; i++)
		{
			_resetableObjectsReset[i] = _roomObjects[i].GetComponent<IReset>();
			_resetableObjectsReset[i].OnEnter(_playerSpawn.position);
		}
	}

	public void Reset()
	{
		for (int i = 0; i < _resetableObjectsReset.Length; i++)
		{
			_resetableObjectsReset[i].Reset();
		}

		_orbReset.Reset();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals("Orb") || other.gameObject.tag.Equals("Golem"))
		{
			_roomManagerRef.SetCurrentRoom(this);

			if (other.gameObject.CompareTag("Orb"))
            {
				_orbReset.OnEnter(_playerSpawn.position);
            }
		}
	}

    private void OnDrawGizmos()
    {
		Color color = Color.green;
		color.a = 0.15f;

		Gizmos.color = color;

		Gizmos.DrawCube(transform.position, _roomCollider.bounds.extents * 2f);
    }
}

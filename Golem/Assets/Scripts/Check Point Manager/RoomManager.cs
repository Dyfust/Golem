using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomManager : MonoBehaviour
{
	[SerializeField] private Room _startRoom; 

	private static RoomManager _instance;
	public static RoomManager instance => _instance;

	private const string _identifierTag = "Room"; 

	private Room _currentRoom;
	private Room[] _rooms;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else
			Debug.LogWarning("Multiple Room Managers Present!");

		GameObject[] roomGOs = GameObject.FindGameObjectsWithTag(_identifierTag);
		_rooms = new Room[roomGOs.Length];

		for (int i = 0; i < _rooms.Length; i++)
		{
			_rooms[i] = roomGOs[i].GetComponent<Room>(); 
		}

		_currentRoom = _startRoom; 
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			_currentRoom.Reset(); 
		}
	}

	public void SetCurrentRoom(Room currentRoom)=> _currentRoom = currentRoom;

	public Room GetCurrentRoom() { return _currentRoom; }

}

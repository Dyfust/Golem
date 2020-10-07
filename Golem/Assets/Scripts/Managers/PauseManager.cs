using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
	private static PauseManager _instance;

	public static PauseManager instance => _instance;

	private List<IPlayerPause> _pausableObjects;

	private const string _identifiertag = "Room"; 

	private RoomCameraTrigger[] _roomCameraTriggers; 
	private VirtualCamera _currentRoomCamera;
	[SerializeField] private VirtualCamera _startingCam;

	[SerializeField] private GameObject _pauseScreen;

	private bool _isPaused = false; 

	private void Awake()
	{
		if (_instance == null)
			_instance = this;

		GameObject[] camTriggerGOs = GameObject.FindGameObjectsWithTag(_identifiertag);
		_roomCameraTriggers = new RoomCameraTrigger[camTriggerGOs.Length];

		for (int i = 0; i < _roomCameraTriggers.Length; i++)
		{
			_roomCameraTriggers[i] = camTriggerGOs[i].GetComponent<RoomCameraTrigger>(); 
		}

		_currentRoomCamera = _startingCam;

		_pauseScreen.SetActive(false); 
	}

	// Start is called before the first frame update
	void Start()
	{
		var temp = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerPause>();
		_pausableObjects = new List<IPlayerPause>();

		foreach (IPlayerPause pausableObject in temp)
		{
			_pausableObjects.Add(pausableObject);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
			PlayerPause();
		if (Input.GetKeyDown(KeyCode.L))
			PlayerResume();
	}

	/// <summary>
	/// Paused triggered by the player 
	/// </summary>
	public void PlayerPause()
	{
		if (_isPaused == true)
			return; 

		for (int i = 0; i < _pausableObjects.Count(); i++)
		{
			_pausableObjects[i].Pause();
		}

		VirtualCameraManager.instance.ToggleExternalCamera(_currentRoomCamera);

		_pauseScreen.SetActive(true);

		_isPaused = true; 
	}

	public void PlayerResume()
	{
		if (_isPaused == false)
			return; 

		for (int i = 0; i < _pausableObjects.Count(); i++)
		{
			_pausableObjects[i].Resume();
		}

		VirtualCameraManager.instance.TogglePlayerCamera();

		_pauseScreen.SetActive(false);

		_isPaused = false; 
	}

	public void SetCurrentCamera(VirtualCamera cam)
	{
		_currentRoomCamera = cam; 
	}
}

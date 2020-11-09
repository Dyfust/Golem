using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour, IPauseableObject
{
	private static PauseManager _instance;

	public static PauseManager instance => _instance;

	private List<IPlayerPause> _pausableObjects;

	private const string _identifiertag = "Room"; 

	private RoomCameraTrigger[] _roomCameraTriggers; 
	private VirtualCamera _currentRoomCamera;
	[SerializeField] private VirtualCamera _startingCam;

	[SerializeField] private PanelManager _panaelManagerRef;
	[SerializeField] private Panel _pauseScreen;
	[SerializeField] private float _pauseTimer; 

	private bool _isPaused = false;
	private bool _interactable = true; 

	private InputMaster _input;

	private float _timeStamp; 

	private void OnEnable()
	{
		_input = new InputMaster();
		_input.Enable();
		_input.MenuNavigation.Pause.performed += Pause_performed;
	}

	private void OnDisable()
	{
		_input.MenuNavigation.Pause.performed -= Pause_performed;
		_input.Disable(); 
	}

	private void Pause_performed(InputAction.CallbackContext obj)
	{
		if (Time.time >= _timeStamp + _pauseTimer && _interactable == true)
		{
			if (_isPaused)
				PlayerResume();
			else
				PlayerPause();
		}

	}

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

		//_panaelManagerRef.CloseAllPanels();
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
		if (Input.GetKeyDown(KeyCode.P) && Time.time >= _timeStamp + _pauseTimer  && _interactable == true)
		{
			if (_isPaused)
				PlayerResume();
			else
				PlayerPause();
		}
	}

	/// <summary>
	/// Paused triggered by the player 
	/// </summary>
	public void PlayerPause()
	{
		_timeStamp = Time.time; 
		if (_isPaused == true)
			return; 

		for (int i = 0; i < _pausableObjects.Count(); i++)
		{
			_pausableObjects[i].Pause();
		}
		
		VirtualCameraManager.instance.ToggleExternalCamera(_currentRoomCamera);

		StopAllCoroutines();
		StartCoroutine("PauseUI");

		CursorManager.instance.ToggleCursor(true); 

		_isPaused = true; 
	}

	public void PlayerResume()
	{
		_timeStamp = Time.time; 
		if (_isPaused == false)
			return; 

		for (int i = 0; i < _pausableObjects.Count(); i++)
		{
			_pausableObjects[i].Resume();
		}


		_panaelManagerRef.CloseAllPanels(); 

		CursorManager.instance.ToggleCursor(false);

		_isPaused = false; 
		VirtualCameraManager.instance.TogglePlayerCamera();
	}

	public void SetCurrentCamera(VirtualCamera cam)
	{
		_currentRoomCamera = cam; 
	}

	private IEnumerator PauseUI()
	{
		yield return new WaitForSeconds(1);
		_panaelManagerRef.ActivatePanel(_pauseScreen); 
	}

	public void Pause()
	{
		_interactable = false; 
	}

	public void Resume()
	{
		_interactable = true; 
	}
}
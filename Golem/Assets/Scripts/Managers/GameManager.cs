using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	public static GameManager instance => _instance;

	[SerializeField] private GameObject _player;
	[SerializeField] private GameObject _spawn;

	private List<IPauseableObject> _pausableObjects;
	
	private void Awake()
	{
		if (_instance == null)
			_instance = this; 
	}

	// Start is called before the first frame update
	void Start()
	{
		var temp = FindObjectsOfType<MonoBehaviour>().OfType<IPauseableObject>();
		_pausableObjects = new List<IPauseableObject>();

		foreach (IPauseableObject pausableObject in temp)
		{
			_pausableObjects.Add(pausableObject); 
		}
	}

	// Update is called once per frame
	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.T))
		//	_player.transform.position = _spawn.transform.position; 

		//if (Input.GetKeyDown(KeyCode.P))
		//	PauseGame();
		//if (Input.GetKeyDown(KeyCode.L))
		//	ResumeGame(); 
	}

	/// <summary>
	/// Paused triggered by a system in the game 
	/// </summary>
	public void SystemPause()
	{
		for (int i = 0; i < _pausableObjects.Count(); i++)
		{
			_pausableObjects[i].Pause();
		}
	}

	public void SystemResume()
	{
		for (int i = 0; i < _pausableObjects.Count(); i++)
		{
			_pausableObjects[i].Resume(); 
		}
	}

	public void QuitGame()
	{
		Application.Quit(); 
	}
}

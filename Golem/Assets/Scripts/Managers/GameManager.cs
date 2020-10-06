using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	public static GameManager instance => _instance; 

	private List<IPause> _pausableObjects;

	private void Awake()
	{
		if (_instance == null)
			_instance = this; 
	}

	// Start is called before the first frame update
	void Start()
	{
		var temp = FindObjectsOfType<MonoBehaviour>().OfType<IPause>();
		_pausableObjects = new List<IPause>(); 
		
		foreach (IPause pausableObject in temp)
		{
			_pausableObjects.Add(pausableObject); 
		}

		Debug.Log(_pausableObjects.Count()); 
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();

		if (Input.GetKeyDown(KeyCode.P))
			PauseGame();
		if (Input.GetKeyDown(KeyCode.L))
			ResumeGame(); 
	}



	public void PauseGame()
	{
		for (int i = 0; i < _pausableObjects.Count(); i++)
		{
			_pausableObjects[i].Pause();
		}
	}

	public void ResumeGame()
	{
		for (int i = 0; i < _pausableObjects.Count(); i++)
		{
			_pausableObjects[i].Resume(); 
		}
	}
}

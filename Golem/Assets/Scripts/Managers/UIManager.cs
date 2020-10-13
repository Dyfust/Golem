using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class UIManager : MonoBehaviour
{	
	public enum UI
	{
		GOLEMUI,
		BLOCKUI
	}

	private static UIManager _instance;
	public static UIManager instance => _instance;

	private UI _choice; 

	[SerializeField] private GameObject _golemInteract;
	[SerializeField] private GameObject _blockInteract;

	private void Awake()
	{
		if (_instance == null)
			_instance = this; 
	}
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
	}

	public void ShowUI(UI choice)
	{
		_choice = choice;

		switch (_choice)
		{
			case UI.GOLEMUI:
				_golemInteract.SetActive(true);
				_blockInteract.SetActive(false);
				break;
			case UI.BLOCKUI:
				_blockInteract.SetActive(true);
				_golemInteract.SetActive(false);
				break; 
		}
	}

	public void HideUI(UI choice)
	{
		_choice = choice;

		switch (_choice)
		{
			case UI.GOLEMUI:
				_golemInteract.SetActive(false);
				_blockInteract.SetActive(false);
				break;
			case UI.BLOCKUI:
				_blockInteract.SetActive(false);
				_golemInteract.SetActive(false);
				break; 
		}
	}

}

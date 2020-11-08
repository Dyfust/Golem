using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GamePadMenuSupport : MonoBehaviour
{
	[SerializeField] private List<GameObject> _mainMenuObjects;
	[SerializeField] private List<GameObject> _settingsObjects;
	[SerializeField] private GameObject _title;
	[SerializeField] private GameObject _mainMenu;
	[SerializeField] private GameObject _settings;
	[SerializeField] private GameObject _credits;
	[SerializeField] private int _sliderIncrement;


	private int _mainMenuIndex;
	private int _settingsIndex; 
	private InputMaster _navigation; 
	private GameObject _currentButtonRef;
	private TextMeshProUGUI _currentText;
	private UnityEngine.UI.Button _currentButton; 

	#region Input
	private void OnEnable()
	{
		_navigation = new InputMaster();
		_navigation.MenuNavigation.Enable();  
		_navigation.MenuNavigation.ConfirmButtonPress.performed += ConfirmButtonPress_performed;
		_navigation.MenuNavigation.ReturnButtonPress.performed += ReturnButtonPress_performed;
		_navigation.MenuNavigation.IndexUp.performed += IndexUp_performed;
		_navigation.MenuNavigation.IndexDown.performed += IndexDown_performed;
		_navigation.MenuNavigation.SliderIncrease.performed += SliderIncrease_performed;
		_navigation.MenuNavigation.SliderDecrease.performed += SliderDecrease_performed;
	}

	private void OnDisable()
	{
		_navigation.MenuNavigation.ConfirmButtonPress.performed -= ConfirmButtonPress_performed;
		_navigation.MenuNavigation.ReturnButtonPress.performed -= ReturnButtonPress_performed;
		_navigation.MenuNavigation.IndexUp.performed -= IndexUp_performed;
		_navigation.MenuNavigation.IndexDown.performed -= IndexDown_performed; 
		_navigation.MenuNavigation.SliderIncrease.performed += SliderIncrease_performed;
		_navigation.MenuNavigation.SliderDecrease.performed += SliderDecrease_performed;
		_navigation.MenuNavigation.Disable();
	}
	private void SliderDecrease_performed(InputAction.CallbackContext obj)
	{
		MoveSlider(_sliderIncrement);
	}

	private void SliderIncrease_performed(InputAction.CallbackContext obj)
	{
		MoveSlider(_sliderIncrement);
	}
	private void ReturnButtonPress_performed(InputAction.CallbackContext obj)
	{
		_settings.SetActive(false);
		_credits.SetActive(false);
		_mainMenu.gameObject.SetActive(true);
		ChangeCurrentButton(); 
	}

	private void IndexDown_performed(InputAction.CallbackContext obj)
	{
		if (_mainMenu.activeInHierarchy == true)
		{
			if (_mainMenuIndex != 0)
				--_mainMenuIndex;
			ChangeCurrentButton();
		}
		else if (_settings.activeInHierarchy == true)
		{
			if (_settingsIndex != 0)
			{
				--_settingsIndex;
				ChangeCurrentSlider();
			}
		}
	}

	private void IndexUp_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		if (_mainMenu.activeInHierarchy == true)
		{
			if (_mainMenuIndex != _mainMenuObjects.Count - 1)
			{
				++_mainMenuIndex;
				ChangeCurrentButton();
			}
		}
		else if (_settings.activeInHierarchy == true)
		{
			if (_settingsIndex != _settingsObjects.Count - 1)
			{
				++_settingsIndex;
				ChangeCurrentSlider(); 
			}
		}
	}
	private void ConfirmButtonPress_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		_currentButtonRef.GetComponent<UnityEngine.UI.Button>().Select();
	}
	#endregion

	void Start()
	{
		_mainMenu.SetActive(true);
		_settings.SetActive(false);
		_credits.SetActive(false);
		_mainMenuIndex = _mainMenuObjects.Count - 1;
		_settingsIndex = _settingsObjects.Count - 1; 
		_currentButtonRef = _mainMenuObjects[_mainMenuIndex];
		EventSystem.current.SetSelectedGameObject(_currentButtonRef);
		Transform child = _currentButtonRef.transform.Find("Text (TMP)");
		_currentText = child.transform.GetComponent<TextMeshProUGUI>();
		EventSystem.current.SetSelectedGameObject(_currentButtonRef);

	}


	// Update is called once per frame
	void Update()
	{
		Color rainbow = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));
		_currentText.color = rainbow;
	}

	private void ChangeCurrentButton()
	{
		_currentText.color = Color.black; 
		_currentButtonRef = _mainMenuObjects[_mainMenuIndex];
		EventSystem.current.SetSelectedGameObject(_currentButtonRef);
		Transform child = _currentButtonRef.transform.Find("Text (TMP)");
		_currentText = child.gameObject.GetComponent<TextMeshProUGUI>(); 
	}

	private void ChangeCurrentSlider()
	{
		_currentText.color = Color.white;
		Transform child = _settingsObjects[_settingsIndex].transform.Find("Text (TMP)");
		_currentText = child.gameObject.GetComponent<TextMeshProUGUI>(); 
	}

	private void MoveSlider(int value)
	{
		UnityEngine.UI.Slider slider = _settingsObjects[_settingsIndex].GetComponent<UnityEngine.UI.Slider>();
	}


	public void Settings()
	{
		_mainMenu.gameObject.SetActive(false);
		_settings.gameObject.SetActive(true);
		//ChangeCurrentSlider(); 
	}

	public void Credits()
	{
		_mainMenu.gameObject.SetActive(false);
		_credits.gameObject.SetActive(true);
	}

	public void Quit()
	{
		Application.Quit(); 
	}

	public void Play()
	{
		_mainMenu.SetActive(false);
	}
}

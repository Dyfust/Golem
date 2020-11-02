using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GamePadMenuSupport : MonoBehaviour
{
	[SerializeField] private List<GameObject> _buttons;

	private InputMaster _navigation; 
	private GameObject _currentButton;
	private int _index; 
	// Start is called before the first frame update
	void Start()
	{
		_navigation = new InputMaster();
		_navigation.MenuNavigation.ButtonPress.performed += ButtonPress_performed;
		_currentButton = _buttons[0];
		EventSystem.current.SetSelectedGameObject(_currentButton); 
	}

	private void ButtonPress_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		_currentButton.GetComponent<UnityEngine.UI.Button>().Select(); 
	}

	// Update is called once per frame
	void Update()
	{
		
		Color rainbow = Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1));

		foreach (GameObject button in _buttons)
		{
			_currentButton = EventSystem.current.currentSelectedGameObject;
			Transform child = button.transform.Find("Text");
			Text text = child.gameObject.GetComponent<Text>();
			if (button == _currentButton)
			{
				text.color = rainbow;
			}
			else
			{
				text.color = Color.black; 
			}
			////UnityEngine.UI.Button currentButton = button.GetComponent<UnityEngine.UI.Button>(); 
			//if (EventSystem.current.currentSelectedGameObject == button)
			//{
			//	Transform child = button.transform.Find("Text");
			//	Text text = child.gameObject.GetComponent<Text>();
			//	text.color = rainbow; 
			//}

			//if (button != _cu)
		}

	}

	public void clicked()
	{
		Debug.Log("SHIT"); 
	}
}

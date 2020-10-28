using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class GolemControls : MonoBehaviour
{
	[SerializeField] private GameObject _glyphs;
	[SerializeField] private GameObject _golemControlTxt;
	[SerializeField] private Golem _ref;
	[SerializeField] private float _fadeInTimer;
	[SerializeField] private float _fadeOutTimer;


	private InputAction _action;
	private TMP_Text _textRef;
	private bool _firstView = true;

	private void OnEnable()
	{
		_action.performed += _action_performed;
	}

	private void _action_performed(InputAction.CallbackContext obj)
	{
		Debug.Log(obj.control.path);
	}

	private void Awake()
	{
		_action = new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>");
		_textRef = _golemControlTxt.GetComponent<TMP_Text>();

		//_textref.text = "Press " + _inputType.action.GetBindingDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice) + " to enter!"; 

	}

	// Update is called once per frame
	void Update()
	{
		if (_ref.IsActive() == true)
		{
			_golemControlTxt.SetActive(false);
			_glyphs.SetActive(false);
		}

		if (GlobalInput.instance.GetCurrentInputMethod() == GlobalInput.DEVICE.KEYBOARD)
			_textRef.text = "Press 'F' to enter!";
		if (GlobalInput.instance.GetCurrentInputMethod() == GlobalInput.DEVICE.GAMEPAD)
			_textRef.text = "Press 'A' to enter!";
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Orb"))
		{
			if (_firstView == true)
			{
				StopAllCoroutines();
				StartCoroutine(FadeOut(_fadeInTimer, _fadeOutTimer, _glyphs, _golemControlTxt));
				_firstView = false;
			}
			//else
			//	_golemControlTxt.SetActive(true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Orb"))
		{
			_glyphs.SetActive(false);
			_golemControlTxt.SetActive(false);
		}
	}

	/// <summary>
	/// This will fade the image in and the text 
	/// </summary>
	/// <param name="t1"></param>
	/// <param name="t2"></param>
	/// <param name="image"></param>
	/// <param name="text"></param>
	/// <returns></returns>
	private IEnumerator FadeOut(float t1, float t2, GameObject image, GameObject text)
	{
		_glyphs.SetActive(true);
		_golemControlTxt.SetActive(true);

		Image img = image.GetComponent<Image>();
		img.color = new Color(img.color.r, img.color.g, img.color.b, 0);

		_textRef.color = new Color(_textRef.color.r, _textRef.color.g, _textRef.color.b, 0);

		while (img.color.a <= 1.0f)
		{
			img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + (Time.deltaTime / t1));
			yield return null;
		}
		while (img.color.a > 0.0f)
		{
			img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - (Time.deltaTime / t2));
			yield return null;
		}

		while (_textRef.color.a <= 1.0f)
		{
			_textRef.color = new Color(_textRef.color.r, _textRef.color.g, _textRef.color.b, _textRef.color.a + (Time.deltaTime / t2));
			yield return null;
		}
	}

	public GameObject GetImage()
	{
		return _glyphs;
	}
}

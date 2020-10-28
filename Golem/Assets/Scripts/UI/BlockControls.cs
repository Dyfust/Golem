using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BlockControls : MonoBehaviour
{
	[SerializeField] private GameObject _glyphs;
	[SerializeField] private GameObject _blockControlTxt;
	[SerializeField] private GameObject _tutGolem;
	[SerializeField] private InputActionReference _inputType;
	[SerializeField] private float _fadeInTimer;
	[SerializeField] private float _fadeOutTimer;

	private TMP_Text _textRef; 
	private GolemControls _controlsRef;
	private Golem _golemRef;
	private Block _blockRef;
	private bool _firstView = true;

	private void Awake()
	{
		_controlsRef = _tutGolem.GetComponent<GolemControls>();
		_golemRef = _tutGolem.GetComponent<Golem>();
		_blockRef = this.GetComponent<Block>();
		_textRef = _blockControlTxt.GetComponent<TMP_Text>();

		//_textRef.text = "Press " + _inputType.action.GetBindingDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice) + " to push/pull"; 
	}

	private void Update()
	{
		if (GlobalInput.instance.GetCurrentInputMethod() == GlobalInput.DEVICE.KEYBOARD)
			_textRef.text = "Press 'E' to Interact!";
		if (GlobalInput.instance.GetCurrentInputMethod() == GlobalInput.DEVICE.GAMEPAD)
			_textRef.text = "Press 'X' to Interact!";
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Golem"))
		{
			if (_controlsRef.GetImage().activeInHierarchy == false && _golemRef.IsActive() == true)
			{
				if (_firstView == true)
				{
					StopAllCoroutines();
					StartCoroutine(FadeOut(_fadeInTimer, _fadeOutTimer, _glyphs, _blockControlTxt));
					_firstView = false;
				}
				else 
					_blockControlTxt.SetActive(true); 

			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Golem"))
		{
			if (_golemRef.IsActive() == true)
			{
				if (_blockRef.IsConnected() == true)
					_blockControlTxt.SetActive(false);
				else
					_blockControlTxt.SetActive(true);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Golem"))
		{
			if (_golemRef.IsActive() == true)
				_blockControlTxt.SetActive(false);
		}
	}

	private IEnumerator FadeOut(float t1, float t2, GameObject image, GameObject text)
	{
		_glyphs.SetActive(true);
		_blockControlTxt.SetActive(true);

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
}

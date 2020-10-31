using Boo.Lang;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]

public class Controls : MonoBehaviour, IUIDisplay
{
	enum TARGET
	{
		ORB,
		GOLEM
	}

	[SerializeField] private GameObject _glyph;
	[SerializeField] private GameObject _text;
	[SerializeField] private float _fadeInTimer;
	[SerializeField] private float _fadeOutTimer;
	[SerializeField] private string _keyboardPrompt;
	[SerializeField] private string _gamepadPrompt;
	[SerializeField] private TARGET _target;

	private string _targetTag;
	private SphereCollider _col;
	private TMP_Text _textRef;
	private bool _active = false;
	private bool _viewed = false;

	private List<IUIDisplay> _uiDisplays; 

	private void Start()
	{

		var temp = FindObjectsOfType<MonoBehaviour>().OfType<IUIDisplay>();
		_uiDisplays = new List<IUIDisplay>();

		foreach (IUIDisplay ui in temp)
		{
			if (ui == this)
				continue; 
			_uiDisplays.Add(ui); 
		}


		_textRef = _text.GetComponent<TMP_Text>();
		_col = GetComponent<SphereCollider>();
		_col.isTrigger = true; 

		switch (_target)
		{
			case TARGET.ORB:
				_targetTag = "Orb";
				break;
			case TARGET.GOLEM:
				_targetTag = "Golem";
				break;
		}
	}

	private void Update()
	{
		if (GlobalInput.instance.GetCurrentInputMethod() == GlobalInput.DEVICE.KEYBOARD)
			_textRef.text = _keyboardPrompt;
		if (GlobalInput.instance.GetCurrentInputMethod() == GlobalInput.DEVICE.GAMEPAD)
			_textRef.text = _gamepadPrompt;


	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(_targetTag) && other.gameObject.GetComponent<Player>().IsActive() == true)
		{
			if (_viewed == false)
			{
				for (int i = 0; i < _uiDisplays.Count(); i++)
				{
					if (_uiDisplays[i].UIActive() == true)
					{
						_uiDisplays[i].HideUI(); 
					}
				}
				_active = true;
				StopAllCoroutines();
				StartCoroutine(FadeOut(_fadeInTimer, _fadeOutTimer, _glyph, _text));
				_viewed = true; 
			}
		}
	}

	private IEnumerator FadeOut(float t1, float t2, GameObject image, GameObject text)
	{
		_glyph.SetActive(true);
		_text.SetActive(true);

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
		while (_textRef.color.a > 0)
		{
			_textRef.color = new Color(_textRef.color.r, _textRef.color.g, _textRef.color.b, _textRef.color.a - (Time.deltaTime / t2));
			yield return null;
		}
		_active = false;
	}

	public bool UIActive()
	{
		return _active; 
	}

	public void HideUI()
	{
		StopAllCoroutines();
		_glyph.SetActive(false);
		_text.SetActive(false); 
	}
}

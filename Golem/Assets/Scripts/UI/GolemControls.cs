using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GolemControls : MonoBehaviour
{
	public GameObject _tineText;
	public GameObject _golemControl;

	private Golem _ref;

	private bool _firstView = true; 
	// Start is called before the first frame update
	void Start()
	{
		_ref = this.GetComponent<Golem>(); 
	}

	// Update is called once per frame
	void Update()
	{
		if (_ref.IsActive() == true)
			_tineText.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Orb"))
		{
			if (_firstView == true)
			{
				StopAllCoroutines();
				StartCoroutine(FadeOut(1.5f, 1.5f, _tineText, _golemControl));
				_firstView = false; 
			}
			else
				_golemControl.SetActive(true); 
		}
	}

	private void OnTriggerStay(Collider other)
	{
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Orb"))
		{
			_tineText.SetActive(false);
			_golemControl.SetActive(false);
		}
	}

	private IEnumerator FadeOut(float t1, float t2, GameObject image, GameObject text)
	{
		_tineText.SetActive(true);
		_golemControl.SetActive(true);
		Image img = image.GetComponent<Image>();
		img.color = new Color(img.color.r, img.color.g, img.color.b, 1);

		TMP_Text txt = text.GetComponent<TMP_Text>();
		txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0); 

		while (img.color.a > 0.0f)
		{
			img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - (Time.deltaTime / t1));
			yield return null;
		}

		while (txt.color.a <= 1.0f)
		{
			txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime / t2));
			yield return null;
		}
	}
}

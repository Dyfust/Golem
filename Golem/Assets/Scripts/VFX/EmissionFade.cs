using System.Collections;
using UnityEngine;

public class EmissionFade : MonoBehaviour
{
	[SerializeField] private Renderer _renderer;
	//[SerializeField] private SkinnedMeshRenderer _renderer;
	[SerializeField] private float _duration;

	[SerializeField] private float _start = -2.0f;
	[SerializeField] private float _end = 4.0f;

	private Color _emissionColourRef; 
	private float _current = 0f;
	private Material _material;

	private void Awake()
	{
		_material = _renderer.material;
		_emissionColourRef = _material.color;
		_current = _start;

		_material.SetColor("_EmissionColor", _emissionColourRef * _start); 
	}

	public void OnActivate()
	{
		StopAllCoroutines();
		StartCoroutine(PlayCoroutine(_end));
	}

	public void OnDeactivate()
	{
		StopAllCoroutines();
		StartCoroutine(PlayCoroutine(_start));
	}

	IEnumerator PlayCoroutine(float end)
	{
		float distance = _end - _start; 
		float speed = distance / _duration;

		while (_current != end)
		{
			_current = Mathf.MoveTowards(_current, end, speed * Time.deltaTime);
			_material.SetColor("_EmissionColor", _emissionColourRef * _current); 
			yield return null;
		}
	}
}

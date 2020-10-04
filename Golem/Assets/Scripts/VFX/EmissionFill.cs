using System.Collections;
using UnityEngine;

public class EmissionFill : MonoBehaviour
{
	[SerializeField] private MeshRenderer _renderer;
	[SerializeField] private float _duration;

	private float _current = 0f;
	private Material _material;

    private void Awake()
    {
		_material = _renderer.material;
    }

	public void OnActivate()
    {
		StopAllCoroutines();
		StartCoroutine(PlayCoroutine(0f, 1f));
	}

	public void OnDeactivate()
    {
		StopAllCoroutines();
		StartCoroutine(PlayCoroutine(_current, 0f));
	}

    IEnumerator PlayCoroutine(float start, float end)
    {
		float elapsedTime = 0f;
		float t = 0f;

		while(t < 1f)
        {
			elapsedTime += Time.deltaTime;
			t = elapsedTime / _duration;

			_current = Mathf.Lerp(start, end, t);
			_material.SetFloat("Vector1_1A79052A", _current);
			yield return null;
		}
    }
}

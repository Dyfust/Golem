using System.Collections;
using UnityEngine;

public class EmissionFill : MonoBehaviour
{
	[SerializeField] private MeshRenderer _renderer;
	[SerializeField] private float _duration;
	[SerializeField] private string _target; 

	private float _current = 0f;
	private Material _material;

    private void Awake()
    {
		_material = _renderer.material;
    }

	public void OnActivate()
    {
		StopAllCoroutines();
		StartCoroutine(PlayCoroutine(1f));
	}

	public void OnDeactivate()
    {
		StopAllCoroutines();
		StartCoroutine(PlayCoroutine(0f));
	}

    IEnumerator PlayCoroutine(float end)
    {
		float speed = 1f / _duration;

		while(_current != end)
        {
			_current = Mathf.MoveTowards(_current, end, speed * Time.deltaTime);
			_material.SetFloat(_target, _current);
			yield return null;
		}
    }
}

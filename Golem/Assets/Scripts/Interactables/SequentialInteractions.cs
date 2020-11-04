using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
struct InteractionRef
{
	public GameObject _interactionGameObject;
	public float _switchDelay;
}

public class SequentialInteractions : MonoBehaviour, IInteractable
{
	//[SerializeField] private GameObject[] _interactions;
	//[SerializeField] private float[] _sequenceBuffer;

	[SerializeField] private InteractionRef[] _interactions; 
	private IInteractable[] _interactref;

	private float _totalDuration = -1;

	private void Start()
	{
		_interactref = new IInteractable[_interactions.Length]; 

		for (int i = 0; i < _interactions.Length; i++)
		{
			_interactref[i] = _interactions[i]._interactionGameObject.GetComponent<IInteractable>(); 
		}
	}
	private IEnumerator ExecuteOrder66()
	{
		for (int i = 0; i < _interactions.Length; i++)
		{
			_interactref[i].Interact();
			yield return new WaitForSeconds(_interactions[i]._switchDelay); 
		}
	}

	public float Duration()
	{
		if (_totalDuration == -1)
		{
			for (int i = 0; i < _interactions.Length; i++)
			{
				_totalDuration +=  _interactions[i]._switchDelay;
			}
		}
		return _totalDuration;
	}

	public void Interact()
	{
		StopAllCoroutines();
		StartCoroutine(ExecuteOrder66());
	}

}

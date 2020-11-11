using UnityEngine;

public class Button : MonoBehaviour
{
	[SerializeField] private GameObject[] _interactions;

	[CustomHeader("Audio")]
	[SerializeField] private OneShotEmitter _sfxEmitter;

	private bool _activated; 
	private string _targetTag = "Orb";

	private EmissionFill _emmisiveAnim;

	private void Awake()
	{
		_emmisiveAnim = GetComponent<EmissionFill>(); 
	}

	private void ToggleInteractions()
	{
		for (int i = 0; i < _interactions.Length; i++)
		{
			_interactions[i].GetComponent<IInteractable>().Interact();
		}

		_sfxEmitter?.Play();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(_targetTag))
		{
			if (_activated == false)
			{
				ToggleInteractions();
				_activated = true;

				if (_emmisiveAnim != null)
					_emmisiveAnim.OnActivate(); 
			}
		}
	}
}

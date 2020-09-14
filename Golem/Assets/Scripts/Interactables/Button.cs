using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
	enum ButtonType { ORB, GOLEM, BLOCK }

	[SerializeField] private ButtonType _type;
	[SerializeField] private GameObject[] _interactions;
	[SerializeField] private bool _oneTimeActivate;

	private bool _activated; 

	private string _targetTag;

	private EmmisiveAnimation _emmisiveAnim;
	private bool _emmisiveActivated; 

	private void Awake()
	{
		_emmisiveAnim = GetComponent<EmmisiveAnimation>(); 

		switch (_type)
		{
			case ButtonType.ORB:
				{
					_targetTag = "Orb";
					break;
				}
			case ButtonType.GOLEM:
				{
					_targetTag = "Golem";
					break;
				}
			case ButtonType.BLOCK:
				{
					_targetTag = "Block";
					break;
				}
		}
	}

	private void ToggleInteractions()
	{
		for (int i = 0; i < _interactions.Length; i++)
		{
			_interactions[i].GetComponent<IInteractable>().Interact();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(_targetTag))
		{
			if (_oneTimeActivate == true && _activated == false)
			{
				ToggleInteractions();
				Debug.Log("Enter");
				_activated = true;

				if (_emmisiveAnim != null)
					_emmisiveAnim.OnActivate(); 
			}
			else if (_oneTimeActivate == false)
			{
				ToggleInteractions();
				Debug.Log("Enter");

				if (_emmisiveActivated == true)
				{
					if (_emmisiveAnim != null)
						_emmisiveAnim.OnDeactivate();
					_emmisiveActivated = false;
				}
				else
				{
					if (_emmisiveAnim != null)
						_emmisiveAnim.OnActivate();
					_emmisiveActivated = true; 
				}
			}
		}
	}


}

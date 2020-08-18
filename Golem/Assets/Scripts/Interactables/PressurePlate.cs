using UnityEngine;

public class PressurePlate : MonoBehaviour
{
	enum PressurePlateType { ORB, GOLEM, BLOCK }

	[SerializeField] private PressurePlateType _type;
	[SerializeField] private GameObject[] _interactions;

	private string _targetTag;

	private void Awake()
	{
		//if (_type == PressurePlateType.ORB)
		//    _targetTag = "Orb";
		//else if (_type == PressurePlateType.GOLEM)
		//    _targetTag = "Golem";
		//else 

		switch (_type)
		{
			case PressurePlateType.ORB:
				{
					_targetTag = "Orb";
					break;
				}
			case PressurePlateType.GOLEM:
				{
					_targetTag = "Golem";
					break;
				}
			case PressurePlateType.BLOCK:
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
			ToggleInteractions();
			Debug.Log("Enter");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag(_targetTag))
		{
			ToggleInteractions();
			Debug.Log("Exit");
		}
	}
}
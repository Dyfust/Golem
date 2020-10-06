using UnityEngine;

public class PressurePlate : MonoBehaviour
{
	enum PressurePlateType { ORB, GOLEM, BLOCK }

	[SerializeField] private PressurePlateType _type;
	[SerializeField] private GameObject[] _interactions;

	[Tooltip("If set to false, what ever the pressure plate activates will deactivate when the player leaves the trigger. If true then the pressure plate will stay active forever")]
	[SerializeField] private bool _functionToggle = false;

	private string _targetTag;

	private EmissionFill _emmisiveAnim;

	private void Awake()
	{
		_emmisiveAnim = GetComponent<EmissionFill>();

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

			if (_emmisiveAnim != null)
				_emmisiveAnim.OnActivate();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (_functionToggle == false)
		{
			if (other.gameObject.CompareTag(_targetTag))
			{
				ToggleInteractions();

				if (_emmisiveAnim != null)
					_emmisiveAnim.OnDeactivate();
			}
		}
	}
}
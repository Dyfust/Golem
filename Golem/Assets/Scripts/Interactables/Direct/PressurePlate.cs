using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    enum PressurePlateType { ORB, GOLEM }

    [SerializeField] private PressurePlateType _type;
    [SerializeField] private GameObject[] _interactions;

    //[Tooltip("If set to false, what ever the pressure plate activates will deactivate when the player leaves the trigger. If true then the pressure plate will stay active forever")]
    //[SerializeField] private bool _functionToggle = false;

    private string[] _targetTags;

    private EmissionFill _emmisiveAnim;

    private void Awake()
    {
        _emmisiveAnim = GetComponent<EmissionFill>();

        switch (_type)
        {
            case PressurePlateType.ORB:
                {
                    _targetTags = new string[1];
                    _targetTags[0] = "Orb";
                    break;
                }
            case PressurePlateType.GOLEM:
                {
                    _targetTags = new string[2];
                    _targetTags[0] = "Golem";
                    _targetTags[1] = "Block";
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
        if (CompareTag(other.gameObject.tag, _targetTags))
        {
            ToggleInteractions();

            if (_emmisiveAnim != null)
                _emmisiveAnim.OnActivate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CompareTag(other.gameObject.tag, _targetTags))
        {
            ToggleInteractions();

            if (_emmisiveAnim != null)
                _emmisiveAnim.OnDeactivate();
        }
    }

    private bool CompareTag(string tag, string[] tags)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (tag == tags[i])
                return true;
        }

        return false;
    }
}
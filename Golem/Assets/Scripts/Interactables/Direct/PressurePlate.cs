using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject[] _interactions;

    //[Tooltip("If set to false, what ever the pressure plate activates will deactivate when the player leaves the trigger. If true then the pressure plate will stay active forever")]
    //[SerializeField] private bool _functionToggle = false;

    private string[] _targetTags;

    [CustomHeader("VFX")]
    [SerializeField] private EmissionFill _innerEmissionFill;
    [SerializeField] private EmissionFill _outerEmmisionFill;

    [CustomHeader("Audio")]
    [SerializeField] private OneShotEmitter _sfxEmitter;
    
    private void Awake()
    {
        _targetTags = new string[2];
        _targetTags[0] = "Golem";
        _targetTags[1] = "Block";
    }

    private void ToggleInteractions()
    {
        for (int i = 0; i < _interactions.Length; i++)
        {
            _interactions[i].GetComponent<IInteractable>().Interact();
        }

        _sfxEmitter.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag(other.gameObject.tag, _targetTags))
        {
            ToggleInteractions();

            _innerEmissionFill.OnActivate();
            _outerEmmisionFill.OnActivate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CompareTag(other.gameObject.tag, _targetTags))
        {
            ToggleInteractions();

            _innerEmissionFill.OnDeactivate();
            _outerEmmisionFill.OnDeactivate();
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
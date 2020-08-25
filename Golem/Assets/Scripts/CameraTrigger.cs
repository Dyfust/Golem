using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    enum Target { ORB, GOLEM }

    [SerializeField] private Target _target;
    [SerializeField] private VirtualCamera _virtualCamera;
    private string _targetTag;

    private void Awake()
    {
        switch (_target)
        {
            case Target.ORB:
                _targetTag = "Orb";
                break;

            case Target.GOLEM:
                _targetTag = "Golem";
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_targetTag))
        {
            VirtualCameraManager.instance.ToggleCamera(_virtualCamera);
            Debug.Log("OKAY");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_targetTag))
            VirtualCameraManager.instance.TogglePlayerCamera();
    }
}

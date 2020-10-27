using UnityEngine;

public class TunnelZone : MonoBehaviour
{
    private enum Axis {X, Y, Z};
    [SerializeField] private Axis _axis;

    private Collider _coll;
    private Vector3 _center;

    private void Awake()
    {
        _coll = GetComponent<Collider>();
        _center = _coll.bounds.center;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            int offset = 0;

            if (other.transform.position[(int)_axis] > _center[(int)_axis])
                offset = 2;
            else
                offset = -2;

            VirtualCameraManager.instance.ToggleTunnelCamera(offset, (int)_axis);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Orb"))
            VirtualCameraManager.instance.TogglePlayerCameraNoFade();
    }
}

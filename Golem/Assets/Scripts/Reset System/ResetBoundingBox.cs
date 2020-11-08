using UnityEngine;

public class ResetBoundingBox : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] private Collider _boundingBox;

    [SerializeField] private Orb _orb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Orb"))
        {
            ResetSystem.instance.SetCurrentBoundingBox(this);
        }
    }

    public void OnExit()
    {
        Disable();
    }

    public void Enable()
    {
        _root.SetActive(true);
    }

    private void Disable()
    {
        _root.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = 0.15f;

        Gizmos.color = color;

        Gizmos.DrawCube(transform.position, _boundingBox.bounds.extents * 2f);
    }
}

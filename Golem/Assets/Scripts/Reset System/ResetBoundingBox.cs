using UnityEngine;

public class ResetBoundingBox : MonoBehaviour
{
    [SerializeField] private Collider _boundingBox;
    [SerializeField] private LayerMask _resetLayerMask;
    [SerializeField] private Transform _checkpoint;

    [SerializeField] private Orb _orb;

    private IReset _orbReset;
    private IReset[] _detectedObjects;

    private void Start()
    {
        _orbReset = _orb.GetComponent<IReset>();

        Collider[] results = Physics.OverlapBox(transform.position, _boundingBox.bounds.extents, Quaternion.identity, _resetLayerMask);

        _detectedObjects = new IReset[results.Length];

        for (int i = 0; i < results.Length; i++)
        {
            _detectedObjects[i] = results[i].GetComponent<IReset>();
            _detectedObjects[i].OnEnter(_checkpoint.position);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < _detectedObjects.Length; i++)
        {
            _detectedObjects[i].Reset();
        }

        _orbReset.Reset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Orb"))
        {
            ResetSystem.instance.SetCurrentBoundingBox(this);
            _orbReset.OnEnter(_checkpoint.position);
        }
    }

    private void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = 0.15f;

        Gizmos.color = color;

        Gizmos.DrawCube(transform.position, _boundingBox.bounds.extents * 2f);
    }
}

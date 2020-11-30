using UnityEngine;

public class ObjectFallSequence : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _rubbles;
    [SerializeField] private float _probability;

    [SerializeField] private int _xSize;
    [SerializeField] private int _ySize;
    [SerializeField] private int _zSize;

    [SerializeField] private float _partitionSize;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartSequence();
    }

    public void StartSequence()
    {
        for (int x = 0; x < _xSize; x++)
        {
            for (int y = 0; y < _ySize; y++)
            {
                for (int z = 0; z < _zSize; z++)
                {
                    float randomValue = Random.Range(0f, 1f);

                    if (randomValue < _probability)
                    {
                        Vector3 position;
                        position.x = transform.position.x + x * _partitionSize;
                        position.y = transform.position.y + y * _partitionSize;
                        position.z = transform.position.z + z * _partitionSize;

                        Instantiate(_rubbles[Random.Range(0, _rubbles.Length)], position, Random.rotation, transform);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        float width = _xSize * _partitionSize;
        float height = _ySize * _partitionSize;
        float depth = _zSize * _partitionSize;
        Vector3 size = new Vector3(width, height, depth);
        
        Vector3 cubePosition;
        cubePosition.x = transform.position.x + width / 2f;
        cubePosition.y = transform.position.y + height / 2f;
        cubePosition.z = transform.position.z + depth / 2f;

        Color gizmosColor = Color.red;
        gizmosColor.a = 0.25f;

        Gizmos.DrawCube(cubePosition, size);
    }
}

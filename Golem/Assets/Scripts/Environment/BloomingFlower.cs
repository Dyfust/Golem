using UnityEngine;

public class BloomingFlower : MonoBehaviour
{
    private enum FlowerState { NORMAL, BLOOMED }

    [SerializeField] private float _distance;
    [SerializeField] private Animator _anim;
    private FlowerState _state;

    private Transform _orbTransform;

    private bool _orbFound;

    private void Awake()
    {
        _orbTransform = FindObjectOfType<Orb>().transform;
        _orbFound = _orbTransform != null;

        _state = FlowerState.NORMAL;
    }

    private void Update()
    {
        if (_state == FlowerState.NORMAL && _orbFound)
        {
            float distBetweenOrb = (_orbTransform.position - transform.position).magnitude;

            if (distBetweenOrb <= _distance)
            {
                Bloom();
            }
        }
    }

    private void Bloom()
    {
        int index = Random.Range(0, 2);
        _anim.SetBool("Bloom_" + index, true);
        _state = FlowerState.BLOOMED;
    }
}
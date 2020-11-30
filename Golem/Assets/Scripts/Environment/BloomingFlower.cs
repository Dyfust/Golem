using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// When a game object enters the trigger it will activated the bloom animation for the flower 
/// </summary>
public class BloomingFlower : MonoBehaviour
{
	private enum FlowerState { NORMAL, BLOOMED }

	[SerializeField] private float _distance;
	[SerializeField] private Animator _anim;

	private string[] _targetTags;
	private FlowerState _state;

	private List<Transform> _targets;
	private bool _targetFound = false;

	private void Awake()
	{
        _targetTags = new string[2];
        _targetTags[0] = "Orb";
        _targetTags[1] = "MainCamera";

        _targets = new List<Transform>();
        for (int i = 0; i < _targetTags.Length; i++)
        {
            GameObject go = GameObject.FindGameObjectWithTag(_targetTags[i]);

            if (go != null)
                _targets.Add(go.transform);
        }

        if (_targets.Count > 0)
            _targetFound = true;

		_state = FlowerState.NORMAL;
	}

	private void Update()
	{
		if (_state == FlowerState.NORMAL && _targetFound)
		{   
            for (int i = 0; i < _targets.Count; i++)
            {
                Vector3 targetPos = _targets[i].position;
                targetPos.y = transform.position.y;

                float distBetweenTarget = (targetPos - transform.position).magnitude;

			    if (distBetweenTarget <= _distance)
			    {
				    Bloom();
			    }
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
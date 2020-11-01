using UnityEngine;

/// <summary>
/// When a game object enters the trigger it will activated the bloom animation for the flower 
/// </summary>
public class BloomingFlower : MonoBehaviour
{
	enum TARGET
	{
		ORB, 
		MENUCAM
	}

	private enum FlowerState { NORMAL, BLOOMED }

	[SerializeField] private float _distance;
	[SerializeField] private Animator _anim;

	private TARGET _target = TARGET.ORB;
	private string _targetTag; 
	private Transform _targetTranform;
	private FlowerState _state;


	private bool _targetFound;

	private void Awake()
	{
		switch (_target)
		{
			case TARGET.ORB:
				_targetTag = "Orb";
				break;
			case TARGET.MENUCAM:
				_targetTag = "MenuCam";
				break;
		}

		_targetTranform = GameObject.FindGameObjectWithTag(_targetTag).transform;
		_targetFound = _targetTranform != null;

		_state = FlowerState.NORMAL;

	}

	private void Update()
	{
		if (_state == FlowerState.NORMAL && _targetFound)
		{
			float distBetweenOrb = (_targetTranform.position - transform.position).magnitude;

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

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("TEst"))
			Bloom();
	}
}
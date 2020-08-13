using UnityEngine;

public class InteractableCube : MonoBehaviour
{
	[SerializeField] private float _mass; public float mass => _mass;
	[SerializeField] private float _rayPadding = 0;
	[SerializeField] private LayerMask _groundLayer;
	private FixedJoint _fj;
	private Rigidbody _rb;

	private bool _isInteracted = false; 

	void Start()
	{
		_fj = GetComponent<FixedJoint>();
		_rb = GetComponent<Rigidbody>();
	}

	void Update()
	{

		if (_isInteracted == false)
		{
			float halfLength = transform.localScale.x / 2;
			bool _topRight = Physics.Raycast(transform.position + new Vector3(halfLength, -halfLength + 0.1f, halfLength), Vector3.down, _rayPadding, _groundLayer);
			bool _bottomRight = Physics.Raycast(transform.position + new Vector3(halfLength, -halfLength + 0.1f, -halfLength), Vector3.down, _rayPadding, _groundLayer);
			bool _bottomLeft = Physics.Raycast(transform.position + new Vector3(-halfLength, -halfLength + 0.1f, -halfLength), Vector3.down, _rayPadding, _groundLayer);
			bool _topLeft = Physics.Raycast(transform.position + new Vector3(-halfLength, -halfLength + 0.1f, halfLength), Vector3.down, _rayPadding, _groundLayer);

			if (!_topRight && !_topLeft && !_bottomRight && !_bottomLeft)
			{
				if (_fj != null)
					Destroy(_fj);
				if (_rb.isKinematic == true)
					_rb.isKinematic = false;
			}

			if (_topRight || _topLeft || _bottomRight || _bottomLeft)
			{
				if (_fj == null)
					_fj = gameObject.AddComponent<FixedJoint>();
				if (_rb.isKinematic == false)
					_rb.isKinematic = true;
			}
		}
	}

	public void SetIsInteractable(bool state)
	{
		_isInteracted = state; 
	}

	public bool GetIsInteractable()
	{
		return _isInteracted; 
	}
}

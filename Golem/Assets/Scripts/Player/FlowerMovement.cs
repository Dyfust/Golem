using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerMovement : MonoBehaviour
{
	[SerializeField] private Transform _leftShoulder;
	[SerializeField] private Transform _back;
	[SerializeField] private Transform _rightShoulder;
	[SerializeField] private float _speed; 

	private Transform _leftShoulderStartPos;
	private Transform _backStartPos;
	private Transform _rightShoulderStartPos;

	private Vector3 _golemVelocity;
	private Golem _golemRef; 


	// Start is called before the first frame update
	void Start()
	{
		_leftShoulderStartPos = _leftShoulder; 
		_backStartPos = _back;
		_rightShoulderStartPos = _rightShoulder; 
		_golemRef = this.GetComponent<Golem>(); 
	}

	// Update is called once per frame
	void Update()
	{
		_golemVelocity = _golemRef.GetVelocity();
		_golemVelocity.Normalize(); 
		
		if (_golemRef.GetIdle() == false)
		{
			_leftShoulder.position -= (_golemVelocity * Time.deltaTime);
			_back.position -= (_golemVelocity * Time.deltaTime);
			_rightShoulder.position -= (_golemVelocity * Time.deltaTime);
		}

		else if (_golemRef.GetIdle() == true)
		{

		}

	}
}

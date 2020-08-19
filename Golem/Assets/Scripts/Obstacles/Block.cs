using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float _mass; public float mass => _mass;

    private UnityEngine.CharacterController _cc;

	private float _gravity = 10;

	private Vector3 _intialPos; 

	private void Start()
	{
		_cc = GetComponent<UnityEngine.CharacterController>(); 
	}

	private void FixedUpdate()
	{


		//Applying gravity do not remove! 
		_cc.Move(Vector3.down * _gravity * Time.fixedDeltaTime);
	}

	public void Move(Vector3 velocity)
	{
		_cc.Move(velocity);
	}
}

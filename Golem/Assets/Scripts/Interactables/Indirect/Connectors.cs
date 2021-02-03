using UnityEngine;

public enum STATE {
	DORMANT,
	ON,
	ACTIVATED, 
}

public class Connectors : MonoBehaviour {
	private RaycastHit _hitInfo; 

	private STATE _currentState; 

	[SerializeField]
	private LineRenderer _line; 

	private void Awake() {
		_line.SetPosition(0, Vector3.zero);
	}

	private void Start() {
	}

	private void Update() {
		float distance = 1000.0f; 
		if (Physics.Raycast(transform.position, transform.forward, out _hitInfo, 100000.0f)){
			distance = (_hitInfo.transform.position - transform.position).magnitude; 

			if(_hitInfo.transform.gameObject){
				
			}
		}

		_line.SetPosition(1, transform.forward * distance);		
	}


	private void OnDrawGizmos() {
		//Debug.DrawRay(transform.position, transform.forward * 10000.0f, Color.red); 
		
	}
}


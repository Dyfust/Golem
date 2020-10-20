using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemControls : MonoBehaviour
{
	public GameObject _ui;

	private Golem _ref; 
	// Start is called before the first frame update
	void Start()
	{
		_ref = this.GetComponent<Golem>(); 
	}

	// Update is called once per frame
	void Update()
	{
		if (_ref.IsActive() == true)
			_ui.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Orb"))
		{
			_ui.SetActive(true);
		}
	}

	private void OnTriggerStay(Collider other)
	{
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Orb"))
		{
			_ui.SetActive(false);
		}
	}
}

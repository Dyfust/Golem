using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControls : MonoBehaviour
{
	[SerializeField] private GameObject _ui;

	[SerializeField] GameObject _tutGolem;

	private GolemControls _controlsRef;
	private Golem _golemRef;
	private Block _blockRef; 

	// Start is called before the first frame update
	void Start()
	{
		_controlsRef = _tutGolem.GetComponent<GolemControls>();
		_golemRef = _tutGolem.GetComponent<Golem>();
		_blockRef = this.GetComponent<Block>(); 
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Golem"))
		{
			if (_controlsRef._tineText.activeInHierarchy == false && _golemRef.IsActive() == true)
			{
				_ui.SetActive(true); 
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Golem"))
		{
			if (_golemRef.IsActive() == true)
			{
				if (_blockRef.IsConnected() == true)
					_ui.SetActive(false);
				else
					_ui.SetActive(true);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Golem"))
		{
			if (_golemRef.IsActive() == true)
				_ui.SetActive(false);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
	[SerializeField] private GameObject _player;
	[SerializeField] private GameObject _spawn; 
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		StopAllCoroutines();
		StartCoroutine("Teleport");
	}

	private IEnumerator Teleport()
	{
		yield return new WaitForSeconds(2); 
		_player.transform.position = _spawn.transform.position;
	}
}

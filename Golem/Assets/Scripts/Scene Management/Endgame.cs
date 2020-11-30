using UnityEngine;

public class Endgame : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		SceneLoader.instance.LoadScene(2);
	}
}

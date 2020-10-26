using UnityEngine;
using UnityEngine.SceneManagement;

public class Endgame : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		SceneManager.LoadScene("Main Menu"); 
	}
}

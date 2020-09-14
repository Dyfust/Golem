using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [SerializeField] private Scene _scene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Golem") || other.CompareTag("Orb"))
            SceneManager.LoadScene(_scene.buildIndex);
    }
}
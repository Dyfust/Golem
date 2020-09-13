using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(Scene scene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene.buildIndex);

        op.allowSceneActivation = false;
        
    }
}
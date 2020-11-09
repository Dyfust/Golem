using UnityEngine;

public class GenericAnimationEvents : MonoBehaviour
{
    public void SystemPause()
    {
        GameManager.instance.SystemPause();
    }

    public void SystemResume()
    {
        GameManager.instance.SystemResume();
    }

    public void LoadScene(int buildIndex)
    {
        SceneLoader.instance.LoadScene(buildIndex);
    }
}

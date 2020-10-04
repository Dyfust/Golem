using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private AsyncOperation _currentOperation;
    private ScreenFader _screenFader;

    private void Start()
    {
        _screenFader = ScreenFader.instance;
    }

    public void LoadScene(int sceneBuildIndex)
    {
        StartCoroutine(LoadSceneCo(sceneBuildIndex));
    }

    private IEnumerator LoadSceneCo(int sceneBuildIndex)
    {
        yield return StartCoroutine(_screenFader.FadeCo(ScreenFader.FadeType.OUT, false));
        yield return StartCoroutine(LoadSceneAsync(sceneBuildIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneBuildIndex)
    {
        _currentOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        _currentOperation.allowSceneActivation = false;

        while (_currentOperation.progress < 0.9f)
        {
            yield return null;
        }

        Debug.Log("Scene ready to activate!");
        _currentOperation.allowSceneActivation = true;
    }
}
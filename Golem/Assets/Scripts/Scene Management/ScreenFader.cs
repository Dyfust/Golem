using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    private static ScreenFader _instance;
    public static ScreenFader instance => _instance;

    private static float _defaultFadeDuration = 0.5f;

    public enum FadeType { IN, OUT }

    [SerializeField] private Image _screenFadeUI;
    [Tooltip("How long the screen will stay black after fading out")]
    [SerializeField] private float _intialFadePauseDuration;
    [SerializeField] private bool _fadeOnInit;
    [SerializeField] private bool _pause;
    [Tooltip("Time it takes for the fade to complete")]
    [SerializeField] private float _fadeDuration; 

    private bool _fading = false;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        if (_fadeOnInit)
            Fade(FadeType.IN, _fadeOnInit);
    }

    public IEnumerator FadeCo(FadeType type, bool init)
    {
        float duration = _fadeDuration > 0 ? _fadeDuration : _defaultFadeDuration; 

        if (type == FadeType.OUT)
            _screenFadeUI.color = new Color(0f, 0f, 0f, 0f);
        else
            _screenFadeUI.color = new Color(0f, 0f, 0f, 1f);

        _screenFadeUI.gameObject.SetActive(true);

        if (init || _pause == true && type == FadeType.IN)
            yield return new WaitForSecondsRealtime(_intialFadePauseDuration);

        float start;
        float end;

        if (type == FadeType.OUT)
        {
            start = 0f;
            end = 1f;
        }
        else
        {
            start = 1f;
            end = 0f;
        }

        float startTime = Time.time;
        float t = 0f;

        _fading = true;

        while (t <= 1f)
        {
            float elapsedTime = Time.time - startTime;
            t = elapsedTime / duration;

            float alpha = Mathf.Lerp(start, end, t);

            _screenFadeUI.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        _fading = false;

        if (type == FadeType.IN)
            _screenFadeUI.gameObject.SetActive(false);
    }

    public void Fade(FadeType type, bool init)
    {
        if (_fading)
            return;

        if (type == FadeType.IN)
            StartCoroutine(FadeCo(type, init));
        else
            StartCoroutine(FadeCo(type, init));
    }
}
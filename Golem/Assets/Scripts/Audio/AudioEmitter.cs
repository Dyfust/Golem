using UnityEngine;
using System.Collections;

public class AudioEmitter : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _fadeoutDuration;
    private float t = 0f;

    private void Start()
    {
        _source.loop = true;
        _source.playOnAwake = false;
        _source.clip = _clip;
    }

    public void Play()
    {
        //StopCoroutine(FadeCoroutine());
        _source.Play();
    }

    public void Stop()
    {
        _source.Stop();
        //StopCoroutine(FadeCoroutine());
        //StartCoroutine(FadeCoroutine());
    }

    public void SetValue(float t)
    {
        t = Mathf.Clamp01(t);
        _source.volume = t;
    }

    private IEnumerator FadeCoroutine()
    {
        float speed = _source.volume / _fadeoutDuration;

        while (_source.volume > 0)
        {
            _source.volume = Mathf.MoveTowards(_source.volume, 0f, speed * Time.deltaTime);
            yield return null;
        }

        _source.Stop();
    }
}
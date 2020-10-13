using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;
    private float t = 0f;

    private void Start()
    {
        _source.loop = true;
        _source.playOnAwake = false;
        _source.clip = _clip;
    }

    public void Play()
    {
        _source.Play();
    }

    public void Stop()
    {
        _source.Stop();
    }

    public void SetValue(float t)
    {
        t = Mathf.Clamp01(t);
        _source.volume = t;
    }
}
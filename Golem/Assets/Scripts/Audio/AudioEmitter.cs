using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _fadeInDuration = 1f;
    [SerializeField] private float _fadeOutDuration = 1f;

    private int _fade; // 0 = fade out, 1 = fade in
    private float _fadeSpeed;
    private bool _fading = false;

    private void Start()
    {
        _source.loop = true;
        _source.playOnAwake = false;
        _source.clip = _clip;
    }

    private void Update()
    {
        if (_fading)
        {
            SetVolume(Mathf.MoveTowards(_source.volume, _fade, _fadeSpeed * Time.deltaTime));

            if (_source.volume == _fade)
                _fading = false;

            if (_source.volume == 0f && _fading == false)
                _source.Stop();
        }
    }

    public void Play()
    {
        _source.Play();
        Fade(1);
    }

    public void Stop()
    {
        Fade(0);
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        _source.volume = volume;
    }

    private void Fade(int fade)
    {
        _fade = Mathf.Clamp(fade, 0, 1);

        float time = fade == 1 ? _fadeInDuration : _fadeOutDuration;
        _fadeSpeed = Mathf.Abs(fade - _source.volume) / time;
        _fading = true;
    }
}
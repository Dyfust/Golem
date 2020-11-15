using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _maxVolume = 1f;
    [SerializeField] private float _fadeInDuration = 1f;
    [SerializeField] private float _fadeOutDuration = 1f;

    private int _fade; // 0 = fade out, 1 = fade in
    private float _targetVolume;
    private float _fadeSpeed;
    private bool _fading = false;

    private void Start()
    {
        _source.loop = true;
        _source.playOnAwake = false;
        _source.clip = _clip;
        _source.volume = _maxVolume;
    }

    private void Update()
    {
        if (_fading)
        {
            SetVolume(Mathf.MoveTowards(_source.volume, _targetVolume, _fadeSpeed * Time.deltaTime));

            if (_source.volume == _fade)
                _fading = false;

            if (_source.volume == 0f && _fading == false)
                _source.Stop();
        }
    }

    public void Play(bool fade = true)
    {
        _source.Play();

        if (fade)
            Fade(1);
        else
            _fading = false;
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
        _targetVolume = _maxVolume * _fade;

        float time = fade == 1 ? _fadeInDuration : _fadeOutDuration;
        _fadeSpeed = Mathf.Abs(_targetVolume - _source.volume) / time;
        _fading = true;
    }

    public bool IsPlaying() => _source.isPlaying;
}
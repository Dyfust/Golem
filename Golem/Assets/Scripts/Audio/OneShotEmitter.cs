using UnityEngine;

public class OneShotEmitter : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _clips;

    [Range(-3f, 3f)]
    [SerializeField] private float _minPitch;

    [Range(-3f, 3f)]
    [SerializeField] private float _maxPitch;

    public void Play()
    {
        AudioClip clip = _clips[Random.Range(0, _clips.Length)];
        float pitch = Random.Range(_minPitch, _maxPitch);

        _source.pitch = pitch;
        _source.PlayOneShot(clip);
    }
}
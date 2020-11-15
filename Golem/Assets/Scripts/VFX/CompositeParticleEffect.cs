using UnityEngine;

public class CompositeParticleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleEffects;

    public void PlayEffect()
    {
        for (int i = 0; i < _particleEffects.Length; i++)
        {
            _particleEffects[i].Play();
        }
    }
    
    public void StopEffect()
    {
        for (int i = 0; i < _particleEffects.Length; i++)
        {
            _particleEffects[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}

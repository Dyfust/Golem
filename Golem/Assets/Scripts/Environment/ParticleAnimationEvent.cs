using UnityEngine;

public class ParticleAnimationEvent : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleEffect;

    public void PlayParticle()
    {
        _particleEffect.Play();
    }
}

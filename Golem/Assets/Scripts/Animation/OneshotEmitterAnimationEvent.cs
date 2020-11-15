using UnityEngine;

public class OneshotEmitterAnimationEvent : MonoBehaviour
{
    [SerializeField] private OneShotEmitter _emitter;

    public void Emit()
    {
        _emitter.Play();
    }
}
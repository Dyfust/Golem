using UnityEngine;

public class VirtualCamera : MonoBehaviour
{
    [Tooltip("Higher number means higher priority")]
    [SerializeField] private int _cameraPriority; 

    public int GetCameraPriority() => _cameraPriority; 
}
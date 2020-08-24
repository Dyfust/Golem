using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VirtualCamera : MonoBehaviour
{
    [Tooltip("Higher number means higher priority")]
    [SerializeField] private int _cameraPriority; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCameraPriority() => _cameraPriority; 
}

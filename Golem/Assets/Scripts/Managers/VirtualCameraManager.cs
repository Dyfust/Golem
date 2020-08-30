using Cinemachine;
using UnityEngine;

// NEED TO DO PRIORITY SYSTEM.

public class VirtualCameraManager : MonoBehaviour
{
    private static VirtualCameraManager _instance;
    public static VirtualCameraManager instance => _instance;

    private const string _identifierTag = "VCam";

    // 1.) Limited to one active VCAM.
    [SerializeField] private Camera _brain; 
    [SerializeField] private VirtualCamera _defaultVirtualCamera;

    [SerializeField] private CinemachineFreeLook _orbFreeLookCM;
    [SerializeField] private CinemachineFreeLook _golemFreeLookCM;
    private VirtualCamera _orbVirtualCamera;
    private VirtualCamera _golemVirtualCamera;

    private VirtualCamera _currentVirtualCamera;
    private VirtualCamera _cachedPlayerCamera;
    private VirtualCamera[] _virtualCameras;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("Multiple VCAM managers present!");

        _orbVirtualCamera = _orbFreeLookCM.GetComponent<VirtualCamera>();
        _golemVirtualCamera = _golemFreeLookCM.GetComponent<VirtualCamera>();

        GameObject[] vcamGOs = GameObject.FindGameObjectsWithTag(_identifierTag);
        _virtualCameras = new VirtualCamera[vcamGOs.Length];
        Debug.Log(_virtualCameras.Length);

        for (int i = 0; i < vcamGOs.Length; i++)
        {
            _virtualCameras[i] = vcamGOs[i].GetComponent<VirtualCamera>();
        }

        ForceToggleCam(_defaultVirtualCamera);
    }

    private void OnEnable()
    {
        Orb.OnOrbActive += ToggleOrbCamera;
        Golem.OnGolemActive += ToggleGolemCamera;
    }

    private void OnDisable()
    {
        Orb.OnOrbActive -= ToggleOrbCamera;
        Golem.OnGolemActive -= ToggleGolemCamera;
    }

    private void ToggleOrbCamera(Orb orb, Quaternion orientation)
    {
        _orbFreeLookCM.Follow = orb.transform;
        _orbFreeLookCM.LookAt = orb.transform;
        _orbFreeLookCM.m_XAxis.Value = orientation.eulerAngles.y;
        ForceToggleCam(_orbVirtualCamera);
    }

    private void ToggleGolemCamera(Golem golem, Quaternion orientation)
    {
        _golemFreeLookCM.Follow = golem.transform;
        _golemFreeLookCM.LookAt = golem.transform;
        _golemFreeLookCM.m_XAxis.Value = orientation.eulerAngles.y;
        ForceToggleCam(_golemVirtualCamera);
    }

    private void ForceToggleCam(VirtualCamera cam)
    {
        CachePlayerCamera(cam);
        _brain.cullingMask = ~cam.GetCullLayer(); 
        _currentVirtualCamera = cam;
        _currentVirtualCamera.gameObject.SetActive(true);
        for (int i = 0; i < _virtualCameras.Length; i++)
        {
            if (_virtualCameras[i] != _currentVirtualCamera)
                _virtualCameras[i].gameObject.SetActive(false);
        }
    }

    public void ToggleCamera(VirtualCamera cam)
    {
        if (cam.GetCameraPriority() >= _currentVirtualCamera.GetCameraPriority())
        {
            ForceToggleCam(cam);
        }
        else
        {
            CachePlayerCamera(cam);
        }
    }

    private void CachePlayerCamera(VirtualCamera cam)
    {
        if (cam == _golemVirtualCamera || cam == _orbVirtualCamera)
            _cachedPlayerCamera = cam;
    }

    public void TogglePlayerCamera()
    {
        ForceToggleCam(_cachedPlayerCamera);
    }
}
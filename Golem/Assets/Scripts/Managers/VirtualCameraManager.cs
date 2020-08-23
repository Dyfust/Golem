using Cinemachine;
using UnityEngine;

// NEED TO DO PRIORITY SYSTEM.

public class VirtualCameraManager : MonoBehaviour
{
    private static VirtualCameraManager _instance;
    public static VirtualCameraManager instance => _instance;

    private const string _identifierTag = "VCam";

    // 1.) Limited to one active VCAM.
    [SerializeField] private GameObject _defaultVirtualCamera;
    [SerializeField] private CinemachineFreeLook _orbVirtualCamera;
    [SerializeField] private CinemachineFreeLook _golemVirtualCamera;


    private GameObject _currentVirtualCamera;
    private GameObject _previousPlayerCamera;
    private GameObject[] _virtualCameras;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("Multiple VCAM managers present!");

        _virtualCameras = GameObject.FindGameObjectsWithTag(_identifierTag);

        ToggleVCam(_defaultVirtualCamera);
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

    private void ToggleOrbCamera()
    {
        ToggleVCam(_orbVirtualCamera.gameObject);
    }

    private void ToggleGolemCamera(Golem golem)
    {
        _golemVirtualCamera.Follow = golem.transform;
        _golemVirtualCamera.LookAt = golem.transform;
        ToggleVCam(_golemVirtualCamera.gameObject);
    }

    public void ToggleVCam(GameObject vcam)
    {
        if (_currentVirtualCamera == _golemVirtualCamera.gameObject || _currentVirtualCamera == _orbVirtualCamera.gameObject)
            _previousPlayerCamera = _currentVirtualCamera;

        _currentVirtualCamera = vcam;

        vcam.SetActive(true);
        for (int i = 0; i < _virtualCameras.Length; i++)
        {
            if (_virtualCameras[i] != vcam)
                _virtualCameras[i].SetActive(false);
        }
    }

    public void TogglePreviousPlayerCamera()
    {
        ToggleVCam(_previousPlayerCamera);
    }
}
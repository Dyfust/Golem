using Cinemachine;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;

// NEED TO DO PRIORITY SYSTEM.

public class VirtualCameraManager : MonoBehaviour
{
	private static VirtualCameraManager _instance;
	public static VirtualCameraManager instance => _instance;

	private const string _identifierTag = "VCam";

	// 1.) Limited to one active VCAM.
	[SerializeField] private Camera _mainCamera;
	[SerializeField] private VirtualCamera _defaultVirtualCamera;

	[SerializeField] private CinemachineFreeLook _orbFreeLookCM;
	[SerializeField] private CinemachineFreeLook _golemFreeLookCM;
	private VirtualCamera _orbVirtualCamera;
	private VirtualCamera _golemVirtualCamera;

	private VirtualCamera _currentVirtualCamera;
	private VirtualCamera _cachedPlayerCamera;
	private VirtualCamera[] _virtualCameras;

	private Cinemachine.CinemachineBrain _brain;

	private void Awake()
	{
		_brain = _mainCamera.GetComponent<CinemachineBrain>();

		if (_instance == null)
			_instance = this;
		else
			Debug.LogWarning("Multiple VCAM managers present!");

		_orbVirtualCamera = _orbFreeLookCM.GetComponent<VirtualCamera>();
		_golemVirtualCamera = _golemFreeLookCM.GetComponent<VirtualCamera>();

		GameObject[] vcamGOs = GameObject.FindGameObjectsWithTag(_identifierTag);
		_virtualCameras = new VirtualCamera[vcamGOs.Length];

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

	private void ToggleOrbCamera(Orb orb)
	{
		_orbFreeLookCM.Follow = orb.transform;
		_orbFreeLookCM.LookAt = orb.transform;
		ForceToggleCam(_orbVirtualCamera);
	}

	private void ToggleGolemCamera(Golem golem)
	{
		_golemFreeLookCM.Follow = golem.transform;
		_golemFreeLookCM.LookAt = golem.transform;
		ForceToggleCam(_golemVirtualCamera);
	}

	private void ForceToggleCam(VirtualCamera cam)
	{
		CachePlayerCamera(cam);
		_mainCamera.cullingMask = ~cam.GetCullLayer();
		_brain.m_UpdateMethod = (Cinemachine.CinemachineBrain.UpdateMethod)cam.GetUpdateMethod();
		_currentVirtualCamera = cam;
		_currentVirtualCamera.gameObject.SetActive(true);
		for (int i = 0; i < _virtualCameras.Length; i++)
		{
			if (_virtualCameras[i] != _currentVirtualCamera)
				_virtualCameras[i].gameObject.SetActive(false);
		}
	}

	public void ToggleExternalCamera(VirtualCamera cam)
	{
		StopAllCoroutines();
		StartCoroutine(ToggleExternalCameraCo(cam)); 
	}

	private IEnumerator ToggleExternalCameraCo(VirtualCamera cam)
	{
		yield return StartCoroutine("FadeOut"); 
		if (cam.GetCameraPriority() >= _currentVirtualCamera.GetCameraPriority())
		{
			ForceToggleCam(cam);
		}
		else
		{
			CachePlayerCamera(cam);
		}
		yield return StartCoroutine("FadeIn"); 
	}

	private void CachePlayerCamera(VirtualCamera cam)
	{
		if (cam == _golemVirtualCamera || cam == _orbVirtualCamera)
			_cachedPlayerCamera = cam;
	}

	public void TogglePlayerCamera()
	{
		StopAllCoroutines();
		StartCoroutine("TogglePlayerCameraCo");
	}
	private IEnumerator TogglePlayerCameraCo()
	{
		yield return StartCoroutine("FadeOut"); 
		ForceToggleCam(_cachedPlayerCamera);
		yield return StartCoroutine("FadeIn");
	}


	IEnumerator FadeOut()
	{
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.OUT, false));
	}

	IEnumerator FadeIn()
	{
		yield return StartCoroutine(ScreenFader.instance.FadeCo(ScreenFader.FadeType.IN, false));
	}
}
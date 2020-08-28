using UnityEngine;

//struct CullingMask
//{

//}
public class VirtualCamera : MonoBehaviour
{
	[Tooltip("Higher number means higher priority")]
	[SerializeField] private int _cameraPriority;

	[SerializeField] private LayerMask _layerToCull; 

	public int GetCameraPriority() => _cameraPriority;

	public LayerMask GetCullLayer() => _layerToCull; 


}
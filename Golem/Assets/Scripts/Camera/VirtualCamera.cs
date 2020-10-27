using UnityEngine;

/// <summary>
/// Container for a virtual camera and used for identification in scene 
/// </summary>
public class VirtualCamera : MonoBehaviour
{
	[Tooltip("Higher number means higher priority")]
	[SerializeField] private int _cameraPriority;

	[SerializeField] private LayerMask _layerToCull;

	[SerializeField] private Cinemachine.CinemachineBrain.BrainUpdateMethod _desiredUpdateMethod;

	public Cinemachine.CinemachineBrain.BrainUpdateMethod GetUpdateMethod() =>  _desiredUpdateMethod; 

	public int GetCameraPriority() => _cameraPriority;

	public LayerMask GetCullLayer() => _layerToCull; 


}
using UnityEngine;

public class ResetSystem : MonoBehaviour
{
	[SerializeField] private ResetBoundingBox _startingBoundingBox; 

	private static ResetSystem _instance;
	public static ResetSystem instance => _instance;

	private const string _identifierTag = "Room"; 

	private ResetBoundingBox[] _boundingBoxes;
	private ResetBoundingBox _currentBoundingBox;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else
			Debug.LogWarning("Multiple reset systems present!");
	}

	public void SetCurrentBoundingBox(ResetBoundingBox currentBoundingBox)
	{
		_currentBoundingBox.OnExit();
		_currentBoundingBox = currentBoundingBox;
	}
}
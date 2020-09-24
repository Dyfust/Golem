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

		GameObject[] boundingBoxGOs = GameObject.FindGameObjectsWithTag(_identifierTag);
		_boundingBoxes = new ResetBoundingBox[boundingBoxGOs.Length];

		for (int i = 0; i < _boundingBoxes.Length; i++)
		{
			_boundingBoxes[i] = boundingBoxGOs[i].GetComponent<ResetBoundingBox>(); 
		}

		_currentBoundingBox = _startingBoundingBox; 
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			_currentBoundingBox.Reset(); 
		}
	}

	public void SetCurrentBoundingBox(ResetBoundingBox currentBoundingBox) => _currentBoundingBox = currentBoundingBox;
}
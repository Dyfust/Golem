using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    public static CursorManager instance => _instance; 

    [SerializeField] private bool _defaultState = false;
    private bool _state;

	private void Awake()
	{
        if (_instance == null)
            _instance = this;
	}
	private void Start()
    {
        ToggleCursor(_defaultState);
    }

    private void Update()
    {
		//if (Input.GetKeyDown(KeyCode.Escape))
		//	ToggleCursor();
	}

    private void ToggleCursor()
    {
        _state = !_state;
        Cursor.lockState = _state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _state;
    }

    public void ToggleCursor(bool state)
    {
        _state = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }
}

using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager _instance;
    public static CursorManager instance => _instance; 

    private bool _state = false;

	private void Awake()
	{
        if (_instance == null)
            _instance = this;
	}
	private void Start()
    {
        ToggleCursor(_state);
    }

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
			ToggleCursor();
	}

    private void ToggleCursor()
    {
        _state = !_state;
        Cursor.lockState = _state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _state;
    }

    public void ToggleCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
        _state = state;
    }
}

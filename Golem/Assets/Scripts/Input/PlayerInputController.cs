using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
	private IRequireInput _dest;
	private PlayerInputData _localInputData;
	private InputMaster controls; 

	private void Awake()
	{
		_dest = GetComponent<IRequireInput>();
		controls = new InputMaster();
	}	

	private void OnEnable()
	{
		controls.Gameplay.Enable();

		controls.Gameplay.PushPull.performed += (c) => { Debug.Log(c); };
	}

	private void OnDisable()
	{
		controls.Gameplay.Disable(); 
	}

	private void Update()
	{
		_localInputData.axes = controls.Gameplay.Movement.ReadValue<Vector2>();
		_localInputData.normalizedAxes = _localInputData.axes.normalized;
		_localInputData.joystickDepth = controls.Gameplay.Movement.ReadValue<Vector2>().magnitude;
		_localInputData.enterButtonPressedThisFrame = controls.Gameplay.EnterExit.triggered;
		_localInputData.pushButtonPressedThisFrame = controls.Gameplay.PushPull.triggered;
		_localInputData.liftButtonPressedThisFrame = controls.Gameplay.Lift.triggered;

		_dest.SetInputData(_localInputData);
	}
}
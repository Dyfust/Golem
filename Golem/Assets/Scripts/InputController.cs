using UnityEngine;

public class InputController : MonoBehaviour
{
	private IRequireInput _dest;
	private InputData _localInputData;
	private InputMaster controls; 

	private void Awake()
	{
		_dest = GetComponent<IRequireInput>();
		controls = new InputMaster();
	}	

	private void OnEnable()
	{
		controls.Gameplay.Enable();
	}

	private void OnDisable()
	{
		controls.Gameplay.Disable(); 
	}

	private void Update()
	{
		_localInputData.movement = controls.Gameplay.Movement.ReadValue<Vector2>();
		_localInputData.normalisedMovement = _localInputData.movement.normalized;
		_localInputData.enterButtonPress = controls.Gameplay.EnterExit.triggered;
		_localInputData.pushButtonPress = controls.Gameplay.PushPull.triggered;
		_localInputData.liftButtonPress = controls.Gameplay.Lift.triggered;

		_dest.SetInputData(_localInputData);
	}
}
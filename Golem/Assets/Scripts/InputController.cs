using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class InputController : MonoBehaviour
{
	private IRequireInput _dest;
	private InputData _localInputData;
	[SerializeField] InputMaster controls; 

	private void Awake()
	{
		_dest = GetComponent<IRequireInput>();
		controls = new InputMaster();
	}	

	private void OnEnable()
	{
		controls.Player.Enable();
	}

	private void OnDisable()
	{
		controls.Player.Disable(); 
	}

	private void Update()
	{
		_localInputData = new InputData();

		_localInputData.movement = controls.Player.Movement.ReadValue<Vector2>();
		_localInputData.normalisedMovement = _localInputData.movement; 
		_localInputData.enterButtonPress = controls.Player.EnterExit.triggered;
		_localInputData.pushButtonPress = controls.Player.PushPull.triggered;
		_localInputData.liftButtonPress = controls.Player.Lift.triggered;
		

		_dest.SetInputData(_localInputData);
	}

}
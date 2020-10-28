using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GlobalInput : MonoBehaviour
{
	private static GlobalInput _instance;
	public static GlobalInput instance => _instance;

	public enum DEVICE { KEYBOARD, GAMEPAD }
	private DEVICE _currentDevice;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;

		_currentDevice = DEVICE.KEYBOARD; 
	}
	private void OnEnable()
	{
		var user = InputUser.CreateUserWithoutPairedDevices();
		InputUser.PerformPairingWithDevice(Mouse.current, user);

		++InputUser.listenForUnpairedDeviceActivity;
		InputUser.onUnpairedDeviceUsed += (c, e) =>
		{
			var device = c.device;
			Debug.Log(device.name);

			if (device == Keyboard.current)
				_currentDevice = DEVICE.KEYBOARD;
			else if (device == Gamepad.current)
				_currentDevice = DEVICE.GAMEPAD;
		};
	}

	public DEVICE GetCurrentInputMethod()
	{
		return _currentDevice; 
	}
}

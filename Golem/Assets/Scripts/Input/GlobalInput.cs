using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GlobalInput : MonoBehaviour
{
    public enum DEVICE {KEYBOARD, GAMEPAD}
    private DEVICE _currentDevice;

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
}

using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerShake : MonoBehaviour
{
	public void SetShake(float lowFreq, float highFeq)
	{
		if (Gamepad.current != null)
			Gamepad.current.SetMotorSpeeds(lowFreq, highFeq);
		else
			DebugWrapper.Log("No Controller Found");
	}

	public void StopShake()
	{
		if (Gamepad.current != null)
			Gamepad.current.SetMotorSpeeds(0.0f, 0.0f); 
		else
			DebugWrapper.Log("No Controller Found");
	}
}

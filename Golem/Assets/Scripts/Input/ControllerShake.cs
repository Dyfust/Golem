using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class ControllerShake : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetShake(float lowFreq, float highFeq)
	{
		if (Gamepad.current != null)
			Gamepad.current.SetMotorSpeeds(lowFreq, highFeq); 
	}

	public void StopShake()
	{
		if (Gamepad.current != null)
		{
			Gamepad.current.SetMotorSpeeds(0.0f, 0.0f); 
		}
	}
}

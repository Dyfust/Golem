using UnityEngine;

[System.Serializable]
public struct PlayerInputData
{
    public Vector2 axes;
    public Vector2 normalizedAxes;
    public float joystickDepth;
    public bool pushButtonPressedThisFrame;
    public bool liftButtonPressedThisFrame;
    public bool enterButtonPressedThisFrame;
}
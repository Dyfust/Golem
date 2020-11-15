using UnityEngine;

[CreateAssetMenu(fileName = "Slider Value", menuName = "Values/Slider Value", order = 0)]
public class SliderValue : ScriptableObject
{
    public float value;
    public float min;
    public float max;
}
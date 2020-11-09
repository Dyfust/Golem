using UnityEngine;
using UnityEngine.UI;

public class SliderComp : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private SliderValue _value;

    private void Start()
    {
        _slider.value = Mathf.InverseLerp(_value.min, _value.max, _value.value);
    }

    public void OnValueChanged()
    {
        _value.value = Mathf.Lerp(_value.min, _value.max, _slider.value);
    }
}

[CreateAssetMenu(fileName = "Slider Value", menuName = "Values/Slider Value", order = 0)]
public class SliderValue : ScriptableObject
{
    public float value;
    public float min;
    public float max;
}

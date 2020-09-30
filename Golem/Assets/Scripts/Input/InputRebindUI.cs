using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputRebindUI : MonoBehaviour
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private Text _keybindingText;

    private InputAction _action;

    private static Dictionary<string, string> _controlSchemeMap = new Dictionary<string, string>();

    private void Awake()
    {
        _controlSchemeMap.Add("Keyboard", "Keyboard");
        _controlSchemeMap.Add("Mouse", "Keyboard");

        _controlSchemeMap.Add("XInputControllerWindows", "Xbox");

        _action = _actionReference.action;
        _keybindingText.text = _action.GetBindingDisplayString();
    }
 
    public void Rebind()
    {
        var operation = _action.PerformInteractiveRebinding();

        operation.WithCancelingThrough("<Keyboard>/escape");

        operation.OnApplyBinding(OnApplyBindingCallback);
        operation.OnComplete(OnCompleteBindingCallBack);


        operation.Start();
    }

    private void OnApplyBindingCallback(InputActionRebindingExtensions.RebindingOperation operation, string path)
    {
        string device = operation.selectedControl.device.name;

        if (_controlSchemeMap.TryGetValue(device, out string bindingGroup))
        {
            operation.action.ApplyBindingOverride(path, bindingGroup);
        }
        else
        {
            Debug.LogWarning("Device couldn't be mapped to a control scheme/binding group");
        }
    }

    private void OnCompleteBindingCallBack(InputActionRebindingExtensions.RebindingOperation operation)
    {
        _keybindingText.text = operation.action.GetBindingDisplayString();
        operation.Dispose();
    }
}

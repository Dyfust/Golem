// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/Unity Input System/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""c9846cad-ecdc-4b9d-8059-b0e472762578"",
            ""actions"": [
                {
                    ""name"": ""Enter/Exit"",
                    ""type"": ""Button"",
                    ""id"": ""42551425-8d8c-47bb-b7d7-cbd0cd9647ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Push/Pull"",
                    ""type"": ""Button"",
                    ""id"": ""98cb368a-1a76-4260-afa8-426709483eb5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Lift"",
                    ""type"": ""Button"",
                    ""id"": ""fdc3bcae-7eef-4099-b8c9-98f54be9575f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""00d88d9f-9865-4f01-b084-341463ff3ddf"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""80aa9edf-f6fa-4b11-ba4d-f0ac7cfb6862"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Push/Pull"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d0a5d865-502c-45b6-a230-fd380a82c333"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Push/Pull"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""16802f76-5ca3-4c54-9c49-798c02fc9410"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a17a0235-7f9e-45bb-af06-403349c4a72b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0ccc2a37-5d6c-485e-b752-888586c0b70f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""92abf08c-bf3c-41f0-89fe-08fbf448783f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2a1bebe0-9fee-45e5-b403-47696fc0ad40"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""0c4dbbbe-ef5d-47d1-9afa-d5df3a1ebb0a"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c5ec4b37-f68a-48a7-842a-e261dbdbcfc7"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ccd09c21-9277-4f51-a344-c6098cd4862b"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""44018047-38a2-4c7c-ae8c-8e99e898ab37"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""565fa435-c423-4ae0-bdc0-ffc9a8552104"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c7f8b657-779a-40a2-ba3c-dff876dd8a76"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Lift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9860ca63-af3e-4724-a996-7159498e07e0"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Lift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f321e4b-b735-40d4-9b92-26de8c28ecc6"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Enter/Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""72899f1e-9a4d-4e09-849b-3dfdc4a9aab5"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Enter/Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""6e76ad43-aebc-4ce3-9d13-5aede2a1d980"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2b90db21-79ef-4eb4-8d3a-03a14bb2c28e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b9d54ca2-169d-4f31-8540-646e6a4e4e3d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.25,y=0.25)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""730eb229-5853-4d28-b3db-b5ed9caace07"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=2,y=2)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""282895fa-d272-45e8-bd46-c96c3101692f"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""99894e41-b7f3-4fab-b75d-7b79413dca19"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9d6bdf6d-edf1-4de9-bc8e-2c24e1e50a6c"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e4c63f9f-129a-45b8-a6af-f6039d5dc25c"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""MenuNavigation"",
            ""id"": ""38f26d80-0f2a-445f-9a9f-588407c0be50"",
            ""actions"": [
                {
                    ""name"": ""ConfirmButtonPress"",
                    ""type"": ""Button"",
                    ""id"": ""4e1f91f6-3ab5-42ae-946f-73515ca1bdad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""IndexUp"",
                    ""type"": ""Button"",
                    ""id"": ""1400b5eb-2eff-4921-83c9-d6adb5924116"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""IndexDown"",
                    ""type"": ""Button"",
                    ""id"": ""e12e183e-814a-49bb-a53e-da7f72631c37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ReturnButtonPress"",
                    ""type"": ""Button"",
                    ""id"": ""e8d8fe8e-77fe-47e4-9a75-50b23c435f26"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SliderIncrease"",
                    ""type"": ""Button"",
                    ""id"": ""6f5faf4b-7ac7-4c99-9d99-954307fc1cc4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SliderDecrease"",
                    ""type"": ""Button"",
                    ""id"": ""bdd137b0-bd86-4f8d-b5b3-576645b61bdf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""007bc931-d85b-48f2-b5ed-bdfce59a728f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""33ed8433-de03-44c6-97c0-ffffb63820cd"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""ConfirmButtonPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81589fca-c7fe-4504-949b-f7e30d1f1da0"",
                    ""path"": ""<XInputController>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""IndexUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5aad2e44-eb1a-49a5-b288-65af7b4270f3"",
                    ""path"": ""<XInputController>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""IndexDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b626172-385f-45b7-896f-8588ed54a4dd"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""ReturnButtonPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""edd630ec-efc8-4765-a9e3-57b300f5e589"",
                    ""path"": ""<XInputController>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SliderIncrease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf8d731f-fb53-4ef3-91ac-164bd37bc003"",
                    ""path"": ""<XInputController>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XBox"",
                    ""action"": ""SliderDecrease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18fa225f-3c7c-4c90-9842-d12979d466ab"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XBox"",
            ""bindingGroup"": ""XBox"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_EnterExit = m_Gameplay.FindAction("Enter/Exit", throwIfNotFound: true);
        m_Gameplay_PushPull = m_Gameplay.FindAction("Push/Pull", throwIfNotFound: true);
        m_Gameplay_Lift = m_Gameplay.FindAction("Lift", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_Look = m_Camera.FindAction("Look", throwIfNotFound: true);
        // MenuNavigation
        m_MenuNavigation = asset.FindActionMap("MenuNavigation", throwIfNotFound: true);
        m_MenuNavigation_ConfirmButtonPress = m_MenuNavigation.FindAction("ConfirmButtonPress", throwIfNotFound: true);
        m_MenuNavigation_IndexUp = m_MenuNavigation.FindAction("IndexUp", throwIfNotFound: true);
        m_MenuNavigation_IndexDown = m_MenuNavigation.FindAction("IndexDown", throwIfNotFound: true);
        m_MenuNavigation_ReturnButtonPress = m_MenuNavigation.FindAction("ReturnButtonPress", throwIfNotFound: true);
        m_MenuNavigation_SliderIncrease = m_MenuNavigation.FindAction("SliderIncrease", throwIfNotFound: true);
        m_MenuNavigation_SliderDecrease = m_MenuNavigation.FindAction("SliderDecrease", throwIfNotFound: true);
        m_MenuNavigation_Pause = m_MenuNavigation.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_EnterExit;
    private readonly InputAction m_Gameplay_PushPull;
    private readonly InputAction m_Gameplay_Lift;
    private readonly InputAction m_Gameplay_Movement;
    public struct GameplayActions
    {
        private @InputMaster m_Wrapper;
        public GameplayActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @EnterExit => m_Wrapper.m_Gameplay_EnterExit;
        public InputAction @PushPull => m_Wrapper.m_Gameplay_PushPull;
        public InputAction @Lift => m_Wrapper.m_Gameplay_Lift;
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @EnterExit.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEnterExit;
                @EnterExit.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEnterExit;
                @EnterExit.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnEnterExit;
                @PushPull.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPushPull;
                @PushPull.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPushPull;
                @PushPull.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPushPull;
                @Lift.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLift;
                @Lift.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLift;
                @Lift.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLift;
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @EnterExit.started += instance.OnEnterExit;
                @EnterExit.performed += instance.OnEnterExit;
                @EnterExit.canceled += instance.OnEnterExit;
                @PushPull.started += instance.OnPushPull;
                @PushPull.performed += instance.OnPushPull;
                @PushPull.canceled += instance.OnPushPull;
                @Lift.started += instance.OnLift;
                @Lift.performed += instance.OnLift;
                @Lift.canceled += instance.OnLift;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_Look;
    public struct CameraActions
    {
        private @InputMaster m_Wrapper;
        public CameraActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Camera_Look;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @Look.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);

    // MenuNavigation
    private readonly InputActionMap m_MenuNavigation;
    private IMenuNavigationActions m_MenuNavigationActionsCallbackInterface;
    private readonly InputAction m_MenuNavigation_ConfirmButtonPress;
    private readonly InputAction m_MenuNavigation_IndexUp;
    private readonly InputAction m_MenuNavigation_IndexDown;
    private readonly InputAction m_MenuNavigation_ReturnButtonPress;
    private readonly InputAction m_MenuNavigation_SliderIncrease;
    private readonly InputAction m_MenuNavigation_SliderDecrease;
    private readonly InputAction m_MenuNavigation_Pause;
    public struct MenuNavigationActions
    {
        private @InputMaster m_Wrapper;
        public MenuNavigationActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @ConfirmButtonPress => m_Wrapper.m_MenuNavigation_ConfirmButtonPress;
        public InputAction @IndexUp => m_Wrapper.m_MenuNavigation_IndexUp;
        public InputAction @IndexDown => m_Wrapper.m_MenuNavigation_IndexDown;
        public InputAction @ReturnButtonPress => m_Wrapper.m_MenuNavigation_ReturnButtonPress;
        public InputAction @SliderIncrease => m_Wrapper.m_MenuNavigation_SliderIncrease;
        public InputAction @SliderDecrease => m_Wrapper.m_MenuNavigation_SliderDecrease;
        public InputAction @Pause => m_Wrapper.m_MenuNavigation_Pause;
        public InputActionMap Get() { return m_Wrapper.m_MenuNavigation; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuNavigationActions set) { return set.Get(); }
        public void SetCallbacks(IMenuNavigationActions instance)
        {
            if (m_Wrapper.m_MenuNavigationActionsCallbackInterface != null)
            {
                @ConfirmButtonPress.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnConfirmButtonPress;
                @ConfirmButtonPress.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnConfirmButtonPress;
                @ConfirmButtonPress.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnConfirmButtonPress;
                @IndexUp.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnIndexUp;
                @IndexUp.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnIndexUp;
                @IndexUp.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnIndexUp;
                @IndexDown.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnIndexDown;
                @IndexDown.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnIndexDown;
                @IndexDown.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnIndexDown;
                @ReturnButtonPress.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnReturnButtonPress;
                @ReturnButtonPress.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnReturnButtonPress;
                @ReturnButtonPress.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnReturnButtonPress;
                @SliderIncrease.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnSliderIncrease;
                @SliderIncrease.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnSliderIncrease;
                @SliderIncrease.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnSliderIncrease;
                @SliderDecrease.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnSliderDecrease;
                @SliderDecrease.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnSliderDecrease;
                @SliderDecrease.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnSliderDecrease;
                @Pause.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_MenuNavigationActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ConfirmButtonPress.started += instance.OnConfirmButtonPress;
                @ConfirmButtonPress.performed += instance.OnConfirmButtonPress;
                @ConfirmButtonPress.canceled += instance.OnConfirmButtonPress;
                @IndexUp.started += instance.OnIndexUp;
                @IndexUp.performed += instance.OnIndexUp;
                @IndexUp.canceled += instance.OnIndexUp;
                @IndexDown.started += instance.OnIndexDown;
                @IndexDown.performed += instance.OnIndexDown;
                @IndexDown.canceled += instance.OnIndexDown;
                @ReturnButtonPress.started += instance.OnReturnButtonPress;
                @ReturnButtonPress.performed += instance.OnReturnButtonPress;
                @ReturnButtonPress.canceled += instance.OnReturnButtonPress;
                @SliderIncrease.started += instance.OnSliderIncrease;
                @SliderIncrease.performed += instance.OnSliderIncrease;
                @SliderIncrease.canceled += instance.OnSliderIncrease;
                @SliderDecrease.started += instance.OnSliderDecrease;
                @SliderDecrease.performed += instance.OnSliderDecrease;
                @SliderDecrease.canceled += instance.OnSliderDecrease;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public MenuNavigationActions @MenuNavigation => new MenuNavigationActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_XBoxSchemeIndex = -1;
    public InputControlScheme XBoxScheme
    {
        get
        {
            if (m_XBoxSchemeIndex == -1) m_XBoxSchemeIndex = asset.FindControlSchemeIndex("XBox");
            return asset.controlSchemes[m_XBoxSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnEnterExit(InputAction.CallbackContext context);
        void OnPushPull(InputAction.CallbackContext context);
        void OnLift(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnLook(InputAction.CallbackContext context);
    }
    public interface IMenuNavigationActions
    {
        void OnConfirmButtonPress(InputAction.CallbackContext context);
        void OnIndexUp(InputAction.CallbackContext context);
        void OnIndexDown(InputAction.CallbackContext context);
        void OnReturnButtonPress(InputAction.CallbackContext context);
        void OnSliderIncrease(InputAction.CallbackContext context);
        void OnSliderDecrease(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}

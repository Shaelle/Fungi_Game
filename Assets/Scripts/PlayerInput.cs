// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""ae70915b-c6cb-48e4-9823-7d179d334ecc"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c86d7dfb-2281-41aa-a6c8-9c9f915cb00d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PointerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""81e04e35-93cd-45e2-831d-7bca7010e1f5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ff6ad5db-57af-4374-b2f1-5c22b0255bd5"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": ""Tap(duration=0.4)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff4308c9-f9ea-4833-a5a2-cee902e13833"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_Click = m_Main.FindAction("Click", throwIfNotFound: true);
        m_Main_PointerPosition = m_Main.FindAction("PointerPosition", throwIfNotFound: true);
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

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_Click;
    private readonly InputAction m_Main_PointerPosition;
    public struct MainActions
    {
        private @PlayerInput m_Wrapper;
        public MainActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_Main_Click;
        public InputAction @PointerPosition => m_Wrapper.m_Main_PointerPosition;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_MainActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnClick;
                @PointerPosition.started -= m_Wrapper.m_MainActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnPointerPosition;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @PointerPosition.started += instance.OnPointerPosition;
                @PointerPosition.performed += instance.OnPointerPosition;
                @PointerPosition.canceled += instance.OnPointerPosition;
            }
        }
    }
    public MainActions @Main => new MainActions(this);
    public interface IMainActions
    {
        void OnClick(InputAction.CallbackContext context);
        void OnPointerPosition(InputAction.CallbackContext context);
    }
}

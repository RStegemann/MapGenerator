// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/CameraMovement.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CameraMouse : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraMouse()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraMovement"",
    ""maps"": [
        {
            ""name"": ""CameraMovement"",
            ""id"": ""e0005478-4c64-48c3-9f43-cf6a05b202cb"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""acd80fcf-3e50-458f-818a-90ca104f4de6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""da38f7ab-bb0c-4ba1-bcc8-0a8df78b339a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""Button"",
                    ""id"": ""acde6a3b-a613-4863-a2e0-f9286121afd2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClickReleased"",
                    ""type"": ""Button"",
                    ""id"": ""f6ab8ad7-82b3-4a3d-87e2-f55125571620"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""MouseScroll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""031d5711-0646-40e6-9987-5f9069c865cc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fb17ff35-d17e-4544-8c0c-694b80169b49"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f84d5a62-ae3d-4530-88fc-6d7c1c9b30a3"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40b452c1-0579-41f0-8bb9-14c78751fb48"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c09dfd8-60f6-4bf7-b118-16fc0a5a60f7"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClickReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30851fe9-7006-40ae-a189-b777441e72cc"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraMovement
        m_CameraMovement = asset.FindActionMap("CameraMovement", throwIfNotFound: true);
        m_CameraMovement_Position = m_CameraMovement.FindAction("Position", throwIfNotFound: true);
        m_CameraMovement_RightClick = m_CameraMovement.FindAction("RightClick", throwIfNotFound: true);
        m_CameraMovement_MiddleClick = m_CameraMovement.FindAction("MiddleClick", throwIfNotFound: true);
        m_CameraMovement_RightClickReleased = m_CameraMovement.FindAction("RightClickReleased", throwIfNotFound: true);
        m_CameraMovement_MouseScroll = m_CameraMovement.FindAction("MouseScroll", throwIfNotFound: true);
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

    // CameraMovement
    private readonly InputActionMap m_CameraMovement;
    private ICameraMovementActions m_CameraMovementActionsCallbackInterface;
    private readonly InputAction m_CameraMovement_Position;
    private readonly InputAction m_CameraMovement_RightClick;
    private readonly InputAction m_CameraMovement_MiddleClick;
    private readonly InputAction m_CameraMovement_RightClickReleased;
    private readonly InputAction m_CameraMovement_MouseScroll;
    public struct CameraMovementActions
    {
        private @CameraMouse m_Wrapper;
        public CameraMovementActions(@CameraMouse wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_CameraMovement_Position;
        public InputAction @RightClick => m_Wrapper.m_CameraMovement_RightClick;
        public InputAction @MiddleClick => m_Wrapper.m_CameraMovement_MiddleClick;
        public InputAction @RightClickReleased => m_Wrapper.m_CameraMovement_RightClickReleased;
        public InputAction @MouseScroll => m_Wrapper.m_CameraMovement_MouseScroll;
        public InputActionMap Get() { return m_Wrapper.m_CameraMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraMovementActions set) { return set.Get(); }
        public void SetCallbacks(ICameraMovementActions instance)
        {
            if (m_Wrapper.m_CameraMovementActionsCallbackInterface != null)
            {
                @Position.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnPosition;
                @Position.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnPosition;
                @Position.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnPosition;
                @RightClick.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnRightClick;
                @MiddleClick.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMiddleClick;
                @RightClickReleased.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnRightClickReleased;
                @RightClickReleased.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnRightClickReleased;
                @RightClickReleased.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnRightClickReleased;
                @MouseScroll.started -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMouseScroll;
                @MouseScroll.performed -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMouseScroll;
                @MouseScroll.canceled -= m_Wrapper.m_CameraMovementActionsCallbackInterface.OnMouseScroll;
            }
            m_Wrapper.m_CameraMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Position.started += instance.OnPosition;
                @Position.performed += instance.OnPosition;
                @Position.canceled += instance.OnPosition;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @RightClickReleased.started += instance.OnRightClickReleased;
                @RightClickReleased.performed += instance.OnRightClickReleased;
                @RightClickReleased.canceled += instance.OnRightClickReleased;
                @MouseScroll.started += instance.OnMouseScroll;
                @MouseScroll.performed += instance.OnMouseScroll;
                @MouseScroll.canceled += instance.OnMouseScroll;
            }
        }
    }
    public CameraMovementActions @CameraMovement => new CameraMovementActions(this);
    public interface ICameraMovementActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClickReleased(InputAction.CallbackContext context);
        void OnMouseScroll(InputAction.CallbackContext context);
    }
}

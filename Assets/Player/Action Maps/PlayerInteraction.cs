// GENERATED AUTOMATICALLY FROM 'Assets/Prefabs/Player/Action Maps/PlayerInteraction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInteraction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInteraction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInteraction"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""d23a5767-8cb8-409f-8122-30bd3c29ee45"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Value"",
                    ""id"": ""cec2fc03-556e-42d3-a57d-cd1cec836c4b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryAttack"",
                    ""type"": ""Value"",
                    ""id"": ""6cb801d8-4b34-4db1-ae72-ea3b912ddf5c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Value"",
                    ""id"": ""10b62436-ea70-4ce2-8d9d-17a96e880454"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""91314646-495f-4288-b8e0-dcd7dccc0ee7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Selection"",
                    ""type"": ""Value"",
                    ""id"": ""bb09b4ca-f270-4c0b-929e-8b2e58275621"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse Position"",
                    ""type"": ""Value"",
                    ""id"": ""c48c7a71-024d-40c4-b9b7-8f39253653f4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shift"",
                    ""type"": ""Value"",
                    ""id"": ""28a70dd3-ae06-4c75-9faa-f3b6ccf2470e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""73d4bb9e-2dfa-4a42-b688-9f00050e6e12"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d91c2197-a1ba-4d10-85e1-4511a256ca75"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7083588-7502-404a-be99-5e40ca665862"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38c188f9-5002-4480-94fb-e5f9e8487626"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Selection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ba656f9-86a6-4310-ad2f-b8941a587f68"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ddcee5ef-0091-41ea-9467-4be28a7068f9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9fcb1c4-ca4c-4388-977a-1f7905b53275"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_SecondaryAttack = m_Player.FindAction("SecondaryAttack", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Inventory = m_Player.FindAction("Inventory", throwIfNotFound: true);
        m_Player_Selection = m_Player.FindAction("Selection", throwIfNotFound: true);
        m_Player_MousePosition = m_Player.FindAction("Mouse Position", throwIfNotFound: true);
        m_Player_Shift = m_Player.FindAction("Shift", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_SecondaryAttack;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Inventory;
    private readonly InputAction m_Player_Selection;
    private readonly InputAction m_Player_MousePosition;
    private readonly InputAction m_Player_Shift;
    public struct PlayerActions
    {
        private @PlayerInteraction m_Wrapper;
        public PlayerActions(@PlayerInteraction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @SecondaryAttack => m_Wrapper.m_Player_SecondaryAttack;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Inventory => m_Wrapper.m_Player_Inventory;
        public InputAction @Selection => m_Wrapper.m_Player_Selection;
        public InputAction @MousePosition => m_Wrapper.m_Player_MousePosition;
        public InputAction @Shift => m_Wrapper.m_Player_Shift;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @SecondaryAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSecondaryAttack;
                @SecondaryAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSecondaryAttack;
                @SecondaryAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSecondaryAttack;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Inventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventory;
                @Selection.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelection;
                @Selection.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelection;
                @Selection.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelection;
                @MousePosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @Shift.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShift;
                @Shift.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShift;
                @Shift.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShift;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @SecondaryAttack.started += instance.OnSecondaryAttack;
                @SecondaryAttack.performed += instance.OnSecondaryAttack;
                @SecondaryAttack.canceled += instance.OnSecondaryAttack;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @Selection.started += instance.OnSelection;
                @Selection.performed += instance.OnSelection;
                @Selection.canceled += instance.OnSelection;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @Shift.started += instance.OnShift;
                @Shift.performed += instance.OnShift;
                @Shift.canceled += instance.OnShift;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnSecondaryAttack(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnSelection(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnShift(InputAction.CallbackContext context);
    }
}

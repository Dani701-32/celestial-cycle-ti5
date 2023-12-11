//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Actions/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""c9c21fde-d378-4757-bd30-154dd62fbfa5"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""95acd9a2-a200-48a5-8e32-c10ccc90ad8b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""120e7e16-32ad-427c-9b57-ab7345c90416"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""3505dbb2-5a2f-443e-b35d-47a07e8a438a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DrawWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""d2eb6bdb-5278-481c-aad0-93463c7bcd1d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""0c0dd3eb-40f2-4b87-9e5c-f8c67e8bddcd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""09aa08d1-d116-4c35-a81e-523857517802"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""6717c99c-9d5a-4892-890b-5a1900d1f8ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Artifact"",
                    ""type"": ""Button"",
                    ""id"": ""80eca5f7-a957-4334-a71b-6a1f2b37ca10"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FirstArtifact"",
                    ""type"": ""Button"",
                    ""id"": ""09a9bb87-d673-4e86-a8ee-ccb2df31299f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondArtifact"",
                    ""type"": ""Button"",
                    ""id"": ""9d0fd47a-c4dc-42f8-a0fb-c919d9fda7df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ThirdArtifact"",
                    ""type"": ""Button"",
                    ""id"": ""0ffa167d-29e5-425d-8886-4b002a2a3ff3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FourthArtifact"",
                    ""type"": ""Button"",
                    ""id"": ""ed91f42e-0ff6-4782-9146-4711fe109eae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""35d521bd-ad26-4c4f-8027-4c2f16d789e6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2588d4cb-7e65-4e19-bf71-4ec155f8aed6"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""12cdf63a-4426-4305-a488-89eb0127c357"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d2a3866c-e369-499d-ae50-fd9aead98ab7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""816d23e0-b7ac-4680-a9ee-58cb132f5554"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d376b8d3-1260-4ce6-aa6b-ed256b79e735"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""087c1ed4-3769-4a10-af34-a70abbd26dfc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8482c6a4-2eae-45fd-b5cb-9927983fc901"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DrawWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0df46e89-77ae-4c1f-b385-53cd138dd989"",
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
                    ""id"": ""f72b1b34-443d-4b21-9ff3-f1b33dec7242"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""233a9ba1-585e-4f6b-8510-4ea8edb5964d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Artifact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd8674ea-6f11-4e81-806c-206847549e96"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec4253b5-3f12-42a3-88ad-67f34ed314b5"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FirstArtifact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c435c676-d0a4-45b9-96f8-b85f0ac86b0e"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondArtifact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab255e3e-1f14-4309-88ec-be9c9ce77fcd"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThirdArtifact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e70fd8e-c366-4c34-ae23-a97628e977ad"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FourthArtifact"",
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
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_DrawWeapon = m_Player.FindAction("DrawWeapon", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Inventory = m_Player.FindAction("Inventory", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Artifact = m_Player.FindAction("Artifact", throwIfNotFound: true);
        m_Player_FirstArtifact = m_Player.FindAction("FirstArtifact", throwIfNotFound: true);
        m_Player_SecondArtifact = m_Player.FindAction("SecondArtifact", throwIfNotFound: true);
        m_Player_ThirdArtifact = m_Player.FindAction("ThirdArtifact", throwIfNotFound: true);
        m_Player_FourthArtifact = m_Player.FindAction("FourthArtifact", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Run;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_DrawWeapon;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Inventory;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Artifact;
    private readonly InputAction m_Player_FirstArtifact;
    private readonly InputAction m_Player_SecondArtifact;
    private readonly InputAction m_Player_ThirdArtifact;
    private readonly InputAction m_Player_FourthArtifact;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @DrawWeapon => m_Wrapper.m_Player_DrawWeapon;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Inventory => m_Wrapper.m_Player_Inventory;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Artifact => m_Wrapper.m_Player_Artifact;
        public InputAction @FirstArtifact => m_Wrapper.m_Player_FirstArtifact;
        public InputAction @SecondArtifact => m_Wrapper.m_Player_SecondArtifact;
        public InputAction @ThirdArtifact => m_Wrapper.m_Player_ThirdArtifact;
        public InputAction @FourthArtifact => m_Wrapper.m_Player_FourthArtifact;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @DrawWeapon.started += instance.OnDrawWeapon;
            @DrawWeapon.performed += instance.OnDrawWeapon;
            @DrawWeapon.canceled += instance.OnDrawWeapon;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @Inventory.started += instance.OnInventory;
            @Inventory.performed += instance.OnInventory;
            @Inventory.canceled += instance.OnInventory;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Artifact.started += instance.OnArtifact;
            @Artifact.performed += instance.OnArtifact;
            @Artifact.canceled += instance.OnArtifact;
            @FirstArtifact.started += instance.OnFirstArtifact;
            @FirstArtifact.performed += instance.OnFirstArtifact;
            @FirstArtifact.canceled += instance.OnFirstArtifact;
            @SecondArtifact.started += instance.OnSecondArtifact;
            @SecondArtifact.performed += instance.OnSecondArtifact;
            @SecondArtifact.canceled += instance.OnSecondArtifact;
            @ThirdArtifact.started += instance.OnThirdArtifact;
            @ThirdArtifact.performed += instance.OnThirdArtifact;
            @ThirdArtifact.canceled += instance.OnThirdArtifact;
            @FourthArtifact.started += instance.OnFourthArtifact;
            @FourthArtifact.performed += instance.OnFourthArtifact;
            @FourthArtifact.canceled += instance.OnFourthArtifact;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @DrawWeapon.started -= instance.OnDrawWeapon;
            @DrawWeapon.performed -= instance.OnDrawWeapon;
            @DrawWeapon.canceled -= instance.OnDrawWeapon;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @Inventory.started -= instance.OnInventory;
            @Inventory.performed -= instance.OnInventory;
            @Inventory.canceled -= instance.OnInventory;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Artifact.started -= instance.OnArtifact;
            @Artifact.performed -= instance.OnArtifact;
            @Artifact.canceled -= instance.OnArtifact;
            @FirstArtifact.started -= instance.OnFirstArtifact;
            @FirstArtifact.performed -= instance.OnFirstArtifact;
            @FirstArtifact.canceled -= instance.OnFirstArtifact;
            @SecondArtifact.started -= instance.OnSecondArtifact;
            @SecondArtifact.performed -= instance.OnSecondArtifact;
            @SecondArtifact.canceled -= instance.OnSecondArtifact;
            @ThirdArtifact.started -= instance.OnThirdArtifact;
            @ThirdArtifact.performed -= instance.OnThirdArtifact;
            @ThirdArtifact.canceled -= instance.OnThirdArtifact;
            @FourthArtifact.started -= instance.OnFourthArtifact;
            @FourthArtifact.performed -= instance.OnFourthArtifact;
            @FourthArtifact.canceled -= instance.OnFourthArtifact;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDrawWeapon(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnArtifact(InputAction.CallbackContext context);
        void OnFirstArtifact(InputAction.CallbackContext context);
        void OnSecondArtifact(InputAction.CallbackContext context);
        void OnThirdArtifact(InputAction.CallbackContext context);
        void OnFourthArtifact(InputAction.CallbackContext context);
    }
}

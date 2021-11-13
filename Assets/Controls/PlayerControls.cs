// GENERATED AUTOMATICALLY FROM 'Assets/Controls/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""e64b0886-7ea6-4c08-b4e7-74f8e73e64cd"",
            ""actions"": [
                {
                    ""name"": ""Horizontal"",
                    ""type"": ""Value"",
                    ""id"": ""9218b739-fde7-4a03-8e80-892a166f6ff5"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Vertical"",
                    ""type"": ""Value"",
                    ""id"": ""cf4f27bb-990e-4c65-b356-bac4f59b52a5"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""f98d33ea-fe5a-4022-9f1a-9b270affa73f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""c3bc302e-97c5-46c4-8e61-3cb28aab32e2"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""4b125e99-ed1b-446e-9b9b-9bc2555ee657"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b9eb05e3-66ae-4c1d-9e5e-60ae4e10e92a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""0fd68947-5e7a-4205-9d77-cbc2c2ca9be3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e41ca6d4-2cbd-44a1-b7af-fb975143061c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""375981fc-f12b-4a7b-a1fc-6a9f6f4ab92f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a5943539-fd9c-4ef8-a7ea-6bcacb01422c"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Interact"",
            ""id"": ""0e5f0192-1211-4676-8110-b7989f6031ef"",
            ""actions"": [
                {
                    ""name"": ""Press"",
                    ""type"": ""Button"",
                    ""id"": ""07d886bd-b0cd-4932-9d3b-5a93171c76ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""61d53972-021b-4881-bbf8-7a7aa19a7558"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rope"",
                    ""type"": ""Button"",
                    ""id"": ""6c8f2d3e-9378-4448-bebc-895dd065a7ca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Place"",
                    ""type"": ""Button"",
                    ""id"": ""63382f7e-8dcc-482b-aa65-b12d5ef957b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Destroy"",
                    ""type"": ""Button"",
                    ""id"": ""c306d2fb-3acf-4ebc-84b5-53eeb3ad2e50"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""822fa2e9-f70b-4157-a81c-dce75af4c90d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchItem"",
                    ""type"": ""Value"",
                    ""id"": ""b4babdbc-91f3-4071-a986-03ba37b468b3"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Market"",
                    ""type"": ""Button"",
                    ""id"": ""e05a60fe-0904-4fe0-8bad-e67295f7fe17"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""d8b34432-c756-41e5-b320-8dd96dae9b1f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""29d77b76-e94e-400f-afbc-715f8c0e509e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fe996f0f-076d-43bf-961c-b7bfbd3d9012"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6c9d708-336e-485d-8d85-6f07693d7122"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c63b114-c346-4f3b-aa77-6b0205785f2d"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rope"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ed6dcca-1299-44c6-b312-c82b069eda19"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Place"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9df8555b-9bc0-4b52-ae38-2e80abeb0cc2"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Destroy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26b252fb-7b89-4ad7-abfb-6b7512e6b8dc"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""cff72537-64ff-4a3d-84d8-bfebda200125"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchItem"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""6286146e-6e4c-448e-99f1-f6b6f3719251"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ef1d0fef-69ca-43af-befc-67ff5ac5ae3b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""63f604e2-d818-4cb8-90e1-7867972ba496"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Market"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d6f8dfa-d86c-4efc-ac09-7310f8bded53"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a10967a-ad71-413d-b300-efd2d291169a"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Inventory"",
            ""id"": ""912660df-12d0-4d56-8faa-e0ecf30ece82"",
            ""actions"": [
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""169ede6b-29a6-4d9d-ad18-de352bc55f81"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ItemRotator"",
                    ""type"": ""Button"",
                    ""id"": ""fd240d7b-fa2d-47a2-a3ff-62f7456dc74b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f9482c31-5340-4e89-8582-fc244113a627"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""381236e5-2ba2-4ba1-b5fe-df29bc7ce4e8"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemRotator"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Fight"",
            ""id"": ""b3148735-daeb-4d29-8187-3adfff16f6be"",
            ""actions"": [
                {
                    ""name"": ""Magic"",
                    ""type"": ""Button"",
                    ""id"": ""cbc351c4-ae02-4616-896c-6d18029af207"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MagicMenu"",
                    ""type"": ""Button"",
                    ""id"": ""a6376774-f7eb-44d0-921d-d3ef645ea786"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AdvanceAttack"",
                    ""type"": ""Button"",
                    ""id"": ""b29dbf48-e4ba-4a4d-847c-63a5ec3b5aa6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""248ccd2a-4420-4f5b-afcb-d09fd63dc429"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Magic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d66f28c-4c67-41bb-bdba-4b8a272be281"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Magic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eb739562-b475-40c1-9f0e-691e0396cb23"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Magic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""158b54d8-85f8-4122-8cf6-e54a0cab93d3"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Magic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83c13685-ee51-4082-87b4-4b6e8c337ac4"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Magic"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c01fc125-0fd8-46f4-aac2-38eb2cf0e0f8"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MagicMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03097948-2c78-4277-97db-73e66ea713e9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AdvanceAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Dialog"",
            ""id"": ""1dab01ba-649d-4ef0-810a-744c9a86746c"",
            ""actions"": [
                {
                    ""name"": ""Change Option"",
                    ""type"": ""Value"",
                    ""id"": ""eb45456e-557b-43f5-b0dc-82ae1936ab87"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Choose Line"",
                    ""type"": ""Button"",
                    ""id"": ""67b93504-d20f-43f3-8c03-93f83f8170c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""67d1d2fa-ccef-43d4-a035-7a40ee35a745"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change Option"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""2d387e99-069a-47df-ac71-63a7a6bb60a3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change Option"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b58f9ee5-3e6b-4727-ab56-753f596c4f61"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change Option"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""167c19b9-13fc-4358-b450-9b1ee535d17b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Choose Line"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Horizontal = m_Movement.FindAction("Horizontal", throwIfNotFound: true);
        m_Movement_Vertical = m_Movement.FindAction("Vertical", throwIfNotFound: true);
        m_Movement_MousePosition = m_Movement.FindAction("MousePosition", throwIfNotFound: true);
        // Interact
        m_Interact = asset.FindActionMap("Interact", throwIfNotFound: true);
        m_Interact_Press = m_Interact.FindAction("Press", throwIfNotFound: true);
        m_Interact_Cancel = m_Interact.FindAction("Cancel", throwIfNotFound: true);
        m_Interact_Rope = m_Interact.FindAction("Rope", throwIfNotFound: true);
        m_Interact_Place = m_Interact.FindAction("Place", throwIfNotFound: true);
        m_Interact_Destroy = m_Interact.FindAction("Destroy", throwIfNotFound: true);
        m_Interact_Inventory = m_Interact.FindAction("Inventory", throwIfNotFound: true);
        m_Interact_SwitchItem = m_Interact.FindAction("SwitchItem", throwIfNotFound: true);
        m_Interact_Market = m_Interact.FindAction("Market", throwIfNotFound: true);
        m_Interact_Enter = m_Interact.FindAction("Enter", throwIfNotFound: true);
        m_Interact_Menu = m_Interact.FindAction("Menu", throwIfNotFound: true);
        // Inventory
        m_Inventory = asset.FindActionMap("Inventory", throwIfNotFound: true);
        m_Inventory_MousePosition = m_Inventory.FindAction("MousePosition", throwIfNotFound: true);
        m_Inventory_ItemRotator = m_Inventory.FindAction("ItemRotator", throwIfNotFound: true);
        // Fight
        m_Fight = asset.FindActionMap("Fight", throwIfNotFound: true);
        m_Fight_Magic = m_Fight.FindAction("Magic", throwIfNotFound: true);
        m_Fight_MagicMenu = m_Fight.FindAction("MagicMenu", throwIfNotFound: true);
        m_Fight_AdvanceAttack = m_Fight.FindAction("AdvanceAttack", throwIfNotFound: true);
        // Dialog
        m_Dialog = asset.FindActionMap("Dialog", throwIfNotFound: true);
        m_Dialog_ChangeOption = m_Dialog.FindAction("Change Option", throwIfNotFound: true);
        m_Dialog_ChooseLine = m_Dialog.FindAction("Choose Line", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Horizontal;
    private readonly InputAction m_Movement_Vertical;
    private readonly InputAction m_Movement_MousePosition;
    public struct MovementActions
    {
        private @PlayerControls m_Wrapper;
        public MovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Horizontal => m_Wrapper.m_Movement_Horizontal;
        public InputAction @Vertical => m_Wrapper.m_Movement_Vertical;
        public InputAction @MousePosition => m_Wrapper.m_Movement_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Horizontal.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnHorizontal;
                @Horizontal.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnHorizontal;
                @Horizontal.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnHorizontal;
                @Vertical.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnVertical;
                @Vertical.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnVertical;
                @Vertical.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnVertical;
                @MousePosition.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMousePosition;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Horizontal.started += instance.OnHorizontal;
                @Horizontal.performed += instance.OnHorizontal;
                @Horizontal.canceled += instance.OnHorizontal;
                @Vertical.started += instance.OnVertical;
                @Vertical.performed += instance.OnVertical;
                @Vertical.canceled += instance.OnVertical;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Interact
    private readonly InputActionMap m_Interact;
    private IInteractActions m_InteractActionsCallbackInterface;
    private readonly InputAction m_Interact_Press;
    private readonly InputAction m_Interact_Cancel;
    private readonly InputAction m_Interact_Rope;
    private readonly InputAction m_Interact_Place;
    private readonly InputAction m_Interact_Destroy;
    private readonly InputAction m_Interact_Inventory;
    private readonly InputAction m_Interact_SwitchItem;
    private readonly InputAction m_Interact_Market;
    private readonly InputAction m_Interact_Enter;
    private readonly InputAction m_Interact_Menu;
    public struct InteractActions
    {
        private @PlayerControls m_Wrapper;
        public InteractActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Press => m_Wrapper.m_Interact_Press;
        public InputAction @Cancel => m_Wrapper.m_Interact_Cancel;
        public InputAction @Rope => m_Wrapper.m_Interact_Rope;
        public InputAction @Place => m_Wrapper.m_Interact_Place;
        public InputAction @Destroy => m_Wrapper.m_Interact_Destroy;
        public InputAction @Inventory => m_Wrapper.m_Interact_Inventory;
        public InputAction @SwitchItem => m_Wrapper.m_Interact_SwitchItem;
        public InputAction @Market => m_Wrapper.m_Interact_Market;
        public InputAction @Enter => m_Wrapper.m_Interact_Enter;
        public InputAction @Menu => m_Wrapper.m_Interact_Menu;
        public InputActionMap Get() { return m_Wrapper.m_Interact; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InteractActions set) { return set.Get(); }
        public void SetCallbacks(IInteractActions instance)
        {
            if (m_Wrapper.m_InteractActionsCallbackInterface != null)
            {
                @Press.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnPress;
                @Press.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnPress;
                @Press.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnPress;
                @Cancel.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnCancel;
                @Rope.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnRope;
                @Rope.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnRope;
                @Rope.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnRope;
                @Place.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnPlace;
                @Place.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnPlace;
                @Place.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnPlace;
                @Destroy.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnDestroy;
                @Destroy.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnDestroy;
                @Destroy.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnDestroy;
                @Inventory.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnInventory;
                @SwitchItem.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnSwitchItem;
                @SwitchItem.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnSwitchItem;
                @SwitchItem.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnSwitchItem;
                @Market.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnMarket;
                @Market.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnMarket;
                @Market.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnMarket;
                @Enter.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnEnter;
                @Menu.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnMenu;
            }
            m_Wrapper.m_InteractActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Press.started += instance.OnPress;
                @Press.performed += instance.OnPress;
                @Press.canceled += instance.OnPress;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Rope.started += instance.OnRope;
                @Rope.performed += instance.OnRope;
                @Rope.canceled += instance.OnRope;
                @Place.started += instance.OnPlace;
                @Place.performed += instance.OnPlace;
                @Place.canceled += instance.OnPlace;
                @Destroy.started += instance.OnDestroy;
                @Destroy.performed += instance.OnDestroy;
                @Destroy.canceled += instance.OnDestroy;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @SwitchItem.started += instance.OnSwitchItem;
                @SwitchItem.performed += instance.OnSwitchItem;
                @SwitchItem.canceled += instance.OnSwitchItem;
                @Market.started += instance.OnMarket;
                @Market.performed += instance.OnMarket;
                @Market.canceled += instance.OnMarket;
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
            }
        }
    }
    public InteractActions @Interact => new InteractActions(this);

    // Inventory
    private readonly InputActionMap m_Inventory;
    private IInventoryActions m_InventoryActionsCallbackInterface;
    private readonly InputAction m_Inventory_MousePosition;
    private readonly InputAction m_Inventory_ItemRotator;
    public struct InventoryActions
    {
        private @PlayerControls m_Wrapper;
        public InventoryActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_Inventory_MousePosition;
        public InputAction @ItemRotator => m_Wrapper.m_Inventory_ItemRotator;
        public InputActionMap Get() { return m_Wrapper.m_Inventory; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryActions set) { return set.Get(); }
        public void SetCallbacks(IInventoryActions instance)
        {
            if (m_Wrapper.m_InventoryActionsCallbackInterface != null)
            {
                @MousePosition.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnMousePosition;
                @ItemRotator.started -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItemRotator;
                @ItemRotator.performed -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItemRotator;
                @ItemRotator.canceled -= m_Wrapper.m_InventoryActionsCallbackInterface.OnItemRotator;
            }
            m_Wrapper.m_InventoryActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @ItemRotator.started += instance.OnItemRotator;
                @ItemRotator.performed += instance.OnItemRotator;
                @ItemRotator.canceled += instance.OnItemRotator;
            }
        }
    }
    public InventoryActions @Inventory => new InventoryActions(this);

    // Fight
    private readonly InputActionMap m_Fight;
    private IFightActions m_FightActionsCallbackInterface;
    private readonly InputAction m_Fight_Magic;
    private readonly InputAction m_Fight_MagicMenu;
    private readonly InputAction m_Fight_AdvanceAttack;
    public struct FightActions
    {
        private @PlayerControls m_Wrapper;
        public FightActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Magic => m_Wrapper.m_Fight_Magic;
        public InputAction @MagicMenu => m_Wrapper.m_Fight_MagicMenu;
        public InputAction @AdvanceAttack => m_Wrapper.m_Fight_AdvanceAttack;
        public InputActionMap Get() { return m_Wrapper.m_Fight; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FightActions set) { return set.Get(); }
        public void SetCallbacks(IFightActions instance)
        {
            if (m_Wrapper.m_FightActionsCallbackInterface != null)
            {
                @Magic.started -= m_Wrapper.m_FightActionsCallbackInterface.OnMagic;
                @Magic.performed -= m_Wrapper.m_FightActionsCallbackInterface.OnMagic;
                @Magic.canceled -= m_Wrapper.m_FightActionsCallbackInterface.OnMagic;
                @MagicMenu.started -= m_Wrapper.m_FightActionsCallbackInterface.OnMagicMenu;
                @MagicMenu.performed -= m_Wrapper.m_FightActionsCallbackInterface.OnMagicMenu;
                @MagicMenu.canceled -= m_Wrapper.m_FightActionsCallbackInterface.OnMagicMenu;
                @AdvanceAttack.started -= m_Wrapper.m_FightActionsCallbackInterface.OnAdvanceAttack;
                @AdvanceAttack.performed -= m_Wrapper.m_FightActionsCallbackInterface.OnAdvanceAttack;
                @AdvanceAttack.canceled -= m_Wrapper.m_FightActionsCallbackInterface.OnAdvanceAttack;
            }
            m_Wrapper.m_FightActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Magic.started += instance.OnMagic;
                @Magic.performed += instance.OnMagic;
                @Magic.canceled += instance.OnMagic;
                @MagicMenu.started += instance.OnMagicMenu;
                @MagicMenu.performed += instance.OnMagicMenu;
                @MagicMenu.canceled += instance.OnMagicMenu;
                @AdvanceAttack.started += instance.OnAdvanceAttack;
                @AdvanceAttack.performed += instance.OnAdvanceAttack;
                @AdvanceAttack.canceled += instance.OnAdvanceAttack;
            }
        }
    }
    public FightActions @Fight => new FightActions(this);

    // Dialog
    private readonly InputActionMap m_Dialog;
    private IDialogActions m_DialogActionsCallbackInterface;
    private readonly InputAction m_Dialog_ChangeOption;
    private readonly InputAction m_Dialog_ChooseLine;
    public struct DialogActions
    {
        private @PlayerControls m_Wrapper;
        public DialogActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ChangeOption => m_Wrapper.m_Dialog_ChangeOption;
        public InputAction @ChooseLine => m_Wrapper.m_Dialog_ChooseLine;
        public InputActionMap Get() { return m_Wrapper.m_Dialog; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DialogActions set) { return set.Get(); }
        public void SetCallbacks(IDialogActions instance)
        {
            if (m_Wrapper.m_DialogActionsCallbackInterface != null)
            {
                @ChangeOption.started -= m_Wrapper.m_DialogActionsCallbackInterface.OnChangeOption;
                @ChangeOption.performed -= m_Wrapper.m_DialogActionsCallbackInterface.OnChangeOption;
                @ChangeOption.canceled -= m_Wrapper.m_DialogActionsCallbackInterface.OnChangeOption;
                @ChooseLine.started -= m_Wrapper.m_DialogActionsCallbackInterface.OnChooseLine;
                @ChooseLine.performed -= m_Wrapper.m_DialogActionsCallbackInterface.OnChooseLine;
                @ChooseLine.canceled -= m_Wrapper.m_DialogActionsCallbackInterface.OnChooseLine;
            }
            m_Wrapper.m_DialogActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ChangeOption.started += instance.OnChangeOption;
                @ChangeOption.performed += instance.OnChangeOption;
                @ChangeOption.canceled += instance.OnChangeOption;
                @ChooseLine.started += instance.OnChooseLine;
                @ChooseLine.performed += instance.OnChooseLine;
                @ChooseLine.canceled += instance.OnChooseLine;
            }
        }
    }
    public DialogActions @Dialog => new DialogActions(this);
    public interface IMovementActions
    {
        void OnHorizontal(InputAction.CallbackContext context);
        void OnVertical(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
    public interface IInteractActions
    {
        void OnPress(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnRope(InputAction.CallbackContext context);
        void OnPlace(InputAction.CallbackContext context);
        void OnDestroy(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnSwitchItem(InputAction.CallbackContext context);
        void OnMarket(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
    }
    public interface IInventoryActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnItemRotator(InputAction.CallbackContext context);
    }
    public interface IFightActions
    {
        void OnMagic(InputAction.CallbackContext context);
        void OnMagicMenu(InputAction.CallbackContext context);
        void OnAdvanceAttack(InputAction.CallbackContext context);
    }
    public interface IDialogActions
    {
        void OnChangeOption(InputAction.CallbackContext context);
        void OnChooseLine(InputAction.CallbackContext context);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Strungerhulder.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IDialoguesActions, GameInput.IMenusActions
    {
        // Assign delegate{} to events to initialise them with an empty delegate
        // so we can skip the null check when we use them

        // Gameplay
        #region Event Delegates
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction MoveToPointEvent = delegate { };
        public event UnityAction MoveToPointCanceledEvent = delegate { };

        public event UnityAction StartedRunning = delegate { };
        public event UnityAction StoppedRunning = delegate { };

        public event UnityAction JumpEvent = delegate { };
        public event UnityAction JumpCanceledEvent = delegate { };

        public event UnityAction AttackEvent = delegate { };
        public event UnityAction AttackCanceledEvent = delegate { };

        public event UnityAction InteractEvent = delegate { }; // Used to talk, pickup objects, interact with tools like the cooking cauldron
        public event UnityAction InteractCanceledEvent = delegate { };
        public event UnityAction InventoryActionButtonEvent = delegate { };
        #endregion

        // Shared between menus and dialogues
        public event UnityAction MoveSelectionEvent = delegate { };

        // Dialogues
        public event UnityAction AdvanceDialogueEvent = delegate { };

        // Menus
        public event UnityAction MenuMouseMoveEvent = delegate { };

        public event UnityAction MenuClickButtonEvent = delegate { };
        public event UnityAction MenuUnpauseEvent = delegate { };
        public event UnityAction MenuPauseEvent = delegate { };
        public event UnityAction MenuCloseEvent = delegate { };
        public event UnityAction OpenInventoryEvent = delegate { }; // Used to bring up the inventory
        public event UnityAction CloseInventoryEvent = delegate { }; // Used to bring up the inventory

        public event UnityAction<float> TabSwitched = delegate { };

        public event UnityAction<float> RotateCamera = delegate { };
        public event UnityAction<float> ZoomCamera = delegate { };


        private GameInput m_GameInput;


        private void OnEnable()
        {
            if (m_GameInput == null)
            {
                m_GameInput = new GameInput();
                m_GameInput.Menus.SetCallbacks(this);
                m_GameInput.Gameplay.SetCallbacks(this);
                m_GameInput.Dialogues.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    AttackEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    AttackCanceledEvent.Invoke();
                    break;
            }
        }

        public void OnOpenInventory(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                OpenInventoryEvent.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuCloseEvent.Invoke();
        }

        public void OnInventoryActionButton(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                InventoryActionButtonEvent.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                InteractEvent.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                InteractCanceledEvent.Invoke();
        }


        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                JumpEvent.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                JumpCanceledEvent.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context) => MoveEvent.Invoke(context.ReadValue<Vector2>());

        public void OnMoveToPoint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MoveToPointEvent.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                MoveToPointCanceledEvent.Invoke();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                StartedRunning.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                StoppedRunning.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuPauseEvent.Invoke();
        }

        public void OnMoveSelection(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MoveSelectionEvent.Invoke();
        }

        public void OnAdvanceDialogue(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                AdvanceDialogueEvent.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuClickButtonEvent.Invoke();
        }


        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuMouseMoveEvent.Invoke();
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuUnpauseEvent.Invoke();
        }

        public void EnableDialogueInput()
        {
            m_GameInput.Menus.Enable();
            m_GameInput.Gameplay.Disable();

            m_GameInput.Dialogues.Enable();
        }

        public void EnableGameplayInput()
        {
            m_GameInput.Menus.Disable();
            m_GameInput.Dialogues.Disable();

            m_GameInput.Gameplay.Enable();
        }

        public void EnableMenuInput()
        {
            m_GameInput.Dialogues.Disable();
            m_GameInput.Gameplay.Disable();

            m_GameInput.Menus.Enable();
        }

        public void DisableAllInput()
        {
            m_GameInput.Gameplay.Disable();
            m_GameInput.Menus.Disable();
            m_GameInput.Dialogues.Disable();
        }

        public void OnChangeTab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                TabSwitched.Invoke(context.ReadValue<float>());
        }

        public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
        public bool RightMouseDown() => Mouse.current.rightButton.isPressed;
        public Vector2 MousePosition() => Mouse.current.position.ReadValue();

        public void OnClick(InputAction.CallbackContext context) { }
        public void OnSubmit(InputAction.CallbackContext context) { }
        public void OnPoint(InputAction.CallbackContext context) { }
        public void OnRightClick(InputAction.CallbackContext context) { }
        public void OnNavigate(InputAction.CallbackContext context) { }
        public void OnCloseInventory(InputAction.CallbackContext context) => CloseInventoryEvent.Invoke();

        public void OnCameraRotate(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                RotateCamera.Invoke(context.ReadValue<float>());
        }

        public void OnCameraZoom(InputAction.CallbackContext context) => ZoomCamera.Invoke(context.ReadValue<float>());


        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";
    }
}

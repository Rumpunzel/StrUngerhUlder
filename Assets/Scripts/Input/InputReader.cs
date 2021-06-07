﻿using UnityEngine;
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
        public event UnityAction<Vector2> moveEvent = delegate { };
        public event UnityAction moveToPointEvent = delegate { };
        public event UnityAction moveToPointCanceledEvent = delegate { };

        public event UnityAction startedRunning = delegate { };
        public event UnityAction stoppedRunning = delegate { };

        public event UnityAction jumpEvent = delegate { };
        public event UnityAction jumpCanceledEvent = delegate { };

        public event UnityAction attackEvent = delegate { };
        public event UnityAction attackCanceledEvent = delegate { };

        public event UnityAction interactEvent = delegate { }; // Used to talk, pickup objects, interact with tools like the cooking cauldron
        public event UnityAction interactCanceledEvent = delegate { };
        public event UnityAction inventoryActionButtonEvent = delegate { };
        #endregion

        // Shared between menus and dialogues
        public event UnityAction moveSelectionEvent = delegate { };

        // Dialogues
        public event UnityAction advanceDialogueEvent = delegate { };

        // Menus
        public event UnityAction menuMouseMoveEvent = delegate { };

        public event UnityAction menuClickButtonEvent = delegate { };
        public event UnityAction menuUnpauseEvent = delegate { };
        public event UnityAction menuPauseEvent = delegate { };
        public event UnityAction menuCloseEvent = delegate { };
        public event UnityAction openInventoryEvent = delegate { }; // Used to bring up the inventory
        public event UnityAction closeInventoryEvent = delegate { }; // Used to bring up the inventory

        public event UnityAction<float> TabSwitched = delegate { };


        private GameInput gameInput;


        private void OnEnable()
        {
            if (gameInput == null)
            {
                gameInput = new GameInput();
                gameInput.Menus.SetCallbacks(this);
                gameInput.Gameplay.SetCallbacks(this);
                gameInput.Dialogues.SetCallbacks(this);
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
                    attackEvent.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    attackCanceledEvent.Invoke();
                    break;
            }
        }

        public void OnOpenInventory(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                openInventoryEvent.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                menuCloseEvent.Invoke();
        }

        public void OnInventoryActionButton(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                inventoryActionButtonEvent.Invoke();

        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                interactEvent.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                interactCanceledEvent.Invoke();
        }


        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                jumpEvent.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                jumpCanceledEvent.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMoveToPoint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                moveToPointEvent.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                moveToPointCanceledEvent.Invoke();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                startedRunning.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                stoppedRunning.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                menuPauseEvent.Invoke();
        }

        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnMoveSelection(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                moveSelectionEvent.Invoke();
        }

        public void OnAdvanceDialogue(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                advanceDialogueEvent.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                menuClickButtonEvent.Invoke();
        }


        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                menuMouseMoveEvent.Invoke();
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                menuUnpauseEvent.Invoke();
        }

        public void EnableDialogueInput()
        {
            gameInput.Menus.Enable();
            gameInput.Gameplay.Disable();

            gameInput.Dialogues.Enable();
        }

        public void EnableGameplayInput()
        {
            gameInput.Menus.Disable();
            gameInput.Dialogues.Disable();

            gameInput.Gameplay.Enable();
        }

        public void EnableMenuInput()
        {
            gameInput.Dialogues.Disable();
            gameInput.Gameplay.Disable();

            gameInput.Menus.Enable();
        }

        public void DisableAllInput()
        {
            gameInput.Gameplay.Disable();
            gameInput.Menus.Disable();
            gameInput.Dialogues.Disable();
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

        public void OnCloseInventory(InputAction.CallbackContext context) => closeInventoryEvent.Invoke();
    }
}

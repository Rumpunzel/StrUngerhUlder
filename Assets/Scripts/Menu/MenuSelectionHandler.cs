using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Strungerhulder.Input;

namespace Strungerhulder.Menues
{
    public class MenuSelectionHandler : MonoBehaviour
    {
        public GameObject currentSelection;
        public GameObject mouseSelection;

        [SerializeField] private GameObject m_DefaultSelection;
        [Space]
        [SerializeField] private InputReader m_InputReader;


        private void OnEnable()
        {
            m_InputReader.MenuMouseMoveEvent += HandleMoveCursor;
            m_InputReader.MoveSelectionEvent += HandleMoveSelection;

            StartCoroutine(SelectDefault());
        }

        private void OnDisable()
        {
            m_InputReader.MenuMouseMoveEvent -= HandleMoveCursor;
            m_InputReader.MoveSelectionEvent -= HandleMoveSelection;
        }

        // Debug
        /*private void OnGUI()
        {
            GUILayout.Box($"currentSelection: {(currentSelection != null ? currentSelection.name : "null")}");
            GUILayout.Box($"_mouseSelection: {(mouseSelection != null ? mouseSelection.name : "null")}");
        }*/

        private void Update()
        {
            if ((EventSystem.current != null) && (EventSystem.current.currentSelectedGameObject == null) && (currentSelection != null))
                EventSystem.current.SetSelectedGameObject(currentSelection);
        }


        public void UpdateDefault(GameObject newDefault) => m_DefaultSelection = newDefault;

        public void Unselect()
        {
            currentSelection = null;
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void HandleMouseEnter(GameObject UIElement)
        {
            mouseSelection = UIElement;
            EventSystem.current.SetSelectedGameObject(UIElement);
        }

        public void HandleMouseExit(GameObject UIElement)
        {
            if (EventSystem.current.currentSelectedGameObject != UIElement)
                return;

            // keep selecting the last thing the mouse has selected 
            mouseSelection = null;
            EventSystem.current.SetSelectedGameObject(currentSelection);
        }

        /// <summary>
        /// Method interactable UI elements should call on Submit interaction to determine whether to continue or not.
        /// </summary>
        /// <returns></returns>
        public bool AllowsSubmit()
        {
            // if LMB is not down, there is no edge case to handle, allow the event to continue
            return !m_InputReader.LeftMouseDown()
                   // if we know mouse & keyboard are on different elements, do not allow interaction to continue
                   || mouseSelection != null && mouseSelection == currentSelection;
        }

        /// <summary>
        /// Fired by gamepad or keyboard navigation inputs
        /// </summary>
        /// <param name="UIElement"></param>
        public void UpdateSelection(GameObject UIElement)
        {
            if ((UIElement.GetComponent<MultiInputSelectableElement>() != null) || (UIElement.GetComponent<MultiInputButton>() != null))
            {
                mouseSelection = UIElement;
                currentSelection = UIElement;
            }
        }


        /// <summary>
        /// Highlights the default element
        /// </summary>
        private IEnumerator SelectDefault()
        {
            yield return new WaitForSeconds(.03f); // Necessary wait otherwise the highlight won't show up

            if (m_DefaultSelection != null)
                UpdateSelection(m_DefaultSelection);
        }

        /// <summary>
        /// Fired by keyboard and gamepad inputs. Current selected UI element will be the ui Element that was selected
        /// when the event was fired. The _currentSelection is updated later on, after the EventSystem moves to the
        /// desired UI element, the UI element will call into UpdateSelection()
        /// </summary>
        private void HandleMoveSelection()
        {
            Cursor.visible = false;

            // Handle case where no UI element is selected because mouse left selectable bounds
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(currentSelection);
        }

        private void HandleMoveCursor()
        {
            if (mouseSelection != null)
                EventSystem.current.SetSelectedGameObject(mouseSelection);

            Cursor.visible = true;
        }
    }
}

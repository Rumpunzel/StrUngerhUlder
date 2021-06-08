using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using Strungerhulder.Inventories.ScriptableObjects;
using Strungerhulder.Input;

namespace Strungerhulder.UI.Iventory
{
    public class UIInventoryActionButton : MonoBehaviour
    {
        public UnityAction Clicked;

        [SerializeField] private LocalizeStringEvent m_ButtonActionText = default;
        [SerializeField] private Button m_ButtonAction = default;
        [SerializeField] private UIButtonPrompt m_ButtonPromptSetter = default;
        [SerializeField] private InputReader m_InputReader = default;

        private bool m_HasEvent = false;


        public void FillInventoryButton(ItemType itemType, bool isInteractable = true)
        {
            m_ButtonAction.interactable = isInteractable;
            m_ButtonActionText.StringReference = itemType.ActionName;

            bool isKeyboard = true;
            m_ButtonPromptSetter.SetButtonPrompt(isKeyboard);

            if (isInteractable)
            {
                if (m_InputReader != null)
                {
                    m_HasEvent = true;
                    m_InputReader.InventoryActionButtonEvent += ClickActionButton;
                }
            }
            else
            {
                if (m_InputReader != null)
                {
                    if (m_HasEvent)
                        m_InputReader.InventoryActionButtonEvent -= ClickActionButton;
                }
            }
        }

        public void ClickActionButton() => Clicked.Invoke();

        private void OnDisable()
        {
            if (m_InputReader != null)
            {
                if (m_HasEvent)
                    m_InputReader.InventoryActionButtonEvent -= ClickActionButton;
            }
        }
    }
}

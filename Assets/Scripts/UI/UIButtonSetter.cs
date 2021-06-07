using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Strungerhulder.UI
{
    public class UIButtonSetter : MonoBehaviour
    {
        public UnityAction clicked = default;

        [SerializeField] private LocalizeStringEvent m_ButtonText = default;
        [SerializeField] private MultiInputButton m_Button = default;

        private bool m_IsDefaultSelection = false;


        private void OnDisable()
        {
            m_Button.isSelected = false;
            m_IsDefaultSelection = false;
        }


        public void SetButton(bool isSelect)
        {
            m_IsDefaultSelection = isSelect;

            if (isSelect)
                m_Button.UpdateSelected();
        }

        public void SetButton(LocalizedString localizedString, bool isSelected)
        {
            m_ButtonText.StringReference = localizedString;

            if (isSelected)
                SelectButton();
        }

        public void SetButton(string tableEntryReference, bool isSelected)
        {
            m_ButtonText.StringReference.TableEntryReference = tableEntryReference;

            if (isSelected)
                SelectButton();
        }

        public void Click() => clicked.Invoke();


        private void SelectButton() => m_Button.Select();
    }
}

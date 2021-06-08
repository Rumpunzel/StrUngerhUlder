using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UnityEngine.Events;
using Strungerhulder.Input;

namespace Strungerhulder.UI
{
    public enum PopupButtonType
    {
        Confirm,
        Cancel,
        Close,
        DoNothing,
    }

    public enum PopupType
    {
        Quit,
        NewGame,
        BackToMenu,
    }

    public class UIPopup : MonoBehaviour
    {
        public UnityAction<bool> confirmationResponseAction;
        public UnityAction closePopupAction;

        [SerializeField] private LocalizeStringEvent m_TitleText = default;
        [SerializeField] private LocalizeStringEvent m_DescriptionText = default;
        [SerializeField] private Button m_ButtonClose = default;
        [SerializeField] private UIButtonSetter m_PopupButton1 = default;
        [SerializeField] private UIButtonSetter m_PopupButton2 = default;

        [SerializeField] private InputReader m_InputReader = default;

        private PopupType m_ActualType;


        private void OnDisable()
        {
            m_PopupButton2.clicked -= CancelButtonClicked;
            m_PopupButton1.clicked -= ConfirmButtonClicked;
            m_InputReader.MenuCloseEvent -= ClosePopupButtonClicked;
        }

        public void SetPopup(PopupType popupType)
        {
            m_ActualType = popupType;

            bool isConfirmation = false;
            bool hasExitButton = false;

            m_TitleText.StringReference.TableEntryReference = m_ActualType.ToString() + "_Popup_Title";
            m_DescriptionText.StringReference.TableEntryReference = m_ActualType.ToString() + "_Popup_Description";

            string tableEntryReferenceConfirm = PopupButtonType.Confirm + "_" + m_ActualType;
            string tableEntryReferenceCancel = PopupButtonType.Cancel + "_" + m_ActualType;

            switch (m_ActualType)
            {
                case PopupType.NewGame:
                case PopupType.BackToMenu:
                    isConfirmation = true;

                    m_PopupButton1.SetButton(tableEntryReferenceConfirm, true);
                    m_PopupButton2.SetButton(tableEntryReferenceCancel, false);
                    hasExitButton = true;
                    break;

                case PopupType.Quit:
                    isConfirmation = true;
                    m_PopupButton1.SetButton(tableEntryReferenceConfirm, true);
                    m_PopupButton2.SetButton(tableEntryReferenceCancel, false);
                    hasExitButton = false;
                    break;

                default:
                    isConfirmation = false;
                    hasExitButton = false;
                    break;
            }

            if (isConfirmation) // needs two button : Is a decision 
            {
                m_PopupButton1.gameObject.SetActive(true);
                m_PopupButton2.gameObject.SetActive(true);

                m_PopupButton2.clicked += CancelButtonClicked;
                m_PopupButton1.clicked += ConfirmButtonClicked;
            }
            else // needs only one button : Is an information 
            {
                m_PopupButton1.gameObject.SetActive(true);
                m_PopupButton2.gameObject.SetActive(false);

                m_PopupButton1.clicked += ConfirmButtonClicked;
            }

            m_ButtonClose.gameObject.SetActive(hasExitButton);

            if (hasExitButton) // can exit : Has to take the decision or aknowledge the information
                m_InputReader.MenuCloseEvent += ClosePopupButtonClicked;
        }


        public void ClosePopupButtonClicked() => closePopupAction.Invoke();


        private void ConfirmButtonClicked() => confirmationResponseAction.Invoke(true);
        private void CancelButtonClicked() => confirmationResponseAction.Invoke(false);
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Strungerhulder.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        public UnityAction newGameButtonAction;
        public UnityAction continueButtonAction;
        public UnityAction settingsButtonAction;
        public UnityAction creditsButtonAction;
        public UnityAction exitButtonActionon;

        [SerializeField] private Button m_ContinueButton = default;
        [SerializeField] private Button m_NewGameButton = default;


        public void SetMenuScreen(bool hasSaveData)
        {
            m_ContinueButton.interactable = hasSaveData;

            if (hasSaveData)
                m_ContinueButton.Select();
            else
                m_NewGameButton.Select();
        }

        public void NewGameButton() => newGameButtonAction.Invoke();
        public void ContinueButton() => continueButtonAction.Invoke();
        public void SettingsButton() => settingsButtonAction.Invoke();
        public void CreditsButton() => creditsButtonAction.Invoke();
        public void ExitButton() => exitButtonActionon.Invoke();
    }
}

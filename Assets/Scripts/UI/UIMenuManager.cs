using System.Collections;
using UnityEngine;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.SavingAndLoading;
using Strungerhulder.Input;

namespace Strungerhulder.UI
{
    public class UIMenuManager : MonoBehaviour
    {
        [SerializeField] private UIPopup m_PopupPanel = default;
        [SerializeField] private UISettings m_SettingsPanel = default;
        [SerializeField] private UICredits m_CreditsPanel = default;
        [SerializeField] private UIMainMenu m_MainMenuPanel = default;

        [SerializeField] private SaveSystem m_SaveSystem = default;

        [SerializeField] private InputReader m_InputReader = default;


        [Header("Broadcasting on")]
        [SerializeField] private VoidEventChannelSO m_StartNewGameEvent = default;
        [SerializeField] private VoidEventChannelSO m_ContinueGameEvent = default;
        [SerializeField] private VoidEventChannelSO m_OnGameExitEvent = default;


        private bool m_HasSaveData;


        private IEnumerator Start()
        {
            m_InputReader.EnableMenuInput();
            yield return new WaitForSeconds(0.4f); //waiting time for all scenes to be loaded 
            SetMenuScreen();
        }


        public void OpenSettingsScreen()
        {
            m_SettingsPanel.gameObject.SetActive(true);
            m_SettingsPanel.SetSettingsScreen();
            m_SettingsPanel.closed += CloseSettingsScreen;
        }

        public void CloseSettingsScreen()
        {
            m_SettingsPanel.closed -= CloseSettingsScreen;
            m_SettingsPanel.gameObject.SetActive(false);
            m_MainMenuPanel.SetMenuScreen(m_HasSaveData);
        }

        public void OpenCreditsScreen()
        {
            m_CreditsPanel.gameObject.SetActive(true);
            m_CreditsPanel.closeCreditsAction += CloseCreditsScreen;
        }

        public void CloseCreditsScreen()
        {
            m_CreditsPanel.closeCreditsAction -= CloseCreditsScreen;
            m_CreditsPanel.gameObject.SetActive(false);
            m_MainMenuPanel.SetMenuScreen(m_HasSaveData);
        }

        public void ShowExitConfirmationPopup()
        {
            m_PopupPanel.confirmationResponseAction += HideExitConfirmationPopup;
            m_PopupPanel.gameObject.SetActive(true);
            m_PopupPanel.SetPopup(PopupType.Quit);
        }


        private void SetMenuScreen()
        {
            m_HasSaveData = m_SaveSystem.LoadSaveDataFromDisk();
            m_MainMenuPanel.SetMenuScreen(m_HasSaveData);
            m_MainMenuPanel.continueButtonAction += m_ContinueGameEvent.RaiseEvent;
            m_MainMenuPanel.newGameButtonAction += ButtonStartNewGameClicked;
            m_MainMenuPanel.settingsButtonAction += OpenSettingsScreen;
            m_MainMenuPanel.creditsButtonAction += OpenCreditsScreen;
            m_MainMenuPanel.exitButtonActionon += ShowExitConfirmationPopup;
        }

        private void ButtonStartNewGameClicked()
        {
            if (!m_HasSaveData)
                ConfirmStartNewGame();
            else
                ShowStartNewGameConfirmationPopup();
        }

        private void ConfirmStartNewGame() => m_StartNewGameEvent.RaiseEvent();

        private void ShowStartNewGameConfirmationPopup()
        {
            m_PopupPanel.confirmationResponseAction += StartNewGamePopupResponse;
            m_PopupPanel.closePopupAction += HidePopup;

            m_PopupPanel.gameObject.SetActive(true);
            m_PopupPanel.SetPopup(PopupType.NewGame);
        }

        private void StartNewGamePopupResponse(bool startNewGameConfirmed)
        {
            m_PopupPanel.confirmationResponseAction -= StartNewGamePopupResponse;
            m_PopupPanel.closePopupAction -= HidePopup;

            m_PopupPanel.gameObject.SetActive(false);

            if (startNewGameConfirmed)
                ConfirmStartNewGame();
            else
                m_ContinueGameEvent.RaiseEvent();

            m_MainMenuPanel.SetMenuScreen(m_HasSaveData);
        }

        private void HidePopup()
        {
            m_PopupPanel.closePopupAction -= HidePopup;
            m_PopupPanel.gameObject.SetActive(false);
            m_MainMenuPanel.SetMenuScreen(m_HasSaveData);
        }

        private void HideExitConfirmationPopup(bool quitConfirmed)
        {
            m_PopupPanel.confirmationResponseAction -= HideExitConfirmationPopup;
            m_PopupPanel.gameObject.SetActive(false);

            if (quitConfirmed)
            {
                Application.Quit();
                m_OnGameExitEvent.onEventRaised();
            }

            m_MainMenuPanel.SetMenuScreen(m_HasSaveData);
        }

        private void OnDestroy()
        {
            m_PopupPanel.confirmationResponseAction -= HideExitConfirmationPopup;
            m_PopupPanel.confirmationResponseAction -= StartNewGamePopupResponse;
        }
    }
}

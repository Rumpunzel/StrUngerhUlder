using UnityEngine;
using Strungerhulder.UI.Iventory;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.Gameplay.ScriptableObjects;
using Strungerhulder.Interaction;
using Strungerhulder.Inventory.ScriptableObjects;

namespace Strungerhulder.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Scene UI")]
        [SerializeField] private MenuSelectionHandler m_SelectionHandler = default;
        [SerializeField] private UIPopup m_PopupPanel = default;

        [SerializeField] private UIInventory m_InventoryPanel = default;

        [SerializeField] private UIInteraction m_InteractionPanel = default;

        [SerializeField] private UIPause m_PauseScreen = default;

        [SerializeField] private UISettings m_SettingScreen = default;

        [Header("Gameplay Components")]
        [SerializeField] private GameStateSO m_GameState = default;
        [SerializeField] private MenuSO m_MainMenu = default;
        [SerializeField] private InputReader m_InputReader = default;

        [Header("Listening on channels")]
        [SerializeField] private VoidEventChannelSO m_OnSceneReady = default;

        [Header("Dialogue Events")]
        //[SerializeField] private DialogueLineChannelSO m_OpenUIDialogueEvent = default;
        [SerializeField] private VoidEventChannelSO m_CloseUIDialogueEvent = default;

        //[Header("Inventory Events")]
        //[SerializeField] private VoidEventChannelSO m_OpenInventoryScreenForCookingEvent = default;

        [Header("Interaction Events")]
        [SerializeField] private InteractionUIEventChannelSO m_SetInteractionEvent = default;

        [Header("Broadcasting on ")]
        [SerializeField] private LoadEventChannelSO m_LoadMenuEvent = default;
        [SerializeField] private VoidEventChannelSO m_OnInteractionEndedEvent = default;

        private bool m_IsForCooking = false;


        private void Start()
        {
            m_OnSceneReady.onEventRaised += ResetUI;
            //m_OpenUIDialogueEvent.onEventRaised += OpenUIDialogue;
            m_CloseUIDialogueEvent.onEventRaised += CloseUIDialogue;
            m_InputReader.menuPauseEvent += OpenUIPause; // subscription to open Pause UI event happens in OnEnabled, but the close Event is only subscribed to when the popup is open

            //m_OpenInventoryScreenForCookingEvent.onEventRaised += SetInventoryScreenForCooking;
            m_SetInteractionEvent.onEventRaised += SetInteractionPanel;

            m_InputReader.openInventoryEvent += SetInventoryScreen;
            m_InventoryPanel.closed += CloseInventoryScreen;
        }

        private void ResetUI()
        {
            //m_DialogueController.gameObject.SetActive(false);
            m_InventoryPanel.gameObject.SetActive(false);
            m_PauseScreen.gameObject.SetActive(false);
            m_InteractionPanel.gameObject.SetActive(false);

            m_InputReader.EnableGameplayInput();

            Time.timeScale = 1;
        }

        /*private void OpenUIDialogue(LocalizedString dialogueLine, ActorSO actor)
        {
            m_DialogueController.SetDialogue(dialogueLine, actor);
            m_DialogueController.gameObject.SetActive(true);
        }*/

        private void CloseUIDialogue()
        {
            m_SelectionHandler.Unselect();
            //m_DialogueController.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            //Check if the event exists to avoid errors
            m_OnSceneReady.onEventRaised -= ResetUI;
            //m_OpenUIDialogueEvent.onEventRaised -= OpenUIDialogue;
            m_CloseUIDialogueEvent.onEventRaised -= CloseUIDialogue;

            m_InputReader.menuPauseEvent -= OpenUIPause;

            //m_OpenInventoryScreenForCookingEvent.onEventRaised -= SetInventoryScreenForCooking;
            m_SetInteractionEvent.onEventRaised -= SetInteractionPanel;
            m_InputReader.openInventoryEvent -= SetInventoryScreen;

            m_InventoryPanel.closed -= CloseInventoryScreen;
        }

        private void OpenUIPause()
        {
            m_InputReader.menuPauseEvent -= OpenUIPause; // you can open UI pause menu again, if it's closed

            //	Time.timeScale = 0; // Pause time

            m_PauseScreen.settingsScreenOpened += OpenSettingScreen;//once the UI Pause popup is open, listen to open Settings 
            m_PauseScreen.backToMainRequested += ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
            m_PauseScreen.resumed += CloseUIPause;//once the UI Pause popup is open, listen to unpause event

            m_PauseScreen.gameObject.SetActive(true);

            m_InputReader.EnableMenuInput();
            m_GameState.UpdateGameState(GameState.Pause);
        }

        private void CloseUIPause()
        {
            Time.timeScale = 1; // unpause time

            m_InputReader.menuPauseEvent += OpenUIPause; // you can open UI pause menu again, if it's closed

            // once the popup is closed, you can't listen to the following events 
            m_PauseScreen.settingsScreenOpened -= OpenSettingScreen;//once the UI Pause popup is open, listen to open Settings 
            m_PauseScreen.backToMainRequested -= ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
            m_PauseScreen.resumed -= CloseUIPause;//once the UI Pause popup is open, listen to unpause event

            m_PauseScreen.gameObject.SetActive(false);

            m_InputReader.EnableGameplayInput();
            m_SelectionHandler.Unselect();
            m_GameState.ResetToPreviousGameState();
        }

        private void OpenSettingScreen()
        {
            m_SettingScreen.closed += CloseSettingScreen; // sub to close setting event with event 

            m_PauseScreen.gameObject.SetActive(false); // Set pause screen to inactive
            m_SettingScreen.gameObject.SetActive(true);// set Setting screen to active 

            // time is still set to 0 and Input is still set to menuInput 
        }

        private void CloseSettingScreen()
        {
            //unsub from close setting events 
            m_SettingScreen.closed -= CloseSettingScreen;

            m_SelectionHandler.Unselect();
            m_PauseScreen.gameObject.SetActive(true); // Set pause screen to inactive

            m_SettingScreen.gameObject.SetActive(false);

            // time is still set to 0 and Input is still set to menuInput 
            //going out from setting screen gets us back to the pause screen
        }


        private void ShowBackToMenuConfirmationPopup()
        {
            m_PauseScreen.gameObject.SetActive(false); // Set pause screen to inactive

            m_PopupPanel.closePopupAction += HideBackToMenuConfirmationPopup;

            m_PopupPanel.confirmationResponseAction += BackToMainMenu;

            m_InputReader.EnableMenuInput();
            m_PopupPanel.gameObject.SetActive(true);
            m_PopupPanel.SetPopup(PopupType.BackToMenu);
        }

        private void BackToMainMenu(bool confirm)
        {
            HideBackToMenuConfirmationPopup();// hide confirmation screen, show close UI pause, 

            if (confirm)
            {
                CloseUIPause();//close ui pause to unsub from all events 
                m_LoadMenuEvent.RaiseEvent(m_MainMenu, false); //load main menu
            }
        }

        private void HideBackToMenuConfirmationPopup()
        {
            m_PopupPanel.closePopupAction -= HideBackToMenuConfirmationPopup;
            m_PopupPanel.confirmationResponseAction -= BackToMainMenu;

            m_PopupPanel.gameObject.SetActive(false);
            m_SelectionHandler.Unselect();
            m_PauseScreen.gameObject.SetActive(true); // Set pause screen to inactive

            // time is still set to 0 and Input is still set to menuInput 
            //going out from confirmaiton popup screen gets us back to the pause screen
        }

        private void SetInventoryScreenForCooking()
        {
            m_IsForCooking = true;
            OpenInventoryScreen();
        }

        private void SetInventoryScreen()
        {
            m_IsForCooking = false;
            OpenInventoryScreen();
        }

        private void OpenInventoryScreen()
        {
            m_InputReader.menuPauseEvent -= OpenUIPause; // you cant open the UI Pause again when you are in inventory  
            m_InputReader.menuUnpauseEvent -= CloseUIPause; // you can close the UI Pause popup when you are in inventory 

            m_InputReader.menuCloseEvent += CloseInventoryScreen;

            m_InputReader.closeInventoryEvent += CloseInventoryScreen;

            if (m_IsForCooking)
                m_InventoryPanel.FillInventory(InventoryTabType.Recipe, true);
            else
                m_InventoryPanel.FillInventory();

            m_InventoryPanel.gameObject.SetActive(true);

            m_InputReader.EnableMenuInput();

            m_GameState.UpdateGameState(GameState.Inventory);
        }

        private void CloseInventoryScreen()
        {
            m_InputReader.menuPauseEvent += OpenUIPause; // you cant open the UI Pause again when you are in inventory  

            m_InputReader.menuCloseEvent -= CloseInventoryScreen;
            m_InputReader.closeInventoryEvent -= CloseInventoryScreen;

            m_InventoryPanel.gameObject.SetActive(false);

            if (m_IsForCooking)
                m_OnInteractionEndedEvent.RaiseEvent();

            m_SelectionHandler.Unselect();
            m_InputReader.EnableGameplayInput();
            m_GameState.ResetToPreviousGameState();
        }


        private void SetInteractionPanel(bool isOpenEvent, InteractionType interactionType)
        {
            if (isOpenEvent)
                m_InteractionPanel.FillInteractionPanel(interactionType);

            m_InteractionPanel.gameObject.SetActive(isOpenEvent);
        }
    }
}

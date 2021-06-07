using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.Gameplay.ScriptableObjects;
using Strungerhulder.Dialogues.ScriptableObjects;

namespace Strungerhulder.Dialogues
{
    /// <summary>
    /// Takes care of all things dialogue, whether they are coming from within a Timeline or just from the interaction with a character, or by any other mean.
    /// Keeps track of choices in the dialogue (if any) and then gives back control to gameplay when appropriate.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        //	[SerializeField] private ChoiceBox _choiceBox; // TODO: Demonstration purpose only. Remove or adjust later.
        [SerializeField] private InputReader m_InputReader = default;

        [Header("Listening on channels")]
        [SerializeField] private DialogueDataChannelSO m_StartDialogue = default;
        [SerializeField] private DialogueChoiceChannelSO m_MakeDialogueChoiceEvent = default;

        [Header("BoradCasting on channels")]
        [SerializeField] private DialogueLineChannelSO m_OpenUIDialogueEvent = default;
        [SerializeField] private DialogueChoicesChannelSO m_ShowChoicesUIEvent = default;
        [SerializeField] private DialogueDataChannelSO m_EndDialogue = default;
        [SerializeField] private VoidEventChannelSO m_ContinueWithStep = default;
        [SerializeField] private VoidEventChannelSO m_CloseDialogueUIEvent = default;

        [Header("Gameplay Components")]
        [SerializeField] private GameStateSO m_GameState = default;


        private DialogueDataSO m_CurrentDialogue = default;
        private int m_Counter;
        private bool m_ReachedEndOfDialogue { get => m_Counter >= m_CurrentDialogue.DialogueLines.Count; }


        private void Start() => m_StartDialogue.onEventRaised += DisplayDialogueData;


        /// <summary>
        /// Displays DialogueData in the UI, one by one.
        /// </summary>
        /// <param name="dialogueDataSO"></param>
        public void DisplayDialogueData(DialogueDataSO dialogueDataSO)
        {
            if (m_GameState.currentGameState != GameState.Cutscene)
                m_GameState.UpdateGameState(GameState.Dialogue);

            BeginDialogueData(dialogueDataSO);
            DisplayDialogueLine(m_CurrentDialogue.DialogueLines[m_Counter], dialogueDataSO.Actor);
        }

        /// <summary>
        /// Displays a line of dialogue in the UI, by requesting it to the <c>DialogueManager</c>.
        /// This function is also called by <c>DialogueBehaviour</c> from clips on Timeline during cutscenes.
        /// </summary>
        /// <param name="dialogueLine"></param>
        public void DisplayDialogueLine(LocalizedString dialogueLine, ActorSO actor)
        {
            m_OpenUIDialogueEvent.RaiseEvent(dialogueLine, actor);
        }

        public void DialogueEndedAndCloseDialogueUI()
        {
            if (m_EndDialogue != null)
                m_EndDialogue.RaiseEvent(m_CurrentDialogue);

            if (m_CloseDialogueUIEvent != null)
                m_CloseDialogueUIEvent.RaiseEvent();

            m_GameState.ResetToPreviousGameState();
            m_InputReader.advanceDialogueEvent -= OnAdvance;
            m_InputReader.EnableGameplayInput();
        }


        /// <summary>
        /// Prepare DialogueManager when first time displaying DialogueData. 
        /// <param name="dialogueDataSO"></param>
        private void BeginDialogueData(DialogueDataSO dialogueDataSO)
        {
            m_Counter = 0;
            m_InputReader.EnableDialogueInput();
            m_InputReader.advanceDialogueEvent += OnAdvance;
            m_CurrentDialogue = dialogueDataSO;
        }

        private void OnAdvance()
        {
            m_Counter++;

            if (!m_ReachedEndOfDialogue)
                DisplayDialogueLine(m_CurrentDialogue.DialogueLines[m_Counter], m_CurrentDialogue.Actor);
            else
            {
                if (m_CurrentDialogue.Choices.Count > 0)
                    DisplayChoices(m_CurrentDialogue.Choices);
                else
                    DialogueEndedAndCloseDialogueUI();
            }
        }

        private void DisplayChoices(List<Choice> choices)
        {
            m_InputReader.advanceDialogueEvent -= OnAdvance;

            m_MakeDialogueChoiceEvent.onEventRaised += MakeDialogueChoice;
            m_ShowChoicesUIEvent.RaiseEvent(choices);
        }

        private void MakeDialogueChoice(Choice choice)
        {
            m_MakeDialogueChoiceEvent.onEventRaised -= MakeDialogueChoice;

            if (choice.ActionType == ChoiceActionType.continueWithStep)
            {
                if (m_ContinueWithStep != null)
                    m_ContinueWithStep.RaiseEvent();

                if (choice.NextDialogue != null)
                    DisplayDialogueData(choice.NextDialogue);
            }
            else
            {
                if (choice.NextDialogue != null)
                    DisplayDialogueData(choice.NextDialogue);
                else
                    DialogueEndedAndCloseDialogueUI();
            }
        }

        private void DialogueEnded()
        {
            if (m_EndDialogue != null)
                m_EndDialogue.RaiseEvent(m_CurrentDialogue);

            m_GameState.ResetToPreviousGameState();
        }
    }
}

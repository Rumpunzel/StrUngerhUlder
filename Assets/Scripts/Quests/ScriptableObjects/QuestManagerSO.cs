using System.Collections.Generic;
using UnityEngine;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.Inventory.ScriptableObjects;

namespace Strungerhulder.Quests.ScriptableObjects
{
    [CreateAssetMenu(fileName = "QuestManager", menuName = "Quests/QuestManager", order = 51)]
    public class QuestManagerSO : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private List<QuestlineSO> m_Questlines = default;
        [SerializeField] private ProtagonistInventory m_Inventory = default;

        [Header("Listening to channels")]
        [SerializeField] private VoidEventChannelSO m_CheckStepValidityEvent = default;
        [SerializeField] private DialogueDataChannelSO m_EndDialogueEvent = default;


        [Header("Broadcasting on channels")]
        [SerializeField] private VoidEventChannelSO m_CompleteDialogueEvent = default;
        [SerializeField] private VoidEventChannelSO m_IncompleteDialogueEvent = default;

        [SerializeField] private ItemEventChannelSO m_GiveItemEvent = default;
        [SerializeField] private ItemEventChannelSO m_RewardItemEvent = default;


        private QuestSO m_CurrentQuest = null;
        private QuestlineSO m_CurrentQuestline;
        private StepSO m_CurrentStep;
        private int m_CurrentQuestIndex = 0;
        private int m_CurrentQuestlineIndex = 0;
        private int m_CurrentStepIndex = 0;


        public void StartGame()
        {
            //Add code for saved information
            m_CheckStepValidityEvent.onEventRaised += CheckStepValidity;
            m_EndDialogueEvent.onEventRaised += EndDialogue;

            StartQuestline();
        }

        public DialogueDataSO InteractWithCharacter(ActorSO actor, bool isCheckValidity, bool isValid)
        {
            if (m_CurrentQuest == null)
            {
                if (CheckQuestlineForQuestWithActor(actor))
                    StartQuest(actor);
            }

            if (HasStep(actor))
            {
                if (isCheckValidity)
                {
                    if (isValid)
                        return m_CurrentStep.CompleteDialogue;
                    else
                        return m_CurrentStep.IncompleteDialogue;
                }
                else
                    return m_CurrentStep.DialogueBeforeStep;
            }

            return null;
        }


        private void StartQuestline()
        {
            if (m_Questlines != null)
            {
                if (m_Questlines.Exists(o => !o.IsDone))
                {
                    m_CurrentQuestlineIndex = m_Questlines.FindIndex(o => !o.IsDone);

                    if (m_CurrentQuestlineIndex >= 0)
                        m_CurrentQuestline = m_Questlines.Find(o => !o.IsDone);
                }
            }
        }

        private bool HasStep(ActorSO actorToCheckWith)
        {
            if (m_CurrentStep != null)
            {
                if (m_CurrentStep.Actor == actorToCheckWith)
                    return true;
            }

            return false;
        }

        private bool CheckQuestlineForQuestWithActor(ActorSO actorToCheckWith)
        {
            if (m_CurrentQuest == null) // Check if there's a current quest 
            {
                if (m_CurrentQuestline != null)
                    return m_CurrentQuestline.Quests.Exists(o => !o.IsDone && o.Steps != null && o.Steps[0].Actor == actorToCheckWith);
            }

            return false;
        }

        // When Interacting with a character, we ask the quest manager if there's a quest that starts with a step with a certain character
        private void StartQuest(ActorSO actorToCheckWith)
        {
            if (m_CurrentQuest != null) // Check if there's a current quest 
                return;

            if (m_CurrentQuestline != null)
            {
                // Find quest index
                m_CurrentQuestIndex = m_CurrentQuestline.Quests.FindIndex(o => !o.IsDone && o.Steps != null && o.Steps[0].Actor == actorToCheckWith);

                if ((m_CurrentQuestline.Quests.Count > m_CurrentQuestIndex) && (m_CurrentQuestIndex >= 0))
                {
                    m_CurrentQuest = m_CurrentQuestline.Quests[m_CurrentQuestIndex];
                    // Start Step
                    m_CurrentStepIndex = 0;
                    m_CurrentStepIndex = m_CurrentQuest.Steps.FindIndex(o => o.IsDone == false);

                    if (m_CurrentStepIndex >= 0)
                        StartStep();
                }
            }
        }

        private void StartStep()
        {
            if (m_CurrentQuest.Steps != null)
            {
                if (m_CurrentQuest.Steps.Count > m_CurrentStepIndex)
                    m_CurrentStep = m_CurrentQuest.Steps[m_CurrentStepIndex];
            }
        }

        private void CheckStepValidity()
        {
            if (m_CurrentStep != null)
            {
                switch (m_CurrentStep.Type)
                {
                    case StepType.CheckItem:
                        if (m_Inventory.Contains(m_CurrentStep.Item))
                        {
                            m_Inventory.Contains(m_CurrentStep.Item);
                            //Trigger win dialogue
                            m_CompleteDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            //trigger lose dialogue
                            m_IncompleteDialogueEvent.RaiseEvent();
                        }
                        break;

                    case StepType.GiveItem:
                        if (m_Inventory.Contains(m_CurrentStep.Item))
                        {
                            m_GiveItemEvent.RaiseEvent(m_CurrentStep.Item);
                            m_CompleteDialogueEvent.RaiseEvent();
                        }
                        else
                        {
                            //trigger lose dialogue
                            m_IncompleteDialogueEvent.RaiseEvent();
                        }
                        break;

                    case StepType.RewardItem:
                        m_RewardItemEvent.RaiseEvent(m_CurrentStep.Item);

                        // No dialogue is needed after Reward Item
                        if (m_CurrentStep.CompleteDialogue != null)
                            m_CompleteDialogueEvent.RaiseEvent();
                        else
                            EndStep();

                        break;

                    case StepType.Dialogue:
                        //dialogue has already been played
                        if (m_CurrentStep.CompleteDialogue != null)
                            m_CompleteDialogueEvent.RaiseEvent();
                        else
                            EndStep();

                        break;
                }
            }
        }

        private void EndDialogue(DialogueDataSO dialogue)
        {
            //depending on the dialogue that ended, do something 
            switch (dialogue.DialogueType)
            {
                case DialogueType.winDialogue:
                    EndStep();
                    break;
                case DialogueType.startDialogue:
                    CheckStepValidity();
                    break;
                default:
                    break;
            }
        }

        private void EndStep()
        {
            m_CurrentStep = null;

            if (m_CurrentQuest != null)
            {
                if (m_CurrentQuest.Steps.Count > m_CurrentStepIndex)
                {
                    m_CurrentQuest.Steps[m_CurrentStepIndex].FinishStep();

                    if (m_CurrentQuest.Steps.Count > m_CurrentStepIndex + 1)
                    {
                        m_CurrentStepIndex++;
                        StartStep();
                    }
                    else
                        EndQuest();
                }
            }
        }

        private void EndQuest()
        {
            if (m_CurrentQuest != null)
                m_CurrentQuest.FinishQuest();

            m_CurrentQuest = null;
            m_CurrentQuestIndex = -1;

            if (m_CurrentQuestline != null)
            {
                if (!m_CurrentQuestline.Quests.Exists(o => !o.IsDone))
                    EndQuestline();
            }
        }

        private void EndQuestline()
        {
            if (m_Questlines != null)
            {
                if (m_CurrentQuestline != null)
                    m_CurrentQuestline.FinishQuestline();

                if (m_Questlines.Exists(o => o.IsDone))
                    StartQuestline();
            }
        }
    }
}

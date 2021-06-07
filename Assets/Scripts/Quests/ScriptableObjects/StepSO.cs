using UnityEngine;
using Strungerhulder.Inventories.ScriptableObjects;
using Strungerhulder.Dialogues.ScriptableObjects;

namespace Strungerhulder.Quests.ScriptableObjects
{
    public enum StepType
    {
        Dialogue,
        GiveItem,
        CheckItem,
        RewardItem,
    }

    [CreateAssetMenu(fileName = "Step", menuName = "Quests/Step", order = 51)]
    public class StepSO : ScriptableObject
    {
        public DialogueDataSO DialogueBeforeStep => m_DialogueBeforeStep;
        public DialogueDataSO CompleteDialogue => m_CompleteDialogue;
        public DialogueDataSO IncompleteDialogue => m_IncompleteDialogue;
        public Item Item => m_Item;
        public StepType Type => m_Type;
        public bool IsDone => m_IsDone;
        public ActorSO Actor => m_Actor;


        [Tooltip("The Character this mission will need interaction with")]
        [SerializeField] private ActorSO m_Actor = default;

        [Tooltip("The dialogue that will be diplayed befor an action, if any")]
        [SerializeField] private DialogueDataSO m_DialogueBeforeStep = default;

        [Tooltip("The dialogue that will be diplayed when the step is achieved")]
        [SerializeField] private DialogueDataSO m_CompleteDialogue = default;

        [Tooltip("The dialogue that will be diplayed if the step is not achieved yet")]
        [SerializeField] private DialogueDataSO m_IncompleteDialogue = default;

        [Tooltip("The item to check/give/reward")]
        [SerializeField] private Item m_Item = default;

        [Tooltip("The type of the step")]
        [SerializeField] private StepType m_Type = default;

        [SerializeField] bool m_IsDone = false;


        public void FinishStep() => m_IsDone = true;
    }
}

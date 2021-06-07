using UnityEngine;
using Strungerhulder.Interactions;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Is Picking Up")]
    public class IsPickingUpSO : StateConditionSO<IsPickingUpCondition> { }

    public class IsPickingUpCondition : Condition
    {
        //Component references
        private InteractionManager m_InteractionManager;


        public override void Awake(StateMachine stateMachine)
        {
            m_InteractionManager = stateMachine.GetComponent<InteractionManager>();
        }

        protected override bool Statement()
        {
            if (m_InteractionManager.currentInteraction == null ||
                m_InteractionManager.currentInteraction.type != InteractionType.PickUp)
                return false;

            // Consume it
            m_InteractionManager.currentInteraction.type = InteractionType.None;
            return true;
        }
    }
}

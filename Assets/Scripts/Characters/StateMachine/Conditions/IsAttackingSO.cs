using UnityEngine;
using Strungerhulder.Interactions;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Is Attacking")]
    public class IsAttackingSO : StateConditionSO<IsAttackingCondition> { }

    public class IsAttackingCondition : Condition
    {
        private InteractionManager m_InteractionManager;


        public override void Awake(StateMachine stateMachine)
        {
            m_InteractionManager = stateMachine.GetComponent<InteractionManager>();
        }

        protected override bool Statement()
        {
            if (m_InteractionManager.currentInteraction == null ||
                m_InteractionManager.currentInteraction.type != InteractionType.Attack)
                return false;

            // Consume it
            m_InteractionManager.currentInteraction.type = InteractionType.None;
            return true;
        }
    }
}

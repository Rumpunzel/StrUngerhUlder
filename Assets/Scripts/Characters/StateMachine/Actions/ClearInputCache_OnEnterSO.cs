using UnityEngine;
using Strungerhulder.Interactions;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ClearInputCache_OnEnter", menuName = "State Machines/Actions/Clear Input Cache On Enter")]
    public class ClearInputCache_OnEnterSO : StateActionSO
    {
        protected override StateAction CreateAction() => new ClearInputCache_OnEnter();
    }

    public class ClearInputCache_OnEnter : StateAction
    {
        private Protagonist m_Protagonist;
        private InteractionManager m_InteractionManager;

        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_InteractionManager = stateMachine.GetComponentInChildren<InteractionManager>();
        }

        public override void OnUpdate() { }

        public override void OnStateEnter()
        {
            m_Protagonist.jumpInput = false;
            m_InteractionManager.currentInteraction = null;
        }
    }
}

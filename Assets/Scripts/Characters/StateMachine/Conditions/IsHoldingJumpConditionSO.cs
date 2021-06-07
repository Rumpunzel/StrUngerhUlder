using UnityEngine;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Is Holding Jump")]
    public class IsHoldingJumpConditionSO : StateConditionSO<IsHoldingJumpCondition> { }

    public class IsHoldingJumpCondition : Condition
    {
        //Component references
        private Protagonist m_ProtagonistScript;


        public override void Awake(StateMachine stateMachine)
        {
            m_ProtagonistScript = stateMachine.GetComponent<Protagonist>();
        }

        protected override bool Statement() => m_ProtagonistScript.jumpInput;
    }
}

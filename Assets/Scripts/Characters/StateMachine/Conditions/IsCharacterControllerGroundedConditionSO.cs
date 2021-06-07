using UnityEngine;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Is Character Controller Grounded Condition")]
    public class IsCharacterControllerGroundedConditionSO : StateConditionSO<IsCharacterControllerGroundedCondition> { }

    public class IsCharacterControllerGroundedCondition : Condition
    {
        private CharacterController m_CharacterController;

        public override void Awake(StateMachine stateMachine)
        {
            m_CharacterController = stateMachine.GetComponent<CharacterController>();
        }

        protected override bool Statement() => m_CharacterController.isGrounded;
    }
}

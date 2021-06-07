using UnityEngine;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Started Moving")]
    public class IsMovingConditionSO : StateConditionSO<IsMovingCondition>
    {
        public float threshold = 0.02f;
    }

    public class IsMovingCondition : Condition
    {
        private Protagonist m_ProtagonistScript;
        protected new IsMovingConditionSO OriginSO => (IsMovingConditionSO)base.OriginSO; // The SO this Condition spawned from


        public override void Awake(StateMachine stateMachine)
        {
            m_ProtagonistScript = stateMachine.GetComponent<Protagonist>();
        }

        protected override bool Statement()
        {
            Vector3 movementVector = m_ProtagonistScript.movementInput;
            movementVector.y = 0f;

            return movementVector.sqrMagnitude > OriginSO.threshold;
        }
    }
}

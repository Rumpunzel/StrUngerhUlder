using UnityEngine;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HorizontalMove", menuName = "State Machines/Actions/Horizontal Move")]
    public class HorizontalMoveActionSO : StateActionSO<HorizontalMoveAction> { }

    public class HorizontalMoveAction : StateAction
    {
        private Protagonist m_Protagonist;
        private CharacterMovementStatsSO m_MovementStats;


        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_MovementStats = m_Protagonist.movementStats;
        }

        public override void OnUpdate()
        {
            float moveSpeed = m_Protagonist.targetSpeed * m_MovementStats.moveSpeed;

            m_Protagonist.movementVector.x = m_Protagonist.movementInput.x * moveSpeed;
            m_Protagonist.movementVector.z = m_Protagonist.movementInput.z * moveSpeed;
        }
    }
}

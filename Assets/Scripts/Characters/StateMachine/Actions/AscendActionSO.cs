using UnityEngine;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Ascend", menuName = "State Machines/Actions/Ascend")]
    public class AscendActionSO : StateActionSO<AscendAction> { }

    public class AscendAction : StateAction
    {
        //Component references
        private Protagonist m_Protagonist;
        private CharacterMovementStatsSO m_MovementStats;

        private float m_VerticalMovement;


        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_MovementStats = m_Protagonist.movementStats;
        }

        public override void OnStateEnter()
        {
            m_VerticalMovement = Mathf.Sqrt(m_MovementStats.jumpHeight * -3.0f * Physics.gravity.y * m_MovementStats.gravityAscendMultiplier);
        }

        public override void OnUpdate()
        {
            m_VerticalMovement += Physics.gravity.y * m_MovementStats.gravityAscendMultiplier * Time.deltaTime;
            m_Protagonist.verticalMovement = m_VerticalMovement;
        }
    }
}

using UnityEngine;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Actions/Descend")]
    public class DescendActionSO : StateActionSO<DescendAction> { }

    public class DescendAction : StateAction
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
            m_VerticalMovement = m_Protagonist.verticalMovement;

            //Prevents a double jump if the player keeps holding the jump button
            //Basically it "consumes" the input
            m_Protagonist.jumpInput = false;
        }

        public override void OnUpdate()
        {
            m_VerticalMovement += Physics.gravity.y * m_MovementStats.gravityDescendMultiplier * Time.deltaTime;

            //Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
            m_VerticalMovement = m_MovementStats.ValidateVerticalSpeed(m_VerticalMovement);
            m_Protagonist.verticalMovement = m_VerticalMovement;
        }
    }
}

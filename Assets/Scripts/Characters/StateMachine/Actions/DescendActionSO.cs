using UnityEngine;
using Strungerhulder.Characters;

namespace Strungerhulder.StateMachine.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Actions/Descend")]
    public class DescendActionSO : StateActionSO<DescendAction> { }

    public class DescendAction : StateAction
    {
        //Component references
        private Protagonist m_Protagonist;

        private float m_VerticalMovement;

        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnStateEnter()
        {
            m_VerticalMovement = m_Protagonist.movementVector.y;

            //Prevents a double jump if the player keeps holding the jump button
            //Basically it "consumes" the input
            m_Protagonist.jumpInput = false;
        }

        public override void OnUpdate()
        {
            m_VerticalMovement += Physics.gravity.y * Protagonist.GRAVITY_MULTIPLIER * Time.deltaTime;
            //Note that even if it's added, the above value is negative due to Physics.gravity.y

            //Cap the maximum so the player doesn't reach incredible speeds when freefalling from high positions
            m_VerticalMovement = Mathf.Clamp(m_VerticalMovement, Protagonist.MAX_FALL_SPEED, Protagonist.MAX_RISE_SPEED);

            m_Protagonist.movementVector.y = m_VerticalMovement;
        }
    }
}

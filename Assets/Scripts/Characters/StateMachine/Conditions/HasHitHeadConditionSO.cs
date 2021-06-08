using UnityEngine;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Has Hit the Head")]
    public class HasHitHeadConditionSO : StateConditionSO<HasHitHeadCondition> { }

    public class HasHitHeadCondition : Condition
    {
        //Component references
        private Protagonist m_Protagonist;
        private CharacterController m_CharacterController;
        private Transform m_Transform;

        public override void Awake(StateMachine stateMachine)
        {
            m_Transform = stateMachine.GetComponent<Transform>();
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_CharacterController = stateMachine.GetComponent<CharacterController>();
        }

        protected override bool Statement()
        {
            bool isMovingUpwards = m_Protagonist.verticalMovement > 0f;

            if (isMovingUpwards)
            {
                // Making sure the collision is near the top of the head
                float permittedDistance = m_CharacterController.radius / 2f;
                float topPositionY = m_Transform.position.y + m_CharacterController.height;
                float distance = Mathf.Abs(m_Protagonist.lastHit.point.y - topPositionY);

                if (distance <= permittedDistance)
                {
                    m_Protagonist.jumpInput = false;
                    m_Protagonist.verticalMovement = 0f;

                    return true;
                }
            }

            return false;
        }
    }
}

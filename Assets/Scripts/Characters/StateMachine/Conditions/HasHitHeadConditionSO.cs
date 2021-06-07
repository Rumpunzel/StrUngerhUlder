using UnityEngine;

namespace Strungerhulder.StateMachine.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Has Hit the Head")]
    public class HasHitHeadConditionSO : StateConditionSO<HasHitHeadCondition> { }

    public class HasHitHeadCondition : Condition
    {
        //Component references
        private Protagonist m_ProtagonistScript;
        private CharacterController m_CharacterController;
        private Transform m_Transform;

        public override void Awake(StateMachine stateMachine)
        {
            m_Transform = stateMachine.GetComponent<Transform>();
            m_ProtagonistScript = stateMachine.GetComponent<Protagonist>();
            m_CharacterController = stateMachine.GetComponent<CharacterController>();
        }

        protected override bool Statement()
        {
            bool isMovingUpwards = m_ProtagonistScript.movementVector.y > 0f;

            if (isMovingUpwards)
            {
                // Making sure the collision is near the top of the head
                float permittedDistance = m_CharacterController.radius / 2f;
                float topPositionY = m_Transform.position.y + m_CharacterController.height;
                float distance = Mathf.Abs(m_ProtagonistScript.lastHit.point.y - topPositionY);

                if (distance <= permittedDistance)
                {
                    m_ProtagonistScript.jumpInput = false;
                    m_ProtagonistScript.movementVector.y = 0f;

                    return true;
                }
            }

            return false;
        }
    }
}

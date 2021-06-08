using UnityEngine;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ApplyMovementVector", menuName = "State Machines/Actions/Apply Movement Vector")]
    public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction> { }

    public class ApplyMovementVectorAction : StateAction
    {
        //Component references
        private Protagonist m_Protagonist;
        private CharacterController m_CharacterController;

        private CharacterMovementStatsSO m_MovementStats;

        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_CharacterController = stateMachine.GetComponent<CharacterController>();

            m_MovementStats = m_Protagonist.movementStats;
        }

        public override void OnUpdate()
        {
            Vector2 horizontalMovement = m_Protagonist.horizontalMovementVector;
            Vector3 newMovementVector = new Vector3(
                horizontalMovement.x,
                m_Protagonist.verticalMovement,
                horizontalMovement.y
            );

            m_CharacterController.Move(newMovementVector * Time.deltaTime);
            //m_Protagonist.movementVector = m_CharacterController.velocity;

            if (horizontalMovement != Vector2.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(new Vector3(horizontalMovement.x, 0f, horizontalMovement.y), Vector3.up);
                m_Protagonist.transform.rotation = Quaternion.RotateTowards(m_Protagonist.transform.rotation, toRotation, m_MovementStats.turnRate * Time.deltaTime);
            }
        }
    }
}

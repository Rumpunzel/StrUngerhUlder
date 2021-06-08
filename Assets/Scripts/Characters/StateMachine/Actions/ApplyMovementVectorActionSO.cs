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

        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_CharacterController = stateMachine.GetComponent<CharacterController>();
        }

        public override void OnUpdate()
        {
            m_CharacterController.Move(m_Protagonist.movementVector * Time.deltaTime);
            //m_Protagonist.movementVector = m_CharacterController.velocity;

            if (!(m_Protagonist.movementVector.x == 0f && m_Protagonist.movementVector.z == 0f))
            {
                Quaternion toRotation = Quaternion.LookRotation(new Vector3(m_Protagonist.movementVector.x, 0f, m_Protagonist.movementVector.z), Vector3.up);
                m_Protagonist.transform.rotation = Quaternion.RotateTowards(m_Protagonist.transform.rotation, toRotation, Protagonist.TURN_RATE * Time.deltaTime);
            }
        }
    }
}

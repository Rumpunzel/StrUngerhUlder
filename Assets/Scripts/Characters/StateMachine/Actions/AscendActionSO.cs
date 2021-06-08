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
        private float m_VerticalMovement;


        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnStateEnter()
        {
            m_VerticalMovement = Mathf.Sqrt(m_Protagonist.jumpHeight * -3.0f * Physics.gravity.y);
        }

        public override void OnUpdate()
        {
            m_VerticalMovement += Physics.gravity.y * Time.deltaTime;
            //Note that even if it's added, the above value is negative due to Physics.gravity.y

            m_Protagonist.movementVector.y = m_VerticalMovement;
        }
    }
}

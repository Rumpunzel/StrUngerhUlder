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


        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnUpdate()
        {
            m_Protagonist.movementVector.x = m_Protagonist.movementInput.x * m_Protagonist.moveSpeed;
            m_Protagonist.movementVector.z = m_Protagonist.movementInput.z * m_Protagonist.moveSpeed;
        }
    }
}

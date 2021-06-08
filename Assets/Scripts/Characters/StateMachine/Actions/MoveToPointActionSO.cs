using UnityEngine;
using UnityEngine.AI;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MoveToPoint", menuName = "State Machines/Actions/Move To Point")]
    public class MoveToPointActionSO : StateActionSO<MoveToPointAction> { }

    public class MoveToPointAction : StateAction
    {
        //Component references
        private Protagonist m_Protagonist;
        private NavMeshAgent m_NavAgent;

        private CharacterMovementStatsSO m_MovementStats;


        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_NavAgent = stateMachine.GetComponent<NavMeshAgent>();

            m_MovementStats = m_Protagonist.movementStats;
        }

        public override void OnUpdate()
        {
            m_NavAgent.speed = m_MovementStats.CalculateHorizontalSpeed(m_Protagonist.isRunning);
            m_NavAgent.acceleration = m_Protagonist.movementStats.moveAcceleration;
            m_Protagonist.destinationPoint = m_Protagonist.destinationInput;
        }
    }
}

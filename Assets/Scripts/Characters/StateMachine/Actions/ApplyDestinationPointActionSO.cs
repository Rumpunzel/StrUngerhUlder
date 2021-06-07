using UnityEngine;
using UnityEngine.AI;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ApplyDestinationPointAction", menuName = "State Machines/Actions/Apply Destination Point")]
    public class ApplyDestinationPointActionSO : StateActionSO<ApplyDestinationPointAction> { }

    public class ApplyDestinationPointAction : StateAction
    {
        //Component references
        private Protagonist m_Protagonist;
        private NavMeshAgent m_NavAgent;


        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_NavAgent = stateMachine.GetComponent<NavMeshAgent>();
        }

        public override void OnUpdate()
        {
            m_NavAgent.destination = m_Protagonist.destinationPoint;
            //m_Protagonist.destinationPoint = m_Protagonist.transform.position;
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

namespace Strungerhulder.StateMachine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MoveToPoint", menuName = "State Machines/Actions/Move To Point")]
    public class MoveToPointActionSO : StateActionSO<MoveToPointAction>
    {
        [Tooltip("Horizontal XZ plane speed multiplier")]
        public float speed = 8f;

        [Tooltip("The acceleration applied to reach the desired speed")]
        [Range(0.1f, 100f)] public float acceleration = 8f;
    }

    public class MoveToPointAction : StateAction
    {
        //Component references
        private Protagonist m_Protagonist;
        private NavMeshAgent m_NavAgent;

        private new MoveToPointActionSO m_OriginSO => (MoveToPointActionSO)base.OriginSO; // The SO this StateAction spawned from

        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_NavAgent = stateMachine.GetComponent<NavMeshAgent>();
        }

        public override void OnUpdate()
        {
            m_NavAgent.speed = m_OriginSO.speed;
            m_NavAgent.acceleration = m_OriginSO.acceleration;
            m_Protagonist.destinationPoint = m_Protagonist.destinationInput;
        }
    }
}

using UnityEngine;
using UnityEngine.AI;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    /// <summary>
    /// Flexible StateActionSO for the StateMachine which allows to set any parameter on the Animator, in any moment of the state (OnStateEnter, OnStateExit, or each OnUpdate).
    /// </summary>
    [CreateAssetMenu(fileName = "AnimatorMoveSpeedAction", menuName = "State Machines/Actions/Set Animator Move Speed")]
    public class AnimatorMoveSpeedActionSO : StateActionSO
    {
        public string parameterName = default;

        protected override StateAction CreateAction() => new AnimatorMoveSpeedAction(Animator.StringToHash(parameterName));
    }

    public class AnimatorMoveSpeedAction : StateAction
    {
        //Component references
        private Animator m_Animator;
        private Protagonist m_Protagonist;
        private NavMeshAgent m_NavAgent;

        private CharacterMovementStatsSO m_MovementStats;

        private new AnimatorParameterActionSO m_OriginSO => (AnimatorParameterActionSO)base.OriginSO; // The SO this StateAction spawned from
        private int m_ParameterHash;


        public AnimatorMoveSpeedAction(int parameterHash)
        {
            m_ParameterHash = parameterHash;
        }

        public override void Awake(StateMachine stateMachine)
        {
            m_Animator = stateMachine.GetComponent<Animator>();
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_NavAgent = stateMachine.GetComponent<NavMeshAgent>();

            m_MovementStats = m_Protagonist.movementStats;
        }

        public override void OnUpdate()
        {
            float normalisedSpeed = m_NavAgent.enabled ?
                m_NavAgent.velocity.magnitude / m_MovementStats.moveSpeed :
                m_Protagonist.horizontalMovementVector.magnitude / m_MovementStats.moveSpeed;

            m_Animator.SetFloat(m_ParameterHash, normalisedSpeed);
        }
    }
}

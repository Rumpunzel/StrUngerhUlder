using UnityEngine;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    /// <summary>
    /// An Action to clear a <see cref="Protagonist.movementVector"/> at the <see cref="StateAction.SpecificMoment"/> <see cref="StopMovementActionSO.Moment"/>
    /// </summary>
    [CreateAssetMenu(fileName = "StopMovementAction", menuName = "State Machines/Actions/Stop Movement")]
    public class StopMovementActionSO : StateActionSO
    {
        [SerializeField] private StateAction.SpecificMoment m_Moment = default;
        public StateAction.SpecificMoment Moment => m_Moment;

        protected override StateAction CreateAction() => new StopMovement();
    }

    public class StopMovement : StateAction
    {
        private Protagonist m_Protagonist;
        private new StopMovementActionSO OriginSO => (StopMovementActionSO)base.OriginSO;


        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnUpdate()
        {
            if (OriginSO.Moment == SpecificMoment.OnUpdate)
                NullMovement();
        }

        public override void OnStateEnter()
        {
            if (OriginSO.Moment == SpecificMoment.OnStateEnter)
                NullMovement();
        }

        public override void OnStateExit()
        {
            if (OriginSO.Moment == SpecificMoment.OnStateExit)
                NullMovement();
        }


        private void NullMovement()
        {
            m_Protagonist.destinationInput = m_Protagonist.transform.position;
            m_Protagonist.horizontalMovementVector = Vector2.zero;
        }
    }
}

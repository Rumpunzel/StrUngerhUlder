using UnityEngine;
using Strungerhulder.Characters;

namespace Strungerhulder.StateMachine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GroundGravity", menuName = "State Machines/Actions/Ground Gravity")]
    public class GroundGravityActionSO : StateActionSO<GroundGravityAction>
    {
        [Tooltip("Vertical movement pulling down the player to keep it anchored to the ground.")]
        public float verticalPull = -5f;
    }

    public class GroundGravityAction : StateAction
    {
        //Component references
        private Protagonist m_Protagonist;

        private new GroundGravityActionSO m_OriginSO => (GroundGravityActionSO)base.OriginSO; // The SO this StateAction spawned from

        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnUpdate()
        {
            m_Protagonist.movementVector.y = m_OriginSO.verticalPull;
        }
    }
}

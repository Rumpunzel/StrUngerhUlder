using UnityEngine;

namespace Strungerhulder.StateMachine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Ascend", menuName = "State Machines/Actions/Ascend")]
    public class AscendActionSO : StateActionSO<AscendAction>
    {
        [Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
        public float jumpHeight = 1f;
    }

    public class AscendAction : StateAction
    {
        //Component references
        private Protagonist m_Protagonist;

        private float m_VerticalMovement;

        private new AscendActionSO m_OriginSO => (AscendActionSO)base.OriginSO; // The SO this StateAction spawned from

        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnStateEnter()
        {
            m_VerticalMovement = Mathf.Sqrt(m_OriginSO.jumpHeight * -3.0f * Physics.gravity.y);
        }

        public override void OnUpdate()
        {
            m_VerticalMovement += Physics.gravity.y * Time.deltaTime;
            //Note that even if it's added, the above value is negative due to Physics.gravity.y

            m_Protagonist.movementVector.y = m_VerticalMovement;
        }
    }
}

using UnityEngine;

namespace Strungerhulder.StateMachine.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Time elapsed")]
    public class TimeElapsedConditionSO : StateConditionSO<TimeElapsedCondition>
    {
        public float timerLength = .5f;
    }

    public class TimeElapsedCondition : Condition
    {
        private float m_StartTime;
        protected new TimeElapsedConditionSO OriginSO => (TimeElapsedConditionSO)base.OriginSO; // The SO this Condition spawned from

        public override void OnStateEnter()
        {
            m_StartTime = Time.time;
        }

        protected override bool Statement() => Time.time >= m_StartTime + OriginSO.timerLength;
    }
}

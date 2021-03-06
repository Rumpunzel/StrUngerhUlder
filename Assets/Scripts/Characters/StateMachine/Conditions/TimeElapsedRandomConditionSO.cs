using UnityEngine;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Time elapsed random")]
    public class TimeElapsedRandomConditionSO : StateConditionSO<TimeElapsedRandomCondition>
    {
        public float minTimerLength = .5f;
        public float maxTimerLength = .5f;
    }

    public class TimeElapsedRandomCondition : Condition
    {
        private float m_StartTime;
        private float m_TimerLength = .5f;
        protected new TimeElapsedRandomConditionSO OriginSO => (TimeElapsedRandomConditionSO)base.OriginSO; // The SO this Condition spawned from

        public override void OnStateEnter()
        {
            m_StartTime = Time.time;
            m_TimerLength = Random.Range(OriginSO.minTimerLength, OriginSO.maxTimerLength);
        }

        protected override bool Statement() => Time.time >= m_StartTime + m_TimerLength;
    }
}

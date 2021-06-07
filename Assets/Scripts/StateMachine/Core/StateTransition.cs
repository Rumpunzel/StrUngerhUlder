namespace Strungerhulder.StateMachines
{
    public class StateTransition : IStateComponent
    {
        private State m_TargetState;
        private StateCondition[] m_Conditions;
        private int[] m_ResultGroups;
        private bool[] m_Results;

        internal StateTransition() { }
        public StateTransition(State targetState, StateCondition[] conditions, int[] resultGroups = null)
        {
            Init(targetState, conditions, resultGroups);
        }

        internal void Init(State targetState, StateCondition[] conditions, int[] resultGroups = null)
        {
            m_TargetState = targetState;
            m_Conditions = conditions;
            m_ResultGroups = resultGroups != null && resultGroups.Length > 0 ? resultGroups : new int[1];
            m_Results = new bool[m_ResultGroups.Length];
        }

        /// <summary>
        /// Checks wether the conditions to transition to the target state are met.
        /// </summary>
        /// <param name="state">Returns the state to transition to. Null if the conditions aren't met.</param>
        /// <returns>True if the conditions are met.</returns>
        public bool TryGetTransiton(out State state)
        {
            state = ShouldTransition() ? m_TargetState : null;
            return state != null;
        }

        public void OnStateEnter()
        {
            for (int i = 0; i < m_Conditions.Length; i++)
                m_Conditions[i]._condition.OnStateEnter();
        }

        public void OnStateExit()
        {
            for (int i = 0; i < m_Conditions.Length; i++)
                m_Conditions[i]._condition.OnStateExit();
        }

        private bool ShouldTransition()
        {
#if UNITY_EDITOR
            m_TargetState.m_StateMachine.m_Debugger.TransitionEvaluationBegin(m_TargetState.m_OriginSO.name);
#endif

            int count = m_ResultGroups.Length;
            for (int i = 0, idx = 0; i < count && idx < m_Conditions.Length; i++)
                for (int j = 0; j < m_ResultGroups[i]; j++, idx++)
                    m_Results[i] = j == 0 ?
                        m_Conditions[idx].IsMet() :
                        m_Results[i] && m_Conditions[idx].IsMet();

            bool ret = false;
            for (int i = 0; i < count && !ret; i++)
                ret = ret || m_Results[i];

#if UNITY_EDITOR
            m_TargetState.m_StateMachine.m_Debugger.TransitionEvaluationEnd(ret, m_TargetState.m_Actions);
#endif

            return ret;
        }

        internal void ClearConditionsCache()
        {
            for (int i = 0; i < m_Conditions.Length; i++)
                m_Conditions[i]._condition.ClearStatementCache();
        }
    }
}

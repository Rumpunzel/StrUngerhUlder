using Strungerhulder.StateMachine.ScriptableObjects;

namespace Strungerhulder.StateMachine
{
    public class State
    {
        internal StateSO m_OriginSO;
        internal StateMachine m_StateMachine;
        internal StateTransition[] m_Transitions;
        internal StateAction[] m_Actions;

        internal State() { }

        public State(
            StateSO originSO,
            StateMachine stateMachine,
            StateTransition[] transitions,
            StateAction[] actions)
        {
            m_OriginSO = originSO;
            m_StateMachine = stateMachine;
            m_Transitions = transitions;
            m_Actions = actions;
        }

        public void OnStateEnter()
        {
            void OnStateEnter(IStateComponent[] comps)
            {
                for (int i = 0; i < comps.Length; i++)
                    comps[i].OnStateEnter();
            }
            OnStateEnter(m_Transitions);
            OnStateEnter(m_Actions);
        }

        public void OnUpdate()
        {
            for (int i = 0; i < m_Actions.Length; i++)
                m_Actions[i].OnUpdate();
        }

        public void OnStateExit()
        {
            void OnStateExit(IStateComponent[] comps)
            {
                for (int i = 0; i < comps.Length; i++)
                    comps[i].OnStateExit();
            }
            OnStateExit(m_Transitions);
            OnStateExit(m_Actions);
        }

        public bool TryGetTransition(out State state)
        {
            state = null;

            for (int i = 0; i < m_Transitions.Length; i++)
                if (m_Transitions[i].TryGetTransiton(out state))
                    break;

            for (int i = 0; i < m_Transitions.Length; i++)
                m_Transitions[i].ClearConditionsCache();

            return state != null;
        }
    }
}

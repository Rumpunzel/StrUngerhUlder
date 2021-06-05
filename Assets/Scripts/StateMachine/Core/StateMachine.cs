using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strungerhulder.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        [Tooltip("Set the initial state of this StateMachine")]
        [SerializeField] private ScriptableObjects.TransitionTableSO m_TransitionTableSO = default;

        #if UNITY_EDITOR
        [Space]
        [SerializeField]
        internal Debugging.StateMachineDebugger m_Debugger = default;
        #endif

        private readonly Dictionary<Type, Component> m_CachedComponents = new Dictionary<Type, Component>();
        internal State m_CurrentState;

        private void Awake()
        {
            m_CurrentState = m_TransitionTableSO.GetInitialState(this);

            #if UNITY_EDITOR
            m_Debugger.Awake(this);
            #endif
        }

        #if UNITY_EDITOR
        private void OnEnable()
        {
            UnityEditor.AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        private void OnAfterAssemblyReload()
        {
            m_CurrentState = m_TransitionTableSO.GetInitialState(this);
            m_Debugger.Awake(this);
        }

        private void OnDisable()
        {
            UnityEditor.AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        }
        #endif

        private void Start()
        {
            m_CurrentState.OnStateEnter();
        }

        public new bool TryGetComponent<T>(out T component) where T : Component
        {
            var type = typeof(T);
            if (!m_CachedComponents.TryGetValue(type, out var value))
            {
                if (base.TryGetComponent<T>(out component))
                    m_CachedComponents.Add(type, component);

                return component != null;
            }

            component = (T)value;
            return true;
        }

        public T GetOrAddComponent<T>() where T : Component
        {
            if (!TryGetComponent<T>(out var component))
            {
                component = gameObject.AddComponent<T>();
                m_CachedComponents.Add(typeof(T), component);
            }

            return component;
        }

        public new T GetComponent<T>() where T : Component
        {
            return TryGetComponent(out T component)
                ? component : throw new InvalidOperationException($"{typeof(T).Name} not found in {name}.");
        }

        private void Update()
        {
            if (m_CurrentState.TryGetTransition(out var transitionState))
                Transition(transitionState);

            m_CurrentState.OnUpdate();
        }

        private void Transition(State transitionState)
        {
            m_CurrentState.OnStateExit();
            m_CurrentState = transitionState;
            m_CurrentState.OnStateEnter();
        }
    }
}

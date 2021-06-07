using System.Collections.Generic;
using UnityEngine;

namespace Strungerhulder.StateMachines.ScriptableObjects
{
    public abstract class StateActionSO : ScriptableObject
    {
        /// <summary>
        /// Will create a new custom <see cref="StateAction"/> or return an existing one inside <paramref name="createdInstances"/>
        /// </summary>
        internal StateAction GetAction(StateMachine stateMachine, Dictionary<ScriptableObject, object> createdInstances)
        {
            if (createdInstances.TryGetValue(this, out var obj))
                return (StateAction)obj;

            var action = CreateAction();
            createdInstances.Add(this, action);
            action.m_OriginSO = this;
            action.Awake(stateMachine);

            return action;
        }

        protected abstract StateAction CreateAction();
    }

    public abstract class StateActionSO<T> : StateActionSO where T : StateAction, new()
    {
        protected override StateAction CreateAction() => new T();
    }
}

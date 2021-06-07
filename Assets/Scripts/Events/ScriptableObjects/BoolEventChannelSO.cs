using UnityEngine.Events;
using UnityEngine;

namespace Strungerhulder.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events that have a bool argument.
    /// Example: An event to toggle a UI interface
    /// </summary>

    [CreateAssetMenu(menuName = "Events/Bool Event Channel")]
    public class BoolEventChannelSO : ScriptableObject
    {
        public UnityAction<bool> onEventRaised;


        public void RaiseEvent(bool value)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(value);
        }

        public void UnsubscribeAll()
        {
            if (onEventRaised != null)
            {
                if (onEventRaised.GetInvocationList() != null)
                {
                    foreach (System.Delegate systemDelegate in onEventRaised.GetInvocationList())
                    {
                        onEventRaised -= systemDelegate as UnityAction<bool>;
                    }
                }
            }
        }
    }
}

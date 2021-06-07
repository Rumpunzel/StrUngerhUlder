using UnityEngine.Events;
using UnityEngine;

namespace Strungerhulder.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events that have one int argument.
    /// Example: An Achievement unlock event, where the int is the Achievement ID.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Int Event Channel")]
    public class IntEventChannelSO : EventChannelBaseSO
    {
        public UnityAction<int> onEventRaised;

        public void RaiseEvent(int value)
        {
            onEventRaised.Invoke(value);
        }
    }
}

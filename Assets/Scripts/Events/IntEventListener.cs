using UnityEngine;
using UnityEngine.Events;
using Strungerhulder.Events.ScriptableObjects;

namespace Strungerhulder.Events
{
    /// <summary>
    /// To use a generic UnityEvent type you must override the generic type.
    /// </summary>
    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }


    /// <summary>
    /// A flexible handler for int events in the form of a MonoBehaviour. Responses can be connected directly from the Unity Inspector.
    /// </summary>
    public class IntEventListener : MonoBehaviour
    {
        public IntEvent onEventRaised;

        [SerializeField] private IntEventChannelSO m_Channel = default;


        private void OnEnable()
        {
            if (m_Channel != null)
                m_Channel.onEventRaised += Respond;
        }

        private void OnDisable()
        {
            if (m_Channel != null)
                m_Channel.onEventRaised -= Respond;
        }


        private void Respond(int value)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(value);
        }
    }
}

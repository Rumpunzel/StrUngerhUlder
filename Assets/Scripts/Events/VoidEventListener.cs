using UnityEngine;
using UnityEngine.Events;
using Strungerhulder.Events.ScriptableObjects;

namespace Strungerhulder.Events
{
    /// <summary>
    /// A flexible handler for void events in the form of a MonoBehaviour. Responses can be connected directly from the Unity Inspector.
    /// </summary>
    public class VoidEventListener : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO m_Channel = default;

        public UnityEvent onEventRaised;


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


        private void Respond()
        {
            if (onEventRaised != null)
                onEventRaised.Invoke();
        }
    }
}

using UnityEngine.Events;
using UnityEngine;
using Strungerhulder.Interactions;

namespace Strungerhulder.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events to toggle the interaction UI.
    /// Example: Dispaly or hide the interaction UI via a bool and the interaction type from the enum via int
    /// </summary>

    [CreateAssetMenu(menuName = "Events/Toogle Interaction UI Event Channel")]
    public class InteractionUIEventChannelSO : ScriptableObject
    {
        public UnityAction<bool, InteractionType> onEventRaised;
        public void RaiseEvent(bool state, InteractionType interactionType)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(state, interactionType);
        }

    }
}

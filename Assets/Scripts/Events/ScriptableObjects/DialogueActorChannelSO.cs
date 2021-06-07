using UnityEngine.Events;
using UnityEngine;

namespace Strungerhulder.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for talk interaction events.
    /// Example: start talking to an actor passed as paramater
    /// </summary>
    [CreateAssetMenu(menuName = "Events/UI/Dialogue Actor Channel")]
    public class DialogueActorChannelSO : ScriptableObject
    {
        public UnityAction<ActorSO> onEventRaised;

        public void RaiseEvent(ActorSO actor)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(actor);
        }
    }
}

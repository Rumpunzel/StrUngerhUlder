using UnityEngine.Events;
using UnityEngine;
using Strungerhulder.Dialogues.ScriptableObjects;

namespace Strungerhulder.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for talk interaction events.
    /// Example: start talking to an actor passed as paramater
    /// </summary>
    [CreateAssetMenu(menuName = "Events/UI/Dialogue Actor Channel")]
    public class DialogueActorChannelSO : ScriptableObject
    {
        public UnityAction<ActorSO> OnEventRaised;

        public void RaiseEvent(ActorSO actor)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(actor);
        }
    }
}

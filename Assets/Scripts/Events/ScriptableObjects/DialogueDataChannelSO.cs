using UnityEngine.Events;
using UnityEngine;

namespace Strungerhulder.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for talk interaction events.
    /// Example: start talking to an actor passed as paramater
    /// </summary>
    [CreateAssetMenu(menuName = "Events/UI/Dialogue Data Channel")]
    public class DialogueDataChannelSO : ScriptableObject
    {
        public UnityAction<DialogueDataSO> onEventRaised;

        public void RaiseEvent(DialogueDataSO dialogue)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(dialogue);
        }
    }
}

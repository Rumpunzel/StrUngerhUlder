using UnityEngine;
using UnityEngine.Events;
using Strungerhulder.Dialogues.ScriptableObjects;

namespace Strungerhulder.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/UI/Dialogue Choice Channel")]
    public class DialogueChoiceChannelSO : ScriptableObject
    {
        public UnityAction<Choice> onEventRaised;

        public void RaiseEvent(Choice choice)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(choice);
        }
    }
}

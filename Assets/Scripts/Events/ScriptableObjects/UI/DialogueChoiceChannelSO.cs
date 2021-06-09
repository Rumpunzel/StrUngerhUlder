using UnityEngine;
using UnityEngine.Events;
using Strungerhulder.Dialogues.ScriptableObjects;

namespace Strungerhulder.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/UI/Dialogue Choice Channel")]
    public class DialogueChoiceChannelSO : ScriptableObject
    {
        public UnityAction<Choice> OnEventRaised;

        public void RaiseEvent(Choice choice)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(choice);
        }
    }
}

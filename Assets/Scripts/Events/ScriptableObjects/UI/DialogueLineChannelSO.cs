using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using Strungerhulder.Dialogues.ScriptableObjects;

namespace Strungerhulder.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/UI/Dialogue Line Channel")]
    public class DialogueLineChannelSO : ScriptableObject
    {
        public UnityAction<LocalizedString, ActorSO> OnEventRaised;

        public void RaiseEvent(LocalizedString line, ActorSO actor)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(line, actor);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
using Strungerhulder.Quests.ScriptableObjects;

namespace Strungerhulder.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/UI/Step Channel")]
    public class StepChannelSO : ScriptableObject
    {
        public UnityAction<StepSO> onEventRaised;

        public void RaiseEvent(StepSO step)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(step);
        }
    }
}

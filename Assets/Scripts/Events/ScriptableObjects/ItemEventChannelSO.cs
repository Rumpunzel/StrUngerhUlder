using UnityEngine.Events;
using UnityEngine;
using Strungerhulder.Inventory.ScriptableObjects;

namespace Strungerhulder.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Item interaction events.
    /// Example: Pick up an item passed as paramater
    /// </summary>
    [CreateAssetMenu(menuName = "Events/UI/Item Event Channel")]
    public class ItemEventChannelSO : ScriptableObject
    {
        public UnityAction<Item> onEventRaised;
        public void RaiseEvent(Item item)
        {
            if (onEventRaised != null)
                onEventRaised.Invoke(item);
        }
    }
}

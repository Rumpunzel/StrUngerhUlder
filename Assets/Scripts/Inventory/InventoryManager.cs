using UnityEngine;
using Strungerhulder.Inventory.ScriptableObjects;
using Strungerhulder.SavingAndLoading;
using Strungerhulder.Events.ScriptableObjects;

namespace Strungerhulder.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private ProtagonistInventory m_CurrentInventory = default;
        //[SerializeField] private ItemEventChannelSO m_CookRecipeEvent = default;
        [SerializeField] private ItemEventChannelSO m_UseItemEvent = default;
        [SerializeField] private ItemEventChannelSO m_EquipItemEvent = default;
        //[SerializeField] private ItemEventChannelSO m_RewardItemEvent = default;
        //[SerializeField] private ItemEventChannelSO m_GiveItemEvent = default;
        [SerializeField] ItemEventChannelSO m_AddItemEvent = default;
        [SerializeField] ItemEventChannelSO m_RemoveItemEvent = default;
        [SerializeField] private SaveSystem m_SaveSystem;


        public void UseItemEventRaised(Item item) => RemoveItem(item);
        public void EquipItemEventRaised(Item item) { }


        private void OnEnable()
        {
            //Check if the event exists to avoid errors
            //m_CookRecipeEvent.onEventRaised += CookRecipeEventRaised;
            m_UseItemEvent.onEventRaised += UseItemEventRaised;
            m_EquipItemEvent.onEventRaised += EquipItemEventRaised;
            m_AddItemEvent.onEventRaised += AddItem;
            m_RemoveItemEvent.onEventRaised += RemoveItem;
            //m_RewardItemEvent.onEventRaised += AddItem;
            //m_GiveItemEvent.onEventRaised += RemoveItem;
        }

        private void OnDisable()
        {
            //m_CookRecipeEvent.onEventRaised -= CookRecipeEventRaised;
            m_UseItemEvent.onEventRaised -= UseItemEventRaised;
            m_EquipItemEvent.onEventRaised -= EquipItemEventRaised;
            m_AddItemEvent.onEventRaised -= AddItem;
            m_RemoveItemEvent.onEventRaised -= RemoveItem;
        }


        private void AddItemWithUIUpdate(Item item)
        {
            m_CurrentInventory.Add(item);

            if (m_CurrentInventory.Contains(item))
            {
                ItemStack itemToUpdate = m_CurrentInventory.Items.Find(o => o.Item == item);
                // UIManager.Instance.UpdateInventoryScreen(itemToUpdate, false);
            }
        }

        private void RemoveItemWithUIUpdate(Item item)
        {
            ItemStack itemToUpdate = new ItemStack();

            if (m_CurrentInventory.Contains(item))
                itemToUpdate = m_CurrentInventory.Items.Find(o => o.Item == item);

            m_CurrentInventory.Remove(item);

            bool removeItem = m_CurrentInventory.Contains(item);
            //	UIManager.Instance.UpdateInventoryScreen(itemToUpdate, removeItem);

        }

        private void AddItem(Item item)
        {
            m_CurrentInventory.Add(item);
            m_SaveSystem.SaveDataToDisk();
        }

        private void RemoveItem(Item item)
        {
            m_CurrentInventory.Remove(item);
            m_SaveSystem.SaveDataToDisk();
        }
    }
}

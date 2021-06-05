using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] private Inventory m_CurrentInventory = default;
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
		//m_CookRecipeEvent.OnEventRaised += CookRecipeEventRaised;
		m_UseItemEvent.OnEventRaised += UseItemEventRaised;
		m_EquipItemEvent.OnEventRaised += EquipItemEventRaised;
		m_AddItemEvent.OnEventRaised += AddItem;
		m_RemoveItemEvent.OnEventRaised += RemoveItem;
		//m_RewardItemEvent.OnEventRaised += AddItem;
		//m_GiveItemEvent.OnEventRaised += RemoveItem;
	}

	private void OnDisable()
	{
		//m_CookRecipeEvent.OnEventRaised -= CookRecipeEventRaised;
		m_UseItemEvent.OnEventRaised -= UseItemEventRaised;
		m_EquipItemEvent.OnEventRaised -= EquipItemEventRaised;
		m_AddItemEvent.OnEventRaised -= AddItem;
		m_RemoveItemEvent.OnEventRaised -= RemoveItem;
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
		{
			itemToUpdate = m_CurrentInventory.Items.Find(o => o.Item == item);
		}

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


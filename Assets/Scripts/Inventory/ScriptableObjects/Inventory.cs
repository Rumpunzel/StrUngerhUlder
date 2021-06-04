using System.Collections.Generic;
using UnityEngine;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
	[Tooltip("The collection of items and their quantities.")]
	[SerializeField]
	private List<ItemStack> m_Items = new List<ItemStack>();

	public List<ItemStack> Items => m_Items;

	public void Add(Item item, int count = 1)
	{
		if (count <= 0)
			return;

		for (int i = 0; i < m_Items.Count; i++)
		{
			ItemStack currentItemStack = m_Items[i];
			if (item == currentItemStack.Item)
			{
				//only add to the amount if the item is usable 
				if (currentItemStack.Item.ItemType.ActionType == ItemInventoryActionType.use)
				{
					currentItemStack.Amount += count;
				}

				return;
			}
		}

		m_Items.Add(new ItemStack(item, count));
	}

	public void Remove(Item item, int count = 1)
	{
		if (count <= 0)
			return;

		for (int i = 0; i < m_Items.Count; i++)
		{
			ItemStack currentItemStack = m_Items[i];

			if (currentItemStack.Item == item)
			{
				currentItemStack.Amount -= count;

				if (currentItemStack.Amount <= 0)
					m_Items.Remove(currentItemStack);

				return;
			}
		}
	}

	public bool Contains(Item item)
	{
		for (int i = 0; i < m_Items.Count; i++)
		{
			if (item == m_Items[i].Item)
			{
				return true;
			}
		}

		return false;
	}

	public int Count(Item item)
	{
		for (int i = 0; i < m_Items.Count; i++)
		{
			ItemStack currentItemStack = m_Items[i];
			
			if (item == currentItemStack.Item)
			{
				return currentItemStack.Amount;
			}
		}

		return 0;
	}

	public bool[] IngredientsAvailability(List<ItemStack> ingredients)
	{
		bool[] availabilityArray = new bool[ingredients.Count];

		for (int i = 0; i < ingredients.Count; i++)
		{
			availabilityArray[i] = m_Items.Exists(o => o.Item == ingredients[i].Item && o.Amount >= ingredients[i].Amount);

		}

		return availabilityArray;
	}

	public bool hasIngredients(List<ItemStack> ingredients)
	{
		return !ingredients.Exists(j => !m_Items.Exists(o => o.Item == j.Item && o.Amount >= j.Amount)); ;
	}
}

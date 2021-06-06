using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class UIInventory : MonoBehaviour
{
    public UnityAction closed;

	[SerializeField] private Inventory m_CurrentInventory = default;

	[SerializeField] private UIInventoryItem m_ItemPrefab = default;

	[SerializeField] private GameObject m_ContentParent = default;

	[FormerlySerializedAs("_inspectorFiller")]
	[SerializeField] private UIInventoryInspector m_InspectorPanel = default;

	[FormerlySerializedAs("_tabFiller")]
	[SerializeField] private UIInventoryTabs m_TabsPanel = default;

	[FormerlySerializedAs("_buttonFiller")]
	[SerializeField] private UIInventoryActionButton m_ActionButton = default;

	[SerializeField] private List<InventoryTabSO> m_TabTypesList = new List<InventoryTabSO>();

	[FormerlySerializedAs("_instanciatedItems")]
	[SerializeField] private List<UIInventoryItem> m_AvailableItemSlots = default;

	[SerializeField] private VoidEventChannelSO m_OnInteractionEndedEvent = default;

	[SerializeField] private ItemEventChannelSO m_UseItemEvent = default;
	[SerializeField] private ItemEventChannelSO m_EquipItemEvent = default;
	//[SerializeField] private ItemEventChannelSO m_CookRecipeEvent = default;

	[SerializeField] private InputReader m_InputReader = default;

    private InventoryTabSO m_SelectedTab = default;
    private int m_SelectedItemId = -1;
	private bool m_IsNearPot = false;


    public void FillInventory(InventoryTabType m_SelectedTabType = InventoryTabType.CookingItem, bool isNearPot = false)
    {
        m_IsNearPot = isNearPot;

        if ((m_TabTypesList.Exists(o => o.TabType == m_SelectedTabType)))
        {
            m_SelectedTab = m_TabTypesList.Find(o => o.TabType == m_SelectedTabType);
        }
        else
        {
            if (m_TabTypesList != null)
            {
                if (m_TabTypesList.Count > 0)
                    m_SelectedTab = m_TabTypesList[0];
            }

        }

        if (m_SelectedTab != null)
        {
            SetTabs(m_TabTypesList, m_SelectedTab);
            List<ItemStack> listItemsToShow = new List<ItemStack>();
            listItemsToShow = m_CurrentInventory.Items.FindAll(o => o.Item.ItemType.TabType == m_SelectedTab);

            FillInvetoryItems(listItemsToShow);
        }
        else
        {
            Debug.LogError("There's no selected tab ");
        }
    }

    public void InspectItem(Item itemToInspect)
    {
        if (m_AvailableItemSlots.Exists(o => o.currentItem.Item == itemToInspect))
        {
            int itemIndex = m_AvailableItemSlots.FindIndex(o => o.currentItem.Item == itemToInspect);

            //unselect selected Item
            if (m_SelectedItemId >= 0 && m_SelectedItemId != itemIndex)
                UnselectItem(m_SelectedItemId);

            //change Selected ID 
            m_SelectedItemId = itemIndex;

            //show Information
            ShowItemInformation(itemToInspect);

            //check if interactable
            bool isInteractable = true;
            m_ActionButton.gameObject.SetActive(true);

            if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.cook)
            {
                //isInteractable = m_CurrentInventory.hasIngredients(itemToInspect.IngredientsList) && m_IsNearPot;
            }
            else if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.doNothing)
            {
                isInteractable = false;
                m_ActionButton.gameObject.SetActive(false);
            }

            //set button
            m_ActionButton.FillInventoryButton(itemToInspect.ItemType, isInteractable);
        }
    }
    public void CloseInventory()
    {
        closed.Invoke();
    }


	private void OnEnable()
	{
		//Check if the event exists to avoid errors
		m_ActionButton.clicked += OnActionButtonClicked;
		m_TabsPanel.tabChanged += OnChangeTab;

		m_OnInteractionEndedEvent.onEventRaised += InteractionEnded;

		foreach (UIInventoryItem item in m_AvailableItemSlots)
		{
            item.itemSelected += InspectItem;
		}

		m_InputReader.TabSwitched += OnSwitchTab;
	}

	private void OnDisable()
	{
		m_ActionButton.clicked -= OnActionButtonClicked;
		m_TabsPanel.tabChanged -= OnChangeTab;

        foreach (UIInventoryItem item in m_AvailableItemSlots)
        {
            item.itemSelected -= InspectItem;
        }

		m_InputReader.TabSwitched -= OnSwitchTab;
	}

	private void OnSwitchTab(float orientation)
	{
		if (orientation != 0)
		{
			bool isLeft = orientation < 0;
			int initialIndex = m_TabTypesList.FindIndex(o => o == m_SelectedTab);

			if (initialIndex != -1)
			{
				initialIndex += isLeft ? -1 : 1;
				initialIndex = Mathf.Clamp(initialIndex, 0, m_TabTypesList.Count - 1);
			}

			OnChangeTab(m_TabTypesList[initialIndex]);
		}
	}

	private void InteractionEnded() => m_IsNearPot = false;

	private void SetTabs(List<InventoryTabSO> typesList, InventoryTabSO selectedType)
	{
		m_TabsPanel.SetTabs(typesList, selectedType);
	}

	private void FillInvetoryItems(List<ItemStack> listItemsToShow)
	{
		if (m_AvailableItemSlots == null)
			m_AvailableItemSlots = new List<UIInventoryItem>();

		int maxCount = Mathf.Max(listItemsToShow.Count, m_AvailableItemSlots.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < listItemsToShow.Count)
			{
				//fill
				bool isSelected = m_SelectedItemId == i;
				m_AvailableItemSlots[i].SetItem(listItemsToShow[i], isSelected);
			}
			else if (i < m_AvailableItemSlots.Count)
			{
				//Desactive
				m_AvailableItemSlots[i].SetInactiveItem();
			}

		}

		HideItemInformation();

		//unselect selected Item
		if (m_SelectedItemId >= 0)
		{
			UnselectItem(m_SelectedItemId);
			m_SelectedItemId = -1;
		}
		//hover First Element
		if (m_AvailableItemSlots.Count > 0)
			m_AvailableItemSlots[0].SelectFirstElement();
	}

	private void UpdateItemInInventory(ItemStack itemToUpdate, bool removeItem)
	{
		if (m_AvailableItemSlots == null)
			m_AvailableItemSlots = new List<UIInventoryItem>();

		if (removeItem)
		{
			if (m_AvailableItemSlots.Exists(o => o.currentItem == itemToUpdate))
			{
				int index = m_AvailableItemSlots.FindIndex(o => o.currentItem == itemToUpdate);
				m_AvailableItemSlots[index].SetInactiveItem();
			}

		}
		else
		{
			int index = 0;
			//if the item has already been created
			if (m_AvailableItemSlots.Exists(o => o.currentItem == itemToUpdate))
			{
				index = m_AvailableItemSlots.FindIndex(o => o.currentItem == itemToUpdate);
			}
			//if the item needs to be created
			else
			{
				//if the new item needs to be instantiated
				if (m_CurrentInventory.Items.Count > m_AvailableItemSlots.Count)
				{
					//instantiate 
					UIInventoryItem instantiatedPrefab = Instantiate(m_ItemPrefab, m_ContentParent.transform) as UIInventoryItem;
					m_AvailableItemSlots.Add(instantiatedPrefab);
				}
				//find the last instantiated game object not used
				index = m_CurrentInventory.Items.Count;
			}

			//set item
			bool isSelected = m_SelectedItemId == index;
			m_AvailableItemSlots[index].SetItem(itemToUpdate, isSelected);
		}
	}

	private void ShowItemInformation(Item item)
	{
		//bool[] availabilityArray = m_CurrentInventory.IngredientsAvailability(item.IngredientsList);

		//m_InspectorPanel.FillInspector(item, availabilityArray);
		m_InspectorPanel.gameObject.SetActive(true);
	}

	private void HideItemInformation()
	{
		m_ActionButton.gameObject.SetActive(false);
		m_InspectorPanel.gameObject.SetActive(false);
	}

	private void UnselectItem(int itemIndex)
	{
		if (m_AvailableItemSlots.Count > itemIndex)
		{
			m_AvailableItemSlots[itemIndex].UnselectItem();
		}
	}

    private void UpdateInventory()
	{
		FillInventory(m_SelectedTab.TabType);
	}

    private void OnActionButtonClicked()
	{
		//find the selected Item
		if (m_AvailableItemSlots.Count > m_SelectedItemId && m_SelectedItemId > -1)
		{
			//find the item 
			Item itemToActOn = new Item();
			itemToActOn = m_AvailableItemSlots[m_SelectedItemId].currentItem.Item;

			//check the selected Item type
			//call action function depending on the itemType
			switch (itemToActOn.ItemType.ActionType)
			{
				case ItemInventoryActionType.cook:
					CookRecipe(itemToActOn);
					break;
				case ItemInventoryActionType.use:
					UseItem(itemToActOn);
					break;
				case ItemInventoryActionType.equip:
					EquipItem(itemToActOn);
					break;
				default:

					break;
			}
		}
	}

    private void UseItem(Item itemToUse)
	{
		Debug.Log("USE ITEM " + itemToUse.name);

		m_UseItemEvent.onEventRaised(itemToUse);
		//update inventory
		UpdateInventory();
	}


    private void EquipItem(Item itemToUse)
	{
		Debug.Log("Equip ITEM " + itemToUse.name);
		m_EquipItemEvent.onEventRaised(itemToUse);
	}

    private void CookRecipe(Item recipeToCook)
	{
		//get item
		//m_CookRecipeEvent.onEventRaised(recipeToCook);

		//update inspector
		InspectItem(recipeToCook);

		//update inventory
		UpdateInventory();
	}

    private void OnChangeTab(InventoryTabSO tabType)
	{
		FillInventory(tabType.TabType);
	}
}

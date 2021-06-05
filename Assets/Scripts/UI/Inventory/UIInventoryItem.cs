using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Events;

public class UIInventoryItem : MonoBehaviour
{
    [HideInInspector] public ItemStack currentItem = default;

    public UnityAction<Item> itemSelected;

	[SerializeField] private Image m_ItemPreviewImage = default;
	[SerializeField] private TextMeshProUGUI m_ItemCount = default;
	[SerializeField] private Image m_BackgroundImage = default;
	[SerializeField] private Image m_ImageHover = default;
	[SerializeField] private Image m_ImageSelected = default;
	[SerializeField] private Button m_ItemButton = default;
	[SerializeField] private Image m_BackgroundInactiveImage = default;
	[SerializeField] private LocalizeSpriteEvent m_BackgroundLocalizedImage = default;


	public void SetItem(ItemStack itemStack, bool isSelected)
	{
		m_ItemPreviewImage.gameObject.SetActive(true);
		m_ItemCount.gameObject.SetActive(true);
		m_BackgroundImage.gameObject.SetActive(true);
		m_ImageHover.gameObject.SetActive(true);
		m_ImageSelected.gameObject.SetActive(true);
		m_ItemButton.gameObject.SetActive(true);
		m_BackgroundInactiveImage.gameObject.SetActive(false);

		UnhoverItem();
		currentItem = itemStack;

		m_ImageSelected.gameObject.SetActive(isSelected);

		if (itemStack.Item.IsLocalized)
		{
			m_BackgroundLocalizedImage.enabled = true;
			m_BackgroundLocalizedImage.AssetReference = itemStack.Item.LocalizePreviewImage;
		}
		else
		{
			m_BackgroundLocalizedImage.enabled = false;
			m_ItemPreviewImage.sprite = itemStack.Item.PreviewImage;
		}

		m_ItemCount.text = itemStack.Amount.ToString();
		m_BackgroundImage.color = itemStack.Item.ItemType.TypeColor;
	}

	public void SetInactiveItem()
	{
		UnhoverItem();
		currentItem = null;
		m_ItemPreviewImage.gameObject.SetActive(false);
		m_ItemCount.gameObject.SetActive(false);
		m_BackgroundImage.gameObject.SetActive(false);
		m_ImageHover.gameObject.SetActive(false);
		m_ImageSelected.gameObject.SetActive(false);
		m_ItemButton.gameObject.SetActive(false);
		m_BackgroundInactiveImage.gameObject.SetActive(true);
	}

	public void SelectFirstElement()
	{
		m_ItemButton.Select();
		SelectItem();
	}

	public void HoverItem()
	{
		m_ImageHover.gameObject.SetActive(true);
	}

	public void UnhoverItem()
	{
		m_ImageHover.gameObject.SetActive(false);
	}

	public void SelectItem()
	{
		if (itemSelected != null && currentItem != null && currentItem.Item != null)
		{
			m_ImageSelected.gameObject.SetActive(true);
			itemSelected.Invoke(currentItem.Item);
		}
		else
		{
			m_ImageSelected.gameObject.SetActive(false);
		}

	}

	public void UnselectItem()
	{
		m_ImageSelected.gameObject.SetActive(false);
	}
}

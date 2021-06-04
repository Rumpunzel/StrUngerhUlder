using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : SerializableScriptableObject
{
	[Tooltip("The name of the item")]
	[SerializeField] private LocalizedString m_Name = default;

	[Tooltip("A preview image for the item")]
	[SerializeField] private Sprite m_PreviewImage = default;

	[Tooltip("A description of the item")]
	[SerializeField] private LocalizedString m_Description = default;


	[Tooltip("The type of item")]
	[SerializeField] private ItemType m_ItemType = default;

	[Tooltip("A prefab reference for the model of the item")]
	[SerializeField] private GameObject m_Prefab = default;

    [Tooltip("The number of items able to stack into on in the inventory")]
    [SerializeField] private int m_StackSize = 1;

	
	[SerializeField] private bool m_IsLocalized = default;
	[Tooltip("A localized preview image for the item")]
	[SerializeField] private LocalizedSprite m_LocalizePreviewImage = default;

	public LocalizedString Name => m_Name;
	public Sprite PreviewImage => m_PreviewImage;
	public LocalizedString Description => m_Description;
	public ItemType ItemType => m_ItemType;
	public GameObject Prefab => m_Prefab;
    public int StackSize => m_StackSize;

	public bool IsLocalized => m_IsLocalized;
	public LocalizedSprite LocalizePreviewImage => m_LocalizePreviewImage;
}

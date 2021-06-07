using UnityEngine;
using UnityEngine.Localization;
// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

namespace Strungerhulder.Inventory.ScriptableObjects
{
    public enum ItemInventoryType
    {
        recipe,
        utensil,
        ingredient,
        customisation,
        dish,
    }
    public enum ItemInventoryActionType
    {
        cook,
        use,
        equip,
        doNothing,
    }

    [CreateAssetMenu(fileName = "ItemType", menuName = "Inventory/ItemType", order = 51)]
    public class ItemType : ScriptableObject
    {
        [Tooltip("The action associated with the item type")]
        [SerializeField]
        private LocalizedString m_ActionName = default;

        [Tooltip("The action associated with the item type")]
        [SerializeField]
        private LocalizedString m_TypeName = default;

        [Tooltip("The Item's background color in the UI")]
        [SerializeField] private Color m_TypeColor = default;
        [Tooltip("The Item's type")]
        [SerializeField] private ItemInventoryType m_Type = default;

        [Tooltip("The Item's action type")]
        [SerializeField] private ItemInventoryActionType m_ActionType = default;


        [Tooltip("The tab type under which the item will be added")]
        [SerializeField] private InventoryTabSO m_TabType = default;

        public LocalizedString ActionName => m_ActionName;
        public LocalizedString TypeName => m_TypeName;
        public Color TypeColor => m_TypeColor;
        public ItemInventoryActionType ActionType => m_ActionType;
        public ItemInventoryType Type => m_Type;
        public InventoryTabSO TabType => m_TabType;
    }
}

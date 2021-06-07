using UnityEngine;
using UnityEngine.Localization;
// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/

namespace Strungerhulder.Inventories.ScriptableObjects
{
    public enum InventoryTabType
    {
        Customization,
        CookingItem,
        Recipe,
    }

    [CreateAssetMenu(fileName = "InventoryTabType", menuName = "Inventory/Inventory Tab Type")]
    public class InventoryTabSO : ScriptableObject
    {
        [Tooltip("The tab Name that will be displayed in the inventory")]
        [SerializeField]
        private LocalizedString m_TabName = default;

        [Tooltip("The tab Picture that will be displayed in the inventory")]
        [SerializeField]
        private Sprite m_TabIcon = default;

        [Tooltip("The tab type used to reference the item")]
        [SerializeField] private InventoryTabType m_TabType = default;


        public LocalizedString TabName => m_TabName;
        public Sprite TabIcon => m_TabIcon;
        public InventoryTabType TabType => m_TabType;
    }
}

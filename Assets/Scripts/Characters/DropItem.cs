using System;
using UnityEngine;
using Strungerhulder.Inventories.ScriptableObjects;

namespace Strungerhulder.Characters
{
    [Serializable]
    public class DropItem
    {
        [SerializeField]
        Item m_Item;

        [SerializeField]
        float m_ItemDropRate;

        public Item Item => m_Item;
        public float ItemDropRate => m_ItemDropRate;
    }
}

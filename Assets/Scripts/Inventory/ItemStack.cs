using System;
using UnityEngine;
using Strungerhulder.Inventories.ScriptableObjects;

namespace Strungerhulder.Inventories
{
    [Serializable]
    public class ItemStack
    {
        [SerializeField]
        private Item _item;

        public Item Item => _item;

        public int Amount;
        public ItemStack()
        {
            _item = null;
            Amount = 0;
        }
        public ItemStack(Item item, int amount)
        {
            _item = item;
            Amount = amount;
        }
    }
}

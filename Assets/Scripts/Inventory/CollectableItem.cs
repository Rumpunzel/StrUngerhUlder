using UnityEngine;
using Strungerhulder.Inventories.ScriptableObjects;

namespace Strungerhulder.Inventories
{
    public class CollectableItem : MonoBehaviour
    {
        [SerializeField] private Item m_CurrentItem = default;
        [SerializeField] private GameObject m_ItemGameObject = default;

        private void Start()
        {
            AnimateItem();
        }

        public Item GetItem()
        {
            return m_CurrentItem;
        }

        public void SetItem(Item item)
        {
            m_CurrentItem = item;
        }

        public void AnimateItem()
        {
            if (m_ItemGameObject != null)
            {
                //m_ItemGameObject.transform.DORotate(Vector3.one * 180, 5, RotateMode.Fast).SetLoops(-1, LoopType.Incremental);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using Strungerhulder.Inventories.ScriptableObjects;

namespace Strungerhulder.UI.Iventory
{
    public class UIInspectorPreview : MonoBehaviour
    {
        [SerializeField] private Image m_PreviewImage = default;


        public void FillPreview(Item ItemToInspect)
        {
            m_PreviewImage.gameObject.SetActive(true);
            m_PreviewImage.sprite = ItemToInspect.PreviewImage;
        }
    }
}

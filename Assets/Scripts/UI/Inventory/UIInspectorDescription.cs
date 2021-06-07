using UnityEngine;
using UnityEngine.Localization.Components;
using Strungerhulder.Inventory.ScriptableObjects;

namespace Strungerhulder.UI.Iventory
{
    public class UIInspectorDescription : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent m_TextDescription = default;
        [SerializeField] private LocalizeStringEvent m_TextName = default;

        public void FillDescription(Item itemToInspect)
        {
            m_TextName.StringReference = itemToInspect.Name;
            m_TextName.StringReference.Arguments = new[] { new { Purpose = 0, Amount = 1 } };
            m_TextDescription.StringReference = itemToInspect.Description;

            m_TextName.gameObject.SetActive(true);
            m_TextDescription.gameObject.SetActive(true);
        }
    }
}

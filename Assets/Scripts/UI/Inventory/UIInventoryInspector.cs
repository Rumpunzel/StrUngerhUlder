using UnityEngine;
using Strungerhulder.Inventories.ScriptableObjects;

namespace Strungerhulder.UI.Iventory
{
    public class UIInventoryInspector : MonoBehaviour
    {
        [SerializeField] private UIInspectorDescription m_InspectorDescription = default;
        [SerializeField] private UIInspectorIngredients m_RecipeIngredients = default;


        public void FillInspector(Item itemToInspect, bool[] availabilityArray = null)
        {
            bool isForCooking = (itemToInspect.ItemType.ActionType == ItemInventoryActionType.cook);

            m_InspectorDescription.FillDescription(itemToInspect);

            if (isForCooking)
            {
                //m_RecipeIngredients.FillIngredients(itemToInspect.IngredientsList, availabilityArray);
                m_RecipeIngredients.gameObject.SetActive(true);
            }
            else
                m_RecipeIngredients.gameObject.SetActive(false);
        }
    }
}

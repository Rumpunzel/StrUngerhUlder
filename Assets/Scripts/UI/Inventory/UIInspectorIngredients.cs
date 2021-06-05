using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIInspectorIngredients : MonoBehaviour
{
	[SerializeField] private List<UIInspectorIngredientFiller> m_InstantiatedGameObjects = new List<UIInspectorIngredientFiller>();


	public void FillIngredients(List<ItemStack> listofIngredients, bool[] availabilityArray)
	{
		int maxCount = Mathf.Max(listofIngredients.Count, m_InstantiatedGameObjects.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < listofIngredients.Count)
			{
				if (i >= m_InstantiatedGameObjects.Count)
				{
					//Do nothing, maximum ingredients for a recipe reached
					Debug.Log("Maximum ingredients reached");
				}
				else
				{
					//fill
					bool isAvailable = availabilityArray[i];
					m_InstantiatedGameObjects[i].FillIngredient(listofIngredients[i], isAvailable);

					m_InstantiatedGameObjects[i].gameObject.SetActive(true);
				}
			}
			else if (i < m_InstantiatedGameObjects.Count)
			{
				//Desactive
				m_InstantiatedGameObjects[i].gameObject.SetActive(false);
			}
		}
	}
}

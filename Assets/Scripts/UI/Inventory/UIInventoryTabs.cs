using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIInventoryTabs : MonoBehaviour
{
    public UnityAction<InventoryTabSO> tabChanged;

	[FormerlySerializedAs("instantiatedGameObjects")]
	[SerializeField] private List<UIInventoryTab> m_InstantiatedGameObjects;

	private bool m_CanDisableLayout = false;


	public void SetTabs(List<InventoryTabSO> typesList, InventoryTabSO selectedType)
	{
		if (m_InstantiatedGameObjects == null)
			m_InstantiatedGameObjects = new List<UIInventoryTab>();

		if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
			gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;


		int maxCount = Mathf.Max(typesList.Count, m_InstantiatedGameObjects.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < typesList.Count)
			{
				if (i >= m_InstantiatedGameObjects.Count)
					Debug.LogError("Maximum tabs reached");

				bool isSelected = typesList[i] == selectedType;
				//fill
				m_InstantiatedGameObjects[i].SetTab(typesList[i], isSelected);
				m_InstantiatedGameObjects[i].gameObject.SetActive(true);
				m_InstantiatedGameObjects[i].tabClicked += ChangeTab;
			}
			else if (i < m_InstantiatedGameObjects.Count)
			{
				//Desactive
				m_InstantiatedGameObjects[i].gameObject.SetActive(false);
			}
		}
		if (isActiveAndEnabled) // check if the game object is active and enabled so that we could start the coroutine. 
			StartCoroutine(waitBeforeDesactiveLayout());
		else // if the game object is inactive, disabling the layout will happen on onEnable 
			m_CanDisableLayout = true;
	}

	public void ChangeTabSelection(InventoryTabSO selectedType)
	{
		for (int i = 0; i < m_InstantiatedGameObjects.Count; i++)
		{
			bool isSelected = m_InstantiatedGameObjects[i].currentTabType == selectedType;
			//fill
			m_InstantiatedGameObjects[i].UpdateState(isSelected);
		}
	}


    private IEnumerator waitBeforeDesactiveLayout()
    {
        yield return new WaitForSeconds(1);
        //disable layout group after layout calculation
        if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
        {
            gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
            m_CanDisableLayout = false;
        }
    }

    private void OnEnable()
    {
        if ((gameObject.GetComponent<VerticalLayoutGroup>() != null) && m_CanDisableLayout)
        {
            gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
            m_CanDisableLayout = false;
        }
    }

	private void OnDisable()
	{
		foreach (UIInventoryTab tab in m_InstantiatedGameObjects)
		{
            tab.tabClicked -= ChangeTab;
		}
	}

	private void ChangeTab(InventoryTabSO newTabType)
	{
		tabChanged.Invoke(newTabType);
	}
}

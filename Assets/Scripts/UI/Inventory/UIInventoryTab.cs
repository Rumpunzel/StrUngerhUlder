using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIInventoryTab : MonoBehaviour
{
    [HideInInspector] public InventoryTabSO currentTabType = default;

    public UnityAction<InventoryTabSO> tabClicked;

	[SerializeField] private Image m_TabImage = default;

	[SerializeField] private Button m_ActionButton = default;

	[SerializeField] private Color m_SelectedIconColor = default;
	[SerializeField] private Color m_DeselectedIconColor = default;


	public void SetTab(InventoryTabSO tabType, bool isSelected)
	{
		currentTabType = tabType;
		m_TabImage.sprite = tabType.TabIcon;
		UpdateState(isSelected);
	}

	public void UpdateState(bool isSelected)
	{
		m_ActionButton.interactable = !isSelected;

		if (isSelected)
			m_TabImage.color = m_SelectedIconColor;
		else
		m_TabImage.color = m_DeselectedIconColor;
	}

	public void ClickButton() => tabClicked.Invoke(currentTabType);
}

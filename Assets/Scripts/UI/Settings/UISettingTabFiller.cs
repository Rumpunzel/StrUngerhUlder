using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using TMPro;

public class UISettingTabFiller : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent m_LocalizedTabTitle;
	[SerializeField] private Image m_BackgroundSelectedTab;
	[SerializeField] private Color m_ColorSelectedTab;
	[SerializeField] private Color m_ColorUnselectedTab;

	private SettingTabType m_CurrentTabType; 


	public void SetTab(SettingTab settingTab, bool isSelected)
	{
		m_LocalizedTabTitle.StringReference = settingTab.title;
		m_CurrentTabType = settingTab.settingTabsType; 

		if (isSelected)
            SelectTab();
		else
            UnselectTab();
	}

	public void SetTab(SettingTabType tabType)
	{
		bool isSelected = (m_CurrentTabType == tabType);

		if (isSelected)
            SelectTab();
		else
			UnselectTab();
	}


	private void SelectTab()
	{
		m_BackgroundSelectedTab.enabled=true;
		m_LocalizedTabTitle.GetComponent<TextMeshProUGUI>().color = m_ColorSelectedTab;
	}

	private void UnselectTab()
	{
		m_BackgroundSelectedTab.enabled = false;
		m_LocalizedTabTitle.GetComponent<TextMeshProUGUI>().color = m_ColorUnselectedTab; 
	}
}

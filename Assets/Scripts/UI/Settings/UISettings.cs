using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Serialization;

[System.Serializable]
public enum SettingTabType
{
	Language,
	Audio,
	Graphics
}

[System.Serializable]
public enum SettingFieldType
{
	Language,
	Volume_SFx,
	Volume_Music,
	Resolution,
	FullScreen,
	GraphicQuality,
	AntiAliasing,
	Shadow,
}

[System.Serializable]
public class SettingTab
{
	public SettingTabType settingTabsType;
	public LocalizedString title;
}

[System.Serializable]
public class SettingField
{
	public SettingTabType settingTabsType;
	public SettingFieldType settingFieldType;
	public LocalizedString title;
}

public class UISettings : MonoBehaviour
{
    public UnityAction closed;

	[SerializeField] private List<SettingTab> m_SettingTabsList = new List<SettingTab>();
	[SerializeField] private UISettingTabsFiller m_SettingTabFiller = default;
	[SerializeField] private List<SettingField> m_SettingFieldsList = default;
	[SerializeField] private UISettingFieldsFiller m_SettingFieldsFiller = default;

	[SerializeField] private InputReader m_InputReader = default;


	private void OnEnable()
	{
		m_InputReader.menuCloseEvent += CloseScreen;
	}

	private void OnDisable()
	{
		m_InputReader.menuCloseEvent -= CloseScreen;
	}


	public void SetSettingsScreen()
	{
		if (m_SettingTabsList.Count > 0)
		{
			SetTabs();
			SettingTabType defaultTabType = m_SettingTabsList[0].settingTabsType;
			SelectTab(defaultTabType);
		}
	}

    public void UnselectField() { }
    public void NextOption() { }
    public void PreviousOption() { }
    public void OpenValidateChoicesPrompt() { }
    public void ValidateChoices() { }
    public void CloseScreen() => closed.Invoke();


	private void SetTabs()
	{
		m_SettingTabFiller.FillTabs(m_SettingTabsList);
	}

    private void SelectTab(SettingTabType selectedTab)
	{
		m_SettingTabFiller.SelectTab(selectedTab);
		SetFields(selectedTab);
	}

    private void SetFields(SettingTabType selectedTab)
	{
		List<SettingField> fields = m_SettingFieldsList.FindAll(o => o.settingTabsType == selectedTab);
		m_SettingFieldsFiller.FillFields(fields);
	}
    private void SelectField() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UISettingFieldsFiller : MonoBehaviour
{
	[SerializeField] private UISettingItemFiller[] m_SettingfieldsList = default;
	[SerializeField] private InputReader m_InputReader = default; 


	public void FillFields(List<SettingField> settingItems)
	{
		for (int i = 0; i < m_SettingfieldsList.Length; i++)
		{
			if(i < settingItems.Count)
			{
				SetField(settingItems[i], m_SettingfieldsList[i]);
				m_SettingfieldsList[i].gameObject.SetActive(true);
			}
			else
			{
				m_SettingfieldsList[i].gameObject.SetActive(false);
			}
		}
	}
	

	public void SetField(SettingField field, UISettingItemFiller uiField)
	{
		int paginationCount=0;
		int selectedPaginationIndex=0;
		string selectedOption=default;
		LocalizedString fieldTitle=field.title;
		SettingFieldType fieldType= field.settingFieldType;
		
		switch (field.settingFieldType)
		{
			case SettingFieldType.Language:
				paginationCount = LocalizationSettings.AvailableLocales.Locales.Count;
				selectedPaginationIndex = LocalizationSettings.AvailableLocales.Locales.FindIndex(o => o == LocalizationSettings.SelectedLocale);
				selectedOption = LocalizationSettings.SelectedLocale.LocaleName; 
				break;

			case SettingFieldType.AntiAliasing:
				break;

			case SettingFieldType.FullScreen:
				selectedPaginationIndex = IsFullscreen();
				paginationCount = 2;

				if (Screen.fullScreen)
					selectedOption = "On";
				else
					selectedOption = "Off";
				break;

			case SettingFieldType.GraphicQuality:
				selectedPaginationIndex = QualitySettings.GetQualityLevel(); 
				paginationCount = 6;
				selectedOption = QualitySettings.names[QualitySettings.GetQualityLevel()]; 
				break;

			case SettingFieldType.Resolution:
				break;

			case SettingFieldType.Shadow:
				break;

			case SettingFieldType.Volume_Music:
			case SettingFieldType.Volume_SFx:
				 paginationCount = 10;
				 selectedPaginationIndex = 5;
				 selectedOption = "5"; 
				 break;
		}

		uiField.FillSettingField(paginationCount, selectedPaginationIndex, selectedOption, fieldTitle, fieldType); 
	}


	private string GetQualityLevelTitle()
	{
		string title = ""; 

		switch (QualitySettings.GetQualityLevel())
		{
			case (int) QualityLevel.Beautiful:
				title = QualityLevel.Beautiful.ToString(); 
				break;

			case (int)QualityLevel.Fantastic:
				title = QualityLevel.Fantastic.ToString();
				break;

			case (int)QualityLevel.Fast:
				title = QualityLevel.Fast.ToString();
				break;

			case (int)QualityLevel.Fastest:
				title = QualityLevel.Fastest.ToString();
				break;

			case (int)QualityLevel.Good:
				title = QualityLevel.Good.ToString();
				break;

			case (int)QualityLevel.Simple:
				title = QualityLevel.Simple.ToString();
				break;
		}

		return title;
	}

    private int IsFullscreen() => Screen.fullScreen ? 0 : 1;
    private void NextField() { }
    private void PreviousField() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization; 
using UnityEngine.Events;

public class UISettingItemFiller : MonoBehaviour
{
    public event UnityAction nextOption = delegate { };
    public event UnityAction previousOption = delegate { };

	[SerializeField] private UIPaginationFiller m_Pagination = default;
	[SerializeField] private TextMeshProUGUI m_CurrentSelectedOption = default;

	[SerializeField] private Image m_Background = default;
	[SerializeField] private LocalizeStringEvent m_Title = default;

	[SerializeField] private Color m_ColorSelected = default;
	[SerializeField] private Color m_ColorUnselected = default;

	[SerializeField] private Sprite m_BackgroundSelected = default;
	[SerializeField] private Sprite m_BackgroundUnselected = default;

	private SettingFieldType m_FieldType = default;

	
	public void FillSettingField(int paginationCount, int selectedPaginationIndex, string selectedOption, LocalizedString fieldTitle, SettingFieldType fieldType)
	{
		m_FieldType = fieldType; 
		m_Pagination.SetPagination(paginationCount, selectedPaginationIndex);
		m_CurrentSelectedOption.text = selectedOption;
		m_Title.StringReference= fieldTitle; 
	}

	public void FillSettingNewOption(int selectedPaginationIndex, string selectedOption)
	{
		m_Pagination.SetCurrentPagination(selectedPaginationIndex);
		m_CurrentSelectedOption.text = selectedOption;
	}

	public void SelectItem()
	{
		m_Background.sprite = m_BackgroundSelected;
		m_Title.GetComponent<TextMeshProUGUI>().color = m_ColorSelected;
		m_CurrentSelectedOption.color = m_ColorSelected;
	}

	public void UnselectItem()
	{
		m_Background.sprite = m_BackgroundUnselected;
		m_Title.GetComponent<TextMeshProUGUI>().color = m_ColorUnselected;
		m_CurrentSelectedOption.color = m_ColorUnselected;
	}

	public void NextOption() => nextOption.Invoke(); 
	public void PreviousOption() => previousOption.Invoke();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using TMPro;

public class UIInspectorIngredientFiller : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_IngredientAmount = default;

	[SerializeField] private GameObject m_AvailableCheckMark = default;
	[SerializeField] private GameObject m_UnavailableCheckMark = default;

	[SerializeField] private GameObject m_Tooltip = default;

	[SerializeField] private LocalizeStringEvent m_TooltipMessage = default;

	[SerializeField] private Image m_IngredientIcon = default;

	[SerializeField] private Color m_TextColorAvailable = default;
	[SerializeField] private Color m_TextColorUnavailable = default;


	public void FillIngredient(ItemStack ingredient, bool isAvailable)
	{
		if (isAvailable)
		{
			m_IngredientAmount.color = m_TextColorAvailable;
		}
		else
		{
			m_IngredientAmount.color = m_TextColorUnavailable;
		}

		m_IngredientAmount.text = ingredient.Amount.ToString();
		m_TooltipMessage.StringReference = ingredient.Item.Name;
		m_TooltipMessage.StringReference.Arguments = new[] { new { Amount = ingredient.Amount } };

		m_IngredientIcon.sprite = ingredient.Item.PreviewImage;
		m_AvailableCheckMark.SetActive(isAvailable);
		m_UnavailableCheckMark.SetActive(!isAvailable);

	}

	public void HoveredItem() => m_Tooltip.SetActive(true);

	public void UnHoveredItem() => m_Tooltip.SetActive(false);
}

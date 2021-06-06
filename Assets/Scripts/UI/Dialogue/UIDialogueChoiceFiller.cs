using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIDialogueChoiceFiller : MonoBehaviour
{
	[SerializeField] private LocalizeStringEvent m_ChoiceText = default;
	[SerializeField] private DialogueChoiceChannelSO m_MakeAChoiceEvent = default;
	[SerializeField] private MultiInputButton m_ActionButton = default;

	private Choice m_CurrentChoice;


	public void FillChoice(Choice choiceToFill, bool isSelected)
	{
		m_CurrentChoice = choiceToFill;
		m_ChoiceText.StringReference = choiceToFill.Response;
		m_ActionButton.interactable = true;

		if (isSelected)
			m_ActionButton.UpdateSelected();
	}

	public void ButtonClicked() => m_MakeAChoiceEvent.RaiseEvent(m_CurrentChoice);
}

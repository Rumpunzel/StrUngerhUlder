using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteraction : MonoBehaviour
{
	[SerializeField] private List<InteractionSO> m_ListInteractions = default;
	[SerializeField] private UIInteractionIndicator m_InteractionItem = default;


	public void FillInteractionPanel(InteractionType interactionType)
	{
		if ((m_ListInteractions != null) && (m_InteractionItem != null))
		{
			if (m_ListInteractions.Exists(o => o.InteractionType == interactionType))
				m_InteractionItem.FillInteractionPanel(m_ListInteractions.Find(o => o.InteractionType == interactionType));
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Events/UI/Dialogue Line Channel")]
public class DialogueLineChannelSO : ScriptableObject
{
	public UnityAction<LocalizedString, ActorSO> onEventRaised;

	public void RaiseEvent(LocalizedString line, ActorSO actor)
	{
		if (onEventRaised != null)
			onEventRaised.Invoke(line, actor);
	}
}

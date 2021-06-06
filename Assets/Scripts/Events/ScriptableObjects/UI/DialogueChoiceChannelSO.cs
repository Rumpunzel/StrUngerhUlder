using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Dialogue Choice Channel")]
public class DialogueChoiceChannelSO : ScriptableObject
{
	public UnityAction<Choice> onEventRaised;

	public void RaiseEvent(Choice choice)
	{
		if (onEventRaised != null)
			onEventRaised.Invoke(choice);
	}
}

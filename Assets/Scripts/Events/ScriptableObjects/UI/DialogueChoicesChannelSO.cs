using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Dialogue Choices Channel")]
public class DialogueChoicesChannelSO : ScriptableObject
{
	public UnityAction<List<Choice>> onEventRaised;
	
	public void RaiseEvent(List<Choice> choices)
	{
		if (onEventRaised != null)
			onEventRaised.Invoke(choices);
	}
}

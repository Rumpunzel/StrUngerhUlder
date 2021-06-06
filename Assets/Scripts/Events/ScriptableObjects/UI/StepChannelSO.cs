using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/UI/Step Channel")]
public class StepChannelSO : ScriptableObject
{
	public UnityAction<StepSO> onEventRaised;

	public void RaiseEvent(StepSO step)
	{
		if (onEventRaised != null)
			onEventRaised.Invoke(step);
	}
}

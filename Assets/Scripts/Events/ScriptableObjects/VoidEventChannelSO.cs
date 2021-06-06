using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>
[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class VoidEventChannelSO : EventChannelBaseSO
{
	public UnityAction onEventRaised;

	public void RaiseEvent()
	{
		if (onEventRaised != null)
			onEventRaised.Invoke();
	}
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool, GameObject>
{

}

/// <summary>
/// A generic class for a "zone", that is a trigger collider that can detect if an object of a certain type (layer) entered or exited it.
/// Implements <code>OnTriggerEnter</code> and <code>OnTriggerExit</code> so it needs to be on the same object that holds the Collider.
/// </summary>
public class ZoneTriggerController : MonoBehaviour
{
	[SerializeField] private BoolEvent m_EnterZone = default;
	[SerializeField] private LayerMask m_Layers = default;

	private void OnTriggerEnter(Collider other)
	{	
		if ((1 << other.gameObject.layer & m_Layers) != 0)
		{
			m_EnterZone.Invoke(true, other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ((1 << other.gameObject.layer & m_Layers) != 0)
		{
			m_EnterZone.Invoke(false, other.gameObject);
		}
	}
}

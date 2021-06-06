using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
	[SerializeField] private FadeChannelSO m_FadeChannelSO;
	[SerializeField] private Image m_ImageComponent;

	private bool m_IsCurrentlyFading = false;


	private void OnEnable()
	{
		m_FadeChannelSO.onEventRaised += InitiateFade;
	}

	private void OnDisable()
	{
		m_FadeChannelSO.onEventRaised -= InitiateFade;
	}


	/// <summary>
	/// Enumerator that fades in the canvas's imageComponent to turn the screen to a flat color over time. Fadeins called simeutaneously will only fade in the earliest call and discard any others.
	/// </summary>
	private IEnumerator FadeCoroutine(bool fadeIn, float duration, Color endColor = default)
	{
		Color startColor = m_ImageComponent.color;

		if (fadeIn)
			endColor = Color.clear;

		float totalTime = 0f;

		while (totalTime <= duration)
		{
			totalTime += Time.deltaTime;
			m_ImageComponent.color = Color.Lerp(startColor, endColor, totalTime / duration);

			yield return null;
		}

		m_ImageComponent.color = endColor; //Force to end result
		m_IsCurrentlyFading = false;
	}

	/// <summary>
	/// Controls the fade-in and fade-out.
	/// </summary>
	/// <param name="fadeIn">If true, rectangle fades out and gameplay is visible. If false, the screen becomes black.</param>
	/// <param name="duration">How long it takes to the image to fade in/out.</param>
	/// <param name="color">Target color for the image to reach. Disregarded when fading out.</param>
	private void InitiateFade(bool fadeIn, float duration, Color desiredColor)
	{
		if (!m_IsCurrentlyFading) // Makes sure multiple fade-ins or outs don't happen at the same time. Note this will mean fadeouts called at the same time will be discarded.
		{
			m_IsCurrentlyFading = true;
			StartCoroutine(FadeCoroutine(fadeIn, duration, desiredColor));
		}
	}
}

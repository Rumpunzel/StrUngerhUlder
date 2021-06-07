using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
//using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    public event UnityAction<SoundEmitter> onSoundFinishedPlaying;

	private AudioSource m_AudioSource;


	private void Awake()
	{
		m_AudioSource = this.GetComponent<AudioSource>();
		m_AudioSource.playOnAwake = false;
	}


	/// <summary>
	/// Instructs the AudioSource to play a single clip, with optional looping, in a position in 3D space.
	/// </summary>
	/// <param name="clip"></param>
	/// <param name="settings"></param>
	/// <param name="hasToLoop"></param>
	/// <param name="position"></param>
	public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool hasToLoop, Vector3 position = default)
	{
		m_AudioSource.clip = clip;
		settings.ApplyTo(m_AudioSource);
		m_AudioSource.transform.position = position;
		m_AudioSource.loop = hasToLoop;
		m_AudioSource.time = 0f; //Reset in case this AudioSource is being reused for a short SFX after being used for a long music track
		m_AudioSource.Play();

		if (!hasToLoop)
			StartCoroutine(FinishedPlaying(clip.length));
	}

	public void FadeMusicIn(AudioClip musicClip, AudioConfigurationSO settings, float duration, float startTime = 0f)
	{
		PlayAudioClip(musicClip, settings, true);
		m_AudioSource.volume = 0f;

		//Start the clip at the same time the previous one left, if length allows
		//TODO: find a better way to sync fading songs
		if (startTime <= m_AudioSource.clip.length)
			m_AudioSource.time = startTime;

		//m_AudioSource.DOFade(1f, duration);
	}

	public float FadeMusicOut(float duration)
	{
		//m_AudioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;

		return m_AudioSource.time;
	}

	/// <summary>
	/// Used to check which music track is being played.
	/// </summary>
	public AudioClip GetClip() => m_AudioSource.clip;

	/// <summary>
	/// Used when the game is unpaused, to pick up SFX from where they left.
	/// </summary>
	public void Resume() => m_AudioSource.Play();

	/// <summary>
	/// Used when the game is paused.
	/// </summary>
	public void Pause() => m_AudioSource.Pause();

	public void Stop() => m_AudioSource.Stop();

	public void Finish()
	{
		if (m_AudioSource.loop)
		{
			m_AudioSource.loop = false;
			float timeRemaining = m_AudioSource.clip.length - m_AudioSource.time;
			StartCoroutine(FinishedPlaying(timeRemaining));
		}
	}

	public bool IsPlaying() => m_AudioSource.isPlaying;

	public bool IsLooping() => m_AudioSource.loop;


    private void OnFadeOutComplete() => NotifyBeingDone();

	private IEnumerator FinishedPlaying(float clipLength)
	{
		yield return new WaitForSeconds(clipLength);

		NotifyBeingDone();
	}

	private void NotifyBeingDone() => onSoundFinishedPlaying.Invoke(this); // The AudioManager will pick this up
}

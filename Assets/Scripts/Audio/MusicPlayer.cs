using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	[SerializeField] private VoidEventChannelSO m_OnSceneReady = default;
	[SerializeField] private AudioCueEventChannelSO m_PlayMusicOn = default;
	[SerializeField] private GameSceneSO m_ThisSceneSO = default;
	[SerializeField] private AudioConfigurationSO m_AudioConfig = default;


	private void OnEnable() => m_OnSceneReady.onEventRaised += PlayMusic;
	private void OnDisable() => m_OnSceneReady.onEventRaised -= PlayMusic;
	private void PlayMusic() => m_PlayMusicOn.RaisePlayEvent(m_ThisSceneSO.musicTrack, m_AudioConfig);
}

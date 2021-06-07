using UnityEngine;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.Audio.ScriptableObjects;

namespace Strungerhulder.Audio
{
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
}

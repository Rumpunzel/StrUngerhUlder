using UnityEngine;
using UnityEngine.Audio;
using Strungerhulder.Audio.ScriptableObjects;

namespace Strungerhulder.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("SoundEmitters pool")]
        [SerializeField] private SoundEmitterPoolSO m_Pool = default;
        [SerializeField] private int m_InitialSize = 10;


        [Header("Listening on channels")]

        //[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
        //[SerializeField] private AudioCueEventChannelSO m_SFXEventChannel = default;

        //[Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
        //[SerializeField] private AudioCueEventChannelSO m_MusicEventChannel = default;


        [Header("Audio control")]
        [SerializeField] private AudioMixer m_AudioMixer = default;
        [Range(0f, 1f)] [SerializeField] private float m_MasterVolume = 1f;
        [Range(0f, 1f)] [SerializeField] private float m_MusicVolume = 1f;
        [Range(0f, 1f)] [SerializeField] private float m_SFXVolume = 1f;


        private SoundEmitterVault m_SoundEmitterVault;
        private SoundEmitter m_MusicSoundEmitter;


        private void Awake()
        {
            //TODO: Get the initial volume levels from the settings
            m_SoundEmitterVault = new SoundEmitterVault();

            m_Pool.Prewarm(m_InitialSize);
            m_Pool.SetParent(this.transform);
        }

        private void OnEnable()
        {
            //m_SFXEventChannel.onAudioCuePlayRequested += PlayAudioCue;
            //m_SFXEventChannel.onAudioCueStopRequested += StopAudioCue;
            //m_SFXEventChannel.onAudioCueFinishRequested += FinishAudioCue;

            //m_MusicEventChannel.onAudioCuePlayRequested += PlayMusicTrack;
            //m_MusicEventChannel.onAudioCueStopRequested += StopMusic;
        }

        private void OnDestroy()
        {
            //m_SFXEventChannel.onAudioCuePlayRequested -= PlayAudioCue;
            //m_SFXEventChannel.onAudioCueStopRequested -= StopAudioCue;
            //m_SFXEventChannel.onAudioCueFinishRequested -= FinishAudioCue;

            //m_MusicEventChannel.onAudioCuePlayRequested -= PlayMusicTrack;
        }

        /// <summary>
        /// This is only used in the Editor, to debug volumes.
        /// It is called when any of the variables is changed, and will directly change the value of the volumes on the AudioMixer.
        /// </summary>
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                SetGroupVolume("MasterVolume", m_MasterVolume);
                SetGroupVolume("MusicVolume", m_MusicVolume);
                SetGroupVolume("SFXVolume", m_SFXVolume);
            }
        }


        public void SetGroupVolume(string parameterName, float normalizedVolume)
        {
            bool volumeSet = m_AudioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));

            if (!volumeSet)
                Debug.LogError("The AudioMixer parameter was not found");
        }

        public float GetGroupVolume(string parameterName)
        {
            if (m_AudioMixer.GetFloat(parameterName, out float rawVolume))
            {
                return MixerValueToNormalized(rawVolume);
            }
            else
            {
                Debug.LogError("The AudioMixer parameter was not found");
                return 0f;
            }
        }

        /// <summary>
        /// Plays an AudioCue by requesting the appropriate number of SoundEmitters from the pool.
        /// </summary>
        public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
        {
            AudioClip[] clipsToPlay = audioCue.GetClips();
            SoundEmitter[] soundEmitterArray = new SoundEmitter[clipsToPlay.Length];

            int nOfClips = clipsToPlay.Length;

            for (int i = 0; i < nOfClips; i++)
            {
                soundEmitterArray[i] = m_Pool.Request();

                if (soundEmitterArray[i] != null)
                {
                    soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings, audioCue.looping, position);

                    if (!audioCue.looping)
                        soundEmitterArray[i].onSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }

            return m_SoundEmitterVault.Add(audioCue, soundEmitterArray);
        }

        public bool FinishAudioCue(AudioCueKey audioCueKey)
        {
            bool isFound = m_SoundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

            if (isFound)
            {
                for (int i = 0; i < soundEmitters.Length; i++)
                {
                    soundEmitters[i].Finish();
                    soundEmitters[i].onSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }
            else
                Debug.LogWarning("Finishing an AudioCue was requested, but the AudioCue was not found.");

            return isFound;
        }

        public bool StopAudioCue(AudioCueKey audioCueKey)
        {
            bool isFound = m_SoundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

            if (isFound)
            {
                for (int i = 0; i < soundEmitters.Length; i++)
                {
                    StopAndCleanEmitter(soundEmitters[i]);
                }

                m_SoundEmitterVault.Remove(audioCueKey);
            }

            return isFound;
        }


        // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
        /// when using UI sliders normalized format
        private float MixerValueToNormalized(float mixerValue)
        {
            // We're assuming the range [-80dB to 0dB] becomes [0 to 1]
            return 1f + (mixerValue / 80f);
        }
        private float NormalizedToMixerValue(float normalizedValue)
        {
            // We're assuming the range [0 to 1] becomes [-80dB to 0dB]
            // This doesn't allow values over 0dB
            return (normalizedValue - 1f) * 80f;
        }

        private AudioCueKey PlayMusicTrack(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
        {
            float fadeDuration = 2f;
            float startTime = 0f;

            if (m_MusicSoundEmitter != null && m_MusicSoundEmitter.IsPlaying())
            {
                AudioClip songToPlay = audioCue.GetClips()[0];
                if (m_MusicSoundEmitter.GetClip() == songToPlay)
                    return AudioCueKey.invalid;

                //Music is already playing, need to fade it out
                startTime = m_MusicSoundEmitter.FadeMusicOut(fadeDuration);
            }

            m_MusicSoundEmitter = m_Pool.Request();
            m_MusicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], audioConfiguration, 1f, startTime);
            m_MusicSoundEmitter.onSoundFinishedPlaying += StopMusicEmitter;

            return AudioCueKey.invalid; //No need to return a valid key for music
        }

        private bool StopMusic(AudioCueKey key)
        {
            if (m_MusicSoundEmitter != null && m_MusicSoundEmitter.IsPlaying())
            {
                m_MusicSoundEmitter.Stop();
                return true;
            }
            else
                return false;
        }

        private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter) => StopAndCleanEmitter(soundEmitter);

        private void StopAndCleanEmitter(SoundEmitter soundEmitter)
        {
            if (!soundEmitter.IsLooping())
                soundEmitter.onSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;

            soundEmitter.Stop();
            m_Pool.Return(soundEmitter);

            //TODO: is the above enough?
            //m_SoundEmitterVault.Remove(audioCueKey); is never called if StopAndClean is called after a Finish event
            //How is the key removed from the vault?
        }

        private void StopMusicEmitter(SoundEmitter soundEmitter)
        {
            soundEmitter.onSoundFinishedPlaying -= StopMusicEmitter;
            m_Pool.Return(soundEmitter);
        }
    }
}

using System.Collections.Generic;
using Strungerhulder.Audio.ScriptableObjects;

namespace Strungerhulder.Audio
{
    public class SoundEmitterVault
    {
        private int m_NextUniqueKey = 0;
        private List<AudioCueKey> m_EmittersKey;
        private List<SoundEmitter[]> m_EmittersList;


        public SoundEmitterVault()
        {
            m_EmittersKey = new List<AudioCueKey>();
            m_EmittersList = new List<SoundEmitter[]>();
        }

        public AudioCueKey GetKey(AudioCueSO cue) => new AudioCueKey(m_NextUniqueKey++, cue);

        public void Add(AudioCueKey key, SoundEmitter[] emitter)
        {
            m_EmittersKey.Add(key);
            m_EmittersList.Add(emitter);
        }

        public AudioCueKey Add(AudioCueSO cue, SoundEmitter[] emitter)
        {
            AudioCueKey emitterKey = GetKey(cue);

            m_EmittersKey.Add(emitterKey);
            m_EmittersList.Add(emitter);

            return emitterKey;
        }

        public bool Get(AudioCueKey key, out SoundEmitter[] emitter)
        {
            int index = m_EmittersKey.FindIndex(x => x == key);

            if (index < 0)
            {
                emitter = null;
                return false;
            }

            emitter = m_EmittersList[index];
            return true;
        }

        public bool Remove(AudioCueKey key)
        {
            int index = m_EmittersKey.FindIndex(x => x == key);
            return RemoveAt(index);
        }


        private bool RemoveAt(int index)
        {
            if (index < 0)
                return false;

            m_EmittersKey.RemoveAt(index);
            m_EmittersList.RemoveAt(index);

            return true;
        }
    }
}

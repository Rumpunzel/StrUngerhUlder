using UnityEngine;
using Strungerhulder.Factory;

namespace Strungerhulder.Audio.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewSoundEmitterFactory", menuName = "Factory/SoundEmitter Factory")]
    public class SoundEmitterFactorySO : FactorySO<SoundEmitter>
    {
        public SoundEmitter prefab = default;

        public override SoundEmitter Create() => Instantiate(prefab);
    }
}

using UnityEngine;
using Strungerhulder.Pool;
using Strungerhulder.Factory;

namespace Strungerhulder.Audio.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewSoundEmitterPool", menuName = "Pool/SoundEmitter Pool")]
    public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
    {
        [SerializeField] private SoundEmitterFactorySO _factory;

        public override IFactory<SoundEmitter> Factory
        {
            get { return _factory; }
            set { _factory = value as SoundEmitterFactorySO; }
        }
    }
}

using UnityEngine;

namespace Strungerhulder.Characters.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GetHitEffectConfig", menuName = "EntityConfig/Get Hit Effect Config")]
    public class GetHitEffectConfigSO : ScriptableObject
    {
        [Tooltip("Flashing effect color applied when getting hit.")]
        [SerializeField]
        private Color m_GetHitFlashingColor = default;

        [Tooltip("Flashing effect duration (in seconds).")]
        [SerializeField]
        private float m_GetHitFlashingDuration = 0.5f;

        [Tooltip("Flashing effect speed (number of flashings during the duration).")]
        [SerializeField]
        private float m_GetHitFlashingSpeed = 3.0f;

        public Color GetHitFlashingColor => m_GetHitFlashingColor;
        public float GetHitFlashingDuration => m_GetHitFlashingDuration;
        public float GetHitFlashingSpeed => m_GetHitFlashingSpeed;
    }
}

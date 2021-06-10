using UnityEngine;

namespace Strungerhulder.Characters.ScriptableObjects
{
    public class NPCMovementConfigSO : ScriptableObject
    {
        [Tooltip("Waypoint stop duration")]
        [SerializeField] private float m_StopDuration;

        [Tooltip("Roaming speed")]
        [SerializeField] private float m_Speed;

        public float Speed => m_Speed;
        public float StopDuration => m_StopDuration;
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Strungerhulder.Characters.ScriptableObjects
{
    [CreateAssetMenu(fileName = "RoamingAroundCenter", menuName = "EntityConfig/Roaming Around Center")]
    public class RoamingAroundCenterConfigSO : NPCMovementConfigSO
    {
        [Tooltip("Is roaming from spwaning center")]
        [SerializeField] private bool m_FromSpawningPoint = true;

        [Tooltip("Custom roaming center")]
        [SerializeField] private Vector3 m_CustomCenter;

        [Tooltip("Roaming distance from center")]
        [SerializeField] private float m_Radius;

        public bool FromSpawningPoint => m_FromSpawningPoint;
        public Vector3 CustomCenter => m_CustomCenter;
        public float Radius => m_Radius;
    }
}

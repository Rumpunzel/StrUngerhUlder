using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Strungerhulder.Characters.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "EntityConfig/Attack Config")]
    public class AttackConfigSO : ScriptableObject
    {
        [Tooltip("Character attack strength")]
        [SerializeField] private int m_AttackStrength;

        [Tooltip("Character attack reload duration (in second).")]
        [SerializeField] private float m_AttackReloadDuration;


        public int AttackStrength => m_AttackStrength;
        public float AttackReloadDuration => m_AttackReloadDuration;
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Strungerhulder.Characters.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HealthConfig", menuName = "EntityConfig/Health Config")]
    public class HealthConfigSO : ScriptableObject
    {
        [Tooltip("Initial critter health")]
        [SerializeField] private int m_MaxHealth;

        public int MaxHealth => m_MaxHealth;
    }
}

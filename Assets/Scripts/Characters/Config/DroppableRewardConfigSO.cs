using System.Collections.Generic;
using UnityEngine;

namespace Strungerhulder.Characters.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DroppableRewardConfig", menuName = "EntityConfig/Reward Dropping Rate Config")]
    public class DroppableRewardConfigSO : ScriptableObject
    {
        [Tooltip("Item scattering distance from the source of dropping.")]
        [SerializeField]
        private float m_ScatteringDistance = default;

        [Tooltip("The list of drop goup that can be dropped by this critter when killed")]
        [SerializeField]
        private List<DropGroup> m_DropGroups = new List<DropGroup>();

        public float ScatteringDistance => m_ScatteringDistance;
        public List<DropGroup> DropGroups => m_DropGroups;
    }
}

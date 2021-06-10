using System.Collections.Generic;
using System;
using UnityEngine;

namespace Strungerhulder.Characters
{
    [Serializable]
    public class DropGroup
    {
        [SerializeField]
        List<DropItem> m_Drops;

        [SerializeField]
        float m_DropRate;

        public List<DropItem> Drops => m_Drops;
        public float DropRate => m_DropRate;
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Strungerhulder.Characters.ScriptableObjects
{
    [System.Serializable]
    public class WaypointData
    {
        public Vector3 waypoint;
        public List<Vector3> corners;
    }

    [CreateAssetMenu(fileName = "PathwayConfig", menuName = "EntityConfig/Pathway Config")]
    public class PathwayConfigSO : NPCMovementConfigSO
    {
        [HideInInspector]
        public List<WaypointData> Waypoints;

#if UNITY_EDITOR
        [HideInInspector]
        public bool DisplayProbes;

        [HideInInspector]
        public bool ToggledNavMeshDisplay;

        [SerializeField]
        private Color m_LineColor = Color.black;

        [SerializeField, Range(0, 100)]
        private int m_TextSize = 20;

        [SerializeField]
        private Color m_TextColor = Color.white;

        [SerializeField, Range(0, 100)]
        [Tooltip("This function may reduce the frame rate if a large probe radius is specified. To avoid frame rate issues," +
                    " it is recommended that you specify a max distance of twice the agent height.")]
        private float m_ProbeRadius = 3;

        private List<Vector3> m_Path;
        private List<bool> m_Hits;

        public const string FIELD_LABEL = "Point ";
        public const string TITLE_LABEL = "Waypoints";

        public Color LineColor { get => m_LineColor; }
        public Color TextColor { get => m_TextColor; }
        public int TextSize { get => m_TextSize; }
        public float ProbeRadius { get => m_ProbeRadius; }
        public List<Vector3> Path { get => m_Path; set => m_Path = value; }
        public List<bool> Hits { get => m_Hits; set => m_Hits = value; }

        public bool RealTimeEnabled;
#endif
    }
}

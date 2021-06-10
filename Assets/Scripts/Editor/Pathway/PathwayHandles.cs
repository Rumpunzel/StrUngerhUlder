using UnityEngine;
using UnityEditor;
using Strungerhulder.Characters.ScriptableObjects;

namespace Strungerhulder.EditorTools.PathwayTool
{
    public class PathwayHandles
    {
        private PathwayConfigSO m_Pathway;
        private Vector3 m_Tmp;


        public PathwayHandles(PathwayConfigSO pathway)
        {
            m_Pathway = pathway;
        }

        public int DisplayHandles()
        {
            for (int i = 0; i < m_Pathway.Waypoints.Count; i++)
            {
                EditorGUI.BeginChangeCheck();

                m_Tmp = Handles.PositionHandle(m_Pathway.Waypoints[i].waypoint, Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    m_Pathway.Waypoints[i].waypoint = m_Tmp;
                    return i;
                }
            }

            return -1;
        }
    }
}

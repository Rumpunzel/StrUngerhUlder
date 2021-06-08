using UnityEngine;
using UnityEditorInternal;

namespace Strungerhulder.EditorTools.PathwayTool
{
    public class PathWayNavMeshUI
    {
        private PathwayConfigSO m_Pathway;
        private PathwayNavMesh m_PathwayNavMesh;


        public PathWayNavMeshUI(PathwayConfigSO pathway)
        {
            m_Pathway = pathway;
            m_PathwayNavMesh = new PathwayNavMesh(pathway);
            RestorePath();
        }

        public void OnInspectorGUI()
        {
            if (!m_Pathway.ToggledNavMeshDisplay)
            {
                if (GUILayout.Button("NavMesh Path"))
                {
                    m_Pathway.ToggledNavMeshDisplay = true;
                    GeneratePath();
                    InternalEditorUtility.RepaintAllViews();
                }
            }
            else
            {
                if (GUILayout.Button("Handles Path"))
                {
                    m_Pathway.ToggledNavMeshDisplay = false;
                    InternalEditorUtility.RepaintAllViews();
                }
            }
        }

        public void UpdatePath()
        {
            if (!m_Pathway.DisplayProbes)
                m_PathwayNavMesh.UpdatePath();
        }

        public void UpdatePathAt(int index)
        {
            if (index >= 0)
            {
                m_Pathway.DisplayProbes = !m_PathwayNavMesh.HasNavMeshAt(index);

                if (!m_Pathway.DisplayProbes && m_Pathway.ToggledNavMeshDisplay)
                    m_Pathway.DisplayProbes = !m_PathwayNavMesh.UpdateCornersAt(index);
            }
        }

        public void RealTime(int index)
        {
            if (m_Pathway.RealTimeEnabled)
            {
                UpdatePathAt(index);

                if (m_Pathway.ToggledNavMeshDisplay)
                    UpdatePath();
            }
        }

        public void GeneratePath()
        {
            if (m_Pathway.ToggledNavMeshDisplay)
            {
                RestorePath();
                m_Pathway.ToggledNavMeshDisplay = !m_Pathway.DisplayProbes;
            }
        }


        private void RestorePath()
        {
            bool existsPath = true;

            m_Pathway.Hits.Clear();
            m_Pathway.DisplayProbes = false;

            if (m_Pathway.Waypoints.Count > 1)
            {
                for (int i = 0; i < m_Pathway.Waypoints.Count; i++)
                {
                    existsPath &= m_PathwayNavMesh.HasNavMeshAt(i);
                    existsPath &= m_PathwayNavMesh.UpdateCornersAt(i);
                }

                if (existsPath)
                    m_PathwayNavMesh.UpdatePath();
            }

            m_Pathway.DisplayProbes = !existsPath;
        }
    }
}

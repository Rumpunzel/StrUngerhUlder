using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

namespace Strungerhulder.EditorTools.PathwayTool
{
    public class PathwayNavMesh
    {
        private PathwayConfigSO m_Pathway;


        public PathwayNavMesh(PathwayConfigSO pathway)
        {
            m_Pathway = pathway;
            m_Pathway.Hits = new List<bool>();
        }

        public bool HasNavMeshAt(int index)
        {
            NavMeshHit hit;
            bool hasHit = true;

            if (m_Pathway.Waypoints.Count >= m_Pathway.Hits.Count)
            {
                hasHit = NavMesh.SamplePosition(m_Pathway.Waypoints[index].waypoint, out hit, m_Pathway.ProbeRadius, NavMesh.AllAreas);

                if (index > m_Pathway.Hits.Count - 1)
                {
                    index = m_Pathway.Hits.Count;
                    m_Pathway.Hits.Add(hasHit);
                }
                else
                    m_Pathway.Hits[index] = hasHit;

                if (hasHit)
                    m_Pathway.Waypoints[index].waypoint = hit.position;
            }
            else
                m_Pathway.Hits.RemoveAt(index);

            return hasHit;
        }

        private List<Vector3> GetPathCorners(int startIndex, int endIndex)
        {
            NavMeshPath navMeshPath = new NavMeshPath();

            if (NavMesh.CalculatePath(m_Pathway.Waypoints[startIndex].waypoint, m_Pathway.Waypoints[endIndex].waypoint, NavMesh.AllAreas, navMeshPath))
                return navMeshPath.corners.ToList();
            else
                return null;
        }

        private bool CopyCorners(int startIndex, int endIndex)
        {
            List<Vector3> result;

            if ((result = GetPathCorners(startIndex, endIndex)) != null)
                m_Pathway.Waypoints[startIndex].corners = result;

            return result != null;
        }

        public bool UpdateCornersAt(int index)
        {
            bool canUpdate = true;

            if (m_Pathway.Waypoints.Count > 1 && index < m_Pathway.Waypoints.Count)
            {
                if (index == 0)
                {
                    canUpdate = CopyCorners(index, index + 1);
                    canUpdate &= CopyCorners(m_Pathway.Waypoints.Count - 1, index);
                }
                else if (index == m_Pathway.Waypoints.Count - 1)
                {
                    canUpdate = CopyCorners(index - 1, index);
                    canUpdate &= CopyCorners(index, 0);
                }
                else
                {
                    canUpdate = CopyCorners(index - 1, index);
                    canUpdate &= CopyCorners(index, index + 1);
                }
            }

            return canUpdate;
        }

        public void UpdatePath()
        {
            if (m_Pathway.Waypoints.Count > 1)
            {
                m_Pathway.Path = m_Pathway.Waypoints.Aggregate(new List<Vector3>(), (acc, wpd) =>
                {
                    wpd.corners.ForEach(c => acc.Add(c));
                    return acc;
                });
            }
            else
                m_Pathway.Path.Clear();
        }
    }
}

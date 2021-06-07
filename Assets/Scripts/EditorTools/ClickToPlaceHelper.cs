using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Strungerhulder.EditorTools
{
    [ExecuteInEditMode]
    [AddComponentMenu("Strungerhulder/Tools/Click to Place")]
    public class ClickToPlaceHelper : MonoBehaviour
    {
        [Tooltip("Vertical offset above the clicked point. Useful to avoid spawn points to be directly ON the geometry which might cause issues.")]
        [SerializeField] private float m_VerticalOffset = 0.1f;

        private Vector3 m_TargetPosition;

        public bool IsTargeting { get; private set; }

        private void OnDrawGizmos()
        {
            if (IsTargeting)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(m_TargetPosition, Vector3.one * 0.3f);
            }
        }

        public void BeginTargeting()
        {
            IsTargeting = true;
            m_TargetPosition = transform.position;
        }

        public void UpdateTargeting(Vector3 spawnPosition)
        {
            m_TargetPosition = spawnPosition + Vector3.up * m_VerticalOffset;
        }

        public void EndTargeting()
        {
            IsTargeting = false;
#if UNITY_EDITOR
            Undo.RecordObject(transform, $"{gameObject.name} moved by ClickToPlaceHelper");
#endif
            transform.position = m_TargetPosition;
        }
    }
}

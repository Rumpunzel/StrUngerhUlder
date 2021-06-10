using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Strungerhulder.Characters.ScriptableObjects;

namespace Strungerhulder.EditorTools.PathwayTool
{
    [CustomEditor(typeof(PathwayConfigSO))]
    public class PathwayEditor : Editor
    {
        private enum LIST_MODIFICATION { ADD, SUPP, DRAG, OTHER };

        private ReorderableList m_ReorderableList;
        private PathwayConfigSO m_Pathway;
        private PathwayHandles m_PathwayHandles;
        private PathWayNavMeshUI m_PathWayNavMeshUI;

        private LIST_MODIFICATION m_CurrentListModification;
        private int m_IndexCurrentModification;


        public void OnSceneGUI(SceneView sceneView)
        {
            int index = m_PathwayHandles.DisplayHandles();
            m_PathWayNavMeshUI.RealTime(index);
            PathwayGizmos.DrawGizmosSelected(m_Pathway);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            m_ReorderableList.DoLayoutList();
            m_PathWayNavMeshUI.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }


        private void OnEnable()
        {
            Undo.undoRedoPerformed += DoUndo;
            m_ReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("Waypoints"), true, true, true, true);
            m_ReorderableList.drawHeaderCallback += DrawHeader;
            m_ReorderableList.drawElementCallback += DrawElement;
            m_ReorderableList.onAddCallback += AddItem;
            m_ReorderableList.onRemoveCallback += RemoveItem;
            m_ReorderableList.onChangedCallback += ListModified;
            m_ReorderableList.onMouseDragCallback += DragItem;
            m_Pathway = (target as PathwayConfigSO);
            m_PathWayNavMeshUI = new PathWayNavMeshUI(m_Pathway);
            m_PathwayHandles = new PathwayHandles(m_Pathway);
            m_CurrentListModification = LIST_MODIFICATION.OTHER;
            SceneView.duringSceneGui += this.OnSceneGUI;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= DoUndo;
            m_ReorderableList.drawHeaderCallback -= DrawHeader;
            m_ReorderableList.drawElementCallback -= DrawElement;
            m_ReorderableList.onAddCallback -= AddItem;
            m_ReorderableList.onRemoveCallback -= RemoveItem;
            m_ReorderableList.onChangedCallback -= ListModified;
            m_ReorderableList.onMouseDragCallback -= DragItem;
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        private void DrawHeader(Rect rect) => GUI.Label(rect, PathwayConfigSO.TITLE_LABEL);

        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            SerializedProperty item = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("waypoint");
            item.vector3Value = EditorGUI.Vector3Field(rect, PathwayConfigSO.FIELD_LABEL + index, item.vector3Value);
        }

        private void AddItem(ReorderableList list)
        {
            int index = list.index;

            if (index > -1 && list.serializedProperty.arraySize >= 1)
            {
                list.serializedProperty.InsertArrayElementAtIndex(index + 1);
                Vector3 previous = list.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("waypoint").vector3Value;
                list.serializedProperty.GetArrayElementAtIndex(index + 1).FindPropertyRelative("waypoint").vector3Value = new Vector3(previous.x + 2, previous.y, previous.z + 2);
                m_IndexCurrentModification = index + 1;
            }
            else
            {
                list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
                Vector3 previous = Vector3.zero;
                list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1).FindPropertyRelative("waypoint").vector3Value = new Vector3(previous.x + 2, previous.y, previous.z + 2);
                m_IndexCurrentModification = list.serializedProperty.arraySize - 1;
            }
            m_CurrentListModification = LIST_MODIFICATION.ADD;
            list.index++;
        }

        private void RemoveItem(ReorderableList list)
        {
            int index = list.index;

            list.serializedProperty.DeleteArrayElementAtIndex(index);

            if (list.index == list.serializedProperty.arraySize)
            {
                list.index--;
            }
            m_IndexCurrentModification = index - 1;
            m_CurrentListModification = LIST_MODIFICATION.SUPP;
        }

        private void DragItem(ReorderableList list)
        {
            m_IndexCurrentModification = list.index;
            m_CurrentListModification = LIST_MODIFICATION.DRAG;
        }

        private void ListModified(ReorderableList list)
        {
            list.serializedProperty.serializedObject.ApplyModifiedProperties();

            switch (m_CurrentListModification)
            {
                case LIST_MODIFICATION.ADD:
                    m_PathWayNavMeshUI.UpdatePathAt(m_IndexCurrentModification);
                    break;

                case LIST_MODIFICATION.SUPP:
                    if (list.serializedProperty.arraySize > 1)
                    {
                        m_PathWayNavMeshUI.UpdatePathAt((list.serializedProperty.arraySize + m_IndexCurrentModification) % list.serializedProperty.arraySize);
                    }
                    break;
                case LIST_MODIFICATION.DRAG:
                    m_PathWayNavMeshUI.UpdatePathAt(list.index);
                    m_PathWayNavMeshUI.UpdatePathAt(m_IndexCurrentModification);
                    break;
                default:
                    break;
            }
            m_CurrentListModification = LIST_MODIFICATION.OTHER;
        }

        private void DoUndo()
        {
            serializedObject.UpdateIfRequiredOrScript();

            if (m_ReorderableList.index >= m_ReorderableList.serializedProperty.arraySize)
                m_ReorderableList.index = m_ReorderableList.serializedProperty.arraySize - 1;

            m_PathWayNavMeshUI.GeneratePath();
        }

    }
}

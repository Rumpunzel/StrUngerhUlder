using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.StateMachines.Editor
{
    [CustomEditor(typeof(StateSO))]
    public class StateEditor : UnityEditor.Editor
    {
        private ReorderableList m_List;
        private SerializedProperty m_Actions;

        private void OnEnable()
        {
            Undo.undoRedoPerformed += DoUndo;
            m_Actions = serializedObject.FindProperty("m_Actions");
            m_List = new ReorderableList(serializedObject, m_Actions, true, true, true, true);
            SetupActionsList(m_List);
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= DoUndo;
        }

        public override void OnInspectorGUI()
        {
            m_List.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void DoUndo()
        {
            serializedObject.UpdateIfRequiredOrScript();
        }

        private static void SetupActionsList(ReorderableList reorderableList)
        {
            reorderableList.elementHeight *= 1.5f;
            reorderableList.drawHeaderCallback += rect => GUI.Label(rect, "Actions");
            reorderableList.onAddCallback += list =>
            {
                int count = list.count;
                list.serializedProperty.InsertArrayElementAtIndex(count);
                var prop = list.serializedProperty.GetArrayElementAtIndex(count);
                prop.objectReferenceValue = null;
            };

            reorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                Rect r = rect;
                r.height = EditorGUIUtility.singleLineHeight;
                r.y += 5;
                r.x += 5;

                var prop = reorderableList.serializedProperty.GetArrayElementAtIndex(index);

                if (prop.objectReferenceValue != null)
                {
                    var label = prop.objectReferenceValue.name;

                    r.width = 35;
                    EditorGUI.PropertyField(r, prop, GUIContent.none);
                    r.width = rect.width - 50;
                    r.x += 42;
                    GUI.Label(r, label, EditorStyles.boldLabel);
                }
                else
                    EditorGUI.PropertyField(r, prop, GUIContent.none);
            };

            reorderableList.onChangedCallback += list => list.serializedProperty.serializedObject.ApplyModifiedProperties();
            reorderableList.drawElementBackgroundCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (isFocused)
                    EditorGUI.DrawRect(rect, ContentStyle.Focused);

                if (index % 2 != 0)
                    EditorGUI.DrawRect(rect, ContentStyle.ZebraDark);
                else
                    EditorGUI.DrawRect(rect, ContentStyle.ZebraLight);
            };
        }
    }
}

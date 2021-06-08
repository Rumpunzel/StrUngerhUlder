using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Strungerhulder.EditorTools.Replacer
{
    // This code has been adapted from a tutorial by Patryk Galach
    // https://www.patrykgalach.com/2019/08/26/replace-tool-for-level-designers

    /// <summary>
    /// Replace tool window,available under the Tools menu.
    /// </summary>
    public class ReplaceTool : EditorWindow
    {
        private ReplaceData m_Data;
        private SerializedObject m_SerializedData;
        // Prefab variable from m_Data object. Using SerializedProperty for integrated Undo
        private SerializedProperty m_ReplaceObjectField;
        // Scroll position for list of selected objects
        private Vector2 m_SelectObjectScrollPosition;


        private void OnGUI()
        {
            InitDataIfNeeded();
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(m_ReplaceObjectField);
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Selected objects to replace", EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            // Saving number of objects to replace.
            int objectToReplaceCount = m_Data.objectsToReplace != null ? m_Data.objectsToReplace.Length : 0;
            EditorGUILayout.IntField("Object count", objectToReplaceCount);
            EditorGUI.indentLevel++;

            // Printing information when no object is selected on scene.
            if (objectToReplaceCount == 0)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Select objects in the hierarchy to replace them", EditorStyles.wordWrappedLabel);
            }

            // Read-only scroll view with selected game objects.
            m_SelectObjectScrollPosition = EditorGUILayout.BeginScrollView(m_SelectObjectScrollPosition);

            GUI.enabled = false;
            if (m_Data && m_Data.objectsToReplace != null)
            {
                foreach (var go in m_Data.objectsToReplace)
                {
                    EditorGUILayout.ObjectField(go, typeof(GameObject), true);
                }
            }

            GUI.enabled = true;

            EditorGUILayout.EndScrollView();
            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();

            if (GUILayout.Button("Replace"))
            {
                // Check if replace object is assigned.
                if (!m_ReplaceObjectField.objectReferenceValue)
                {
                    Debug.LogErrorFormat("{0}", "No prefab to replace with!");
                    return;
                }
                // Check if there are objects to replace.
                if (m_Data.objectsToReplace.Length == 0)
                {
                    Debug.LogErrorFormat("{0}", "No objects to replace!");
                    return;
                }

                ReplaceSelectedObjects(m_Data.objectsToReplace, m_Data.replacementPrefab);
            }

            EditorGUILayout.Separator();
            m_SerializedData.ApplyModifiedProperties();

        }

        private void OnInspectorUpdate()
        {
            if (m_SerializedData != null && m_SerializedData.UpdateIfRequiredOrScript())
                Repaint();
        }

        private void OnSelectionChange()
        {
            InitDataIfNeeded();
            SelectionMode objectFilter = SelectionMode.Unfiltered ^ ~(SelectionMode.Assets | SelectionMode.DeepAssets | SelectionMode.Deep);
            Transform[] selection = Selection.GetTransforms(objectFilter);

            m_Data.objectsToReplace = selection.Select(s => s.gameObject).ToArray();

            if (m_SerializedData.UpdateIfRequiredOrScript())
                Repaint();
        }


        // Register menu item to open Window
        [MenuItem("Strungerhulder/Replace with Prefab")]
        public static void ShowWindow()
        {
            var window = GetWindow<ReplaceTool>();
            window.Show();
        }


        /// <summary>
        /// Replaces game objects with provided replace object.
        /// </summary>
        /// <param name="objectToReplace">Game Objects to replace.</param>
        /// <param name="replaceObject">Prefab that will be instantiated in place of the objects to replace.</param>
        internal static void ReplaceSelectedObjects(GameObject[] objectToReplace, GameObject replaceObject)
        {
            var newInstances = new int[objectToReplace.Length];

            for (int i = 0; i < objectToReplace.Length; i++)
            {
                var go = objectToReplace[i];

                Undo.RegisterCompleteObjectUndo(go, "Saving game object state");

                var sibling = go.transform.GetSiblingIndex();
                var inst = (GameObject)PrefabUtility.InstantiatePrefab(replaceObject);

                newInstances[i] = inst.GetInstanceID();

                inst.transform.position = go.transform.position;
                inst.transform.rotation = go.transform.rotation;
                inst.transform.parent = go.transform.parent;
                inst.transform.localScale = go.transform.localScale;
                inst.transform.SetSiblingIndex(sibling);

                Undo.RegisterCreatedObjectUndo(inst, "Replacement creation.");

                foreach (Transform child in go.transform)
                {
                    Undo.SetTransformParent(child, inst.transform, "Parent Change");
                }
                Undo.DestroyObjectImmediate(go);
            }

            Selection.instanceIDs = newInstances;
        }


        private void InitDataIfNeeded()
        {
            if (!m_Data)
            {
                m_Data = ScriptableObject.CreateInstance<ReplaceData>();
                m_SerializedData = null;
            }

            // If m_Data was not wrapped into SerializedObject, wrap it
            if (m_SerializedData == null)
            {
                m_SerializedData = new SerializedObject(m_Data);
                m_ReplaceObjectField = null;
            }

            // If prefab field was not assigned as SerializedProperty, assign it
            if (m_ReplaceObjectField == null)
                m_ReplaceObjectField = m_SerializedData.FindProperty("replacementPrefab");
        }
    }

    /// <summary>
    /// Data class for replace tool.
    /// </summary>
    public class ReplaceData : ScriptableObject
    {
        public GameObject replacementPrefab;
        public GameObject[] objectsToReplace;
    }
}

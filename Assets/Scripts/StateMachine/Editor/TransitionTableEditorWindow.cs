using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.StateMachines.Editor
{
    internal class TransitionTableEditorWindow : EditorWindow
    {
        private static TransitionTableEditorWindow m_Window;
        private static readonly string m_uxmlPath = "Assets/Scripts/StateMachine/Editor/TransitionTableEditorWindow.uxml";
        private static readonly string m_ussPath = "Assets/Scripts/StateMachine/Editor/TransitionTableEditorWindow.uss";
        private bool m_DoRefresh;

        private UnityEditor.Editor m_TransitionTableEditor;

        [MenuItem("Transition Table Editor", menuItem = "Strungerhulder/Transition Table Editor")]
        internal static void Display()
        {
            if (m_Window == null)
                m_Window = GetWindow<TransitionTableEditorWindow>("Transition Table Editor");

            m_Window.Show();
        }

        private void OnEnable()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(m_uxmlPath);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(m_ussPath);

            rootVisualElement.Add(visualTree.CloneTree());

            string labelClass = $"label-{(EditorGUIUtility.isProSkin ? "pro" : "personal")}";
            rootVisualElement.Query<Label>().Build().ForEach(label => label.AddToClassList(labelClass));

            rootVisualElement.styleSheets.Add(styleSheet);

            minSize = new Vector2(480, 360);

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode)
                m_DoRefresh = true;
        }

        /// <summary>
        /// Update list every time we gain focus
        /// </summary>
        private void OnFocus()
        {
            // Calling CreateListView() from here when the window is docked
            // throws a NullReferenceException in UnityEditor.DockArea:OnEnable().
            if (m_DoRefresh == false)
                m_DoRefresh = true;
        }

        private void OnLostFocus()
        {
            ListView listView = rootVisualElement.Q<ListView>(className: "table-list");
            listView.onSelectionChanged -= OnListSelectionChanged;
        }

        private void Update()
        {
            if (!m_DoRefresh)
                return;

            CreateListView();
            m_DoRefresh = false;
        }

        private void CreateListView()
        {
            var assets = FindAssets();
            ListView listView = rootVisualElement.Q<ListView>(className: "table-list");

            listView.makeItem = null;
            listView.bindItem = null;

            listView.itemsSource = assets;
            listView.itemHeight = 16;
            string labelClass = $"label-{(EditorGUIUtility.isProSkin ? "pro" : "personal")}";
            listView.makeItem = () =>
            {
                var label = new Label();
                label.AddToClassList(labelClass);
                return label;
            };
            listView.bindItem = (element, i) => ((Label)element).text = assets[i].name;
            listView.selectionType = SelectionType.Single;

            listView.onSelectionChanged -= OnListSelectionChanged;
            listView.onSelectionChanged += OnListSelectionChanged;

            if (m_TransitionTableEditor && m_TransitionTableEditor.target)
                listView.selectedIndex = System.Array.IndexOf(assets, m_TransitionTableEditor.target);
        }

        private void OnListSelectionChanged(List<object> list)
        {
            IMGUIContainer editor = rootVisualElement.Q<IMGUIContainer>(className: "table-editor");
            editor.onGUIHandler = null;

            if (list.Count == 0)
                return;

            var table = (TransitionTableSO)list[0];
            if (table == null)
                return;

            if (m_TransitionTableEditor == null)
                m_TransitionTableEditor = UnityEditor.Editor.CreateEditor(table, typeof(TransitionTableEditor));
            else if (m_TransitionTableEditor.target != table)
                UnityEditor.Editor.CreateCachedEditor(table, typeof(TransitionTableEditor), ref m_TransitionTableEditor);

            editor.onGUIHandler = () =>
            {
                if (!m_TransitionTableEditor.target)
                {
                    editor.onGUIHandler = null;
                    return;
                }

                ListView listView = rootVisualElement.Q<ListView>(className: "table-list");
                if ((Object)listView.selectedItem != m_TransitionTableEditor.target)
                {
                    var i = listView.itemsSource.IndexOf(m_TransitionTableEditor.target);
                    listView.selectedIndex = i;
                    if (i < 0)
                    {
                        editor.onGUIHandler = null;
                        return;
                    }
                }

                m_TransitionTableEditor.OnInspectorGUI();
            };
        }


        private TransitionTableSO[] FindAssets()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(TransitionTableSO)}");
            var assets = new TransitionTableSO[guids.Length];
            for (int i = 0; i < guids.Length; i++)
                assets[i] = AssetDatabase.LoadAssetAtPath<TransitionTableSO>(AssetDatabase.GUIDToAssetPath(guids[i]));

            return assets;
        }
    }
}

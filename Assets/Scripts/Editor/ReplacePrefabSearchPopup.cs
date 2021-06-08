using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static UnityEditor.EditorJsonUtility;
using static UnityEngine.Application;

namespace Strungerhulder.EditorTools.Replacer
{
    internal class ReplacePrefabSearchPopup : EditorWindow
    {
        private const float k_PreviewHeight = 128;

        private static ReplacePrefabSearchPopup s_Window;
        private static Styles s_Styles;

        private static Event s_Event => Event.current;
        private static string s_AssetPath => Path.Combine(dataPath.Remove(dataPath.Length - 7, 7), "Library", "ReplacePrefabTreeState.asset");

        private bool m_HasSelection => m_Tree.state.selectedIDs.Count > 0;
        private int m_SelectedID => m_Tree.state.selectedIDs[0];
        private GameObject m_Instance => EditorUtility.InstanceIDToObject(m_SelectedID) as GameObject;

        private SearchField m_SearchField;
        private PrefabSelectionTreeView m_Tree;
        private ViewState m_ViewState;

        private Vector2 m_StartPos;
        private Vector2 m_StartSize;
        private Vector2 m_LastSize;

        private GameObjectPreview m_SelectionPreview = new GameObjectPreview();


        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            m_Tree.Cleanup();
        }

        private void OnGUI()
        {
            if (s_Event.type == EventType.KeyDown && s_Event.keyCode == KeyCode.Escape)
            {
                if (m_Tree.hasSearch)
                    m_Tree.searchString = "";
                else
                    Close();
            }

            if (focusedWindow != this)
                Close();

            if (s_Styles == null)
                s_Styles = new Styles();

            DoToolbar();
            DoTreeView();
            DoSelectionPreview();
        }


        public static void Show(Rect rect)
        {
            var s_Windows = Resources.FindObjectsOfTypeAll<ReplacePrefabSearchPopup>();

            s_Window = s_Windows.Length != 0 ? s_Windows[0] : CreateInstance<ReplacePrefabSearchPopup>();

            s_Window.Init();

            s_Window.m_StartPos = rect.position;
            s_Window.m_StartSize = rect.size;

            s_Window.position = new Rect(rect.position, rect.size);
            // Need to predict start s_Window size to avoid trash frame
            s_Window.SetInitialSize();

            // This type of s_Window supports resizing, but is also persistent, so we need to close it manually
            s_Window.ShowPopup();

            //onSelectEntry += _ => s_Window.Close();
        }

        public new void Close()
        {
            SaveState();
            base.Close();
        }


        private void Init()
        {
            m_ViewState = CreateInstance<ViewState>();

            if (File.Exists(s_AssetPath))
                FromJsonOverwrite(File.ReadAllText(s_AssetPath), m_ViewState);

            m_Tree = new PrefabSelectionTreeView(m_ViewState.m_TreeViewState);
            m_Tree.OnSelectEntry += OnSelectEntry;

            AssetPreview.SetPreviewTextureCacheSize(m_Tree.RowsCount);

            m_SearchField = new SearchField();
            m_SearchField.downOrUpArrowKeyPressed += m_Tree.SetFocusAndEnsureSelectedItem;
            m_SearchField.SetFocus();
        }

        private void OnSelectEntry(GameObject prefab)
        {
            ReplaceTool.ReplaceSelectedObjects(Selection.gameObjects, prefab);
        }

        private void SaveState() => File.WriteAllText(s_AssetPath, ToJson(m_ViewState));

        private void DoToolbar()
        {
            m_Tree.searchString = m_SearchField.OnToolbarGUI(m_Tree.searchString);

            GUILayout.Label("Replace With...", s_Styles.headerLabel);
        }

        private void DoTreeView()
        {
            var rect = GUILayoutUtility.GetRect(0, 10000, 0, 10000);

            rect.x += 2;
            rect.width -= 4;

            rect.y += 2;
            rect.height -= 4;

            m_Tree.OnGUI(rect);
        }

        private void DoSelectionPreview()
        {
            if (m_HasSelection && m_Tree.IsRenderable(m_SelectedID))
            {
                SetSize(m_StartSize.x, m_StartSize.y + k_PreviewHeight);
                var previewRect = GUILayoutUtility.GetRect(position.width, k_PreviewHeight);

                m_SelectionPreview.CreatePreviewForTarget(m_Instance);
                m_SelectionPreview.RenderInteractivePreview(previewRect);

                m_SelectionPreview.DrawPreviewTexture(previewRect);
            }
            else
                SetSize(m_StartSize.x, m_StartSize.y);
        }

        private void SetInitialSize()
        {
            if (m_HasSelection && m_Tree.IsRenderable(m_SelectedID))
                SetSize(m_StartSize.x, m_StartSize.y + k_PreviewHeight);
            else
                SetSize(m_StartSize.x, m_StartSize.y);
        }

        private void SetSize(float width, float height)
        {
            var newSize = new Vector2(width, height);
            if (newSize != m_LastSize)
            {
                m_LastSize = newSize;
                position = new Rect(position.x, position.y, width, height);
            }
        }


        private class ViewState : ScriptableObject
        {
            public TreeViewState m_TreeViewState = new TreeViewState();
        }

        private class Styles
        {
            public GUIStyle headerLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold
            };
        }
    }
}

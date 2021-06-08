using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Strungerhulder.EditorTools.Replacer
{
    internal class PrefabSelectionTreeView : TreeView
    {
        public int RowsCount => m_Rows.Count;
        public Action<GameObject> OnSelectEntry;


        private static Texture2D s_PrefabOnIcon = EditorGUIUtility.IconContent("Prefab On Icon").image as Texture2D;
        private static Texture2D s_PrefabVariantOnIcon = EditorGUIUtility.IconContent("PrefabVariant On Icon").image as Texture2D;
        private static Texture2D s_FolderIcon = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;
        private static Texture2D s_FolderOnIcon = EditorGUIUtility.IconContent("Folder On Icon").image as Texture2D;

        private static GUIStyle s_WhiteLabel;
        private static GUIStyle s_Foldout;


        private Event e_Event => Event.current;

        private List<TreeViewItem> m_Rows = new List<TreeViewItem>();
        private HashSet<string> m_Paths = new HashSet<string>();

        private Dictionary<int, RenderTexture> m_PreviewCache = new Dictionary<int, RenderTexture>();
        private HashSet<int> m_RenderableItems = new HashSet<int>();

        private GameObjectPreview m_ItemPreview = new GameObjectPreview();
        private GUIContent m_ItemContent = new GUIContent();

        private int m_SelectedID;


        public override void OnGUI(Rect rect)
        {
            if (s_WhiteLabel == null)
                s_WhiteLabel = new GUIStyle(EditorStyles.label) { normal = { textColor = EditorStyles.whiteLabel.normal.textColor } };

            base.OnGUI(rect);
        }

        public PrefabSelectionTreeView(TreeViewState state) : base(state)
        {
            foldoutOverride = FoldoutOverride;
            Reload();
        }

        public void Cleanup()
        {
            foreach (var texture in m_PreviewCache.Values)
                Object.DestroyImmediate(texture);
        }

        public bool IsRenderable(int id) => m_RenderableItems.Contains(id);


        protected override bool CanMultiSelect(TreeViewItem item) => false;

        protected override void DoubleClickedItem(int id)
        {
            if (IsPrefabAsset(id, out var prefab))
                OnSelectEntry(prefab);
            else
                SetExpanded(id, !IsExpanded(id));
        }

        protected override void KeyEvent()
        {
            var key = e_Event.keyCode;
            if (key == KeyCode.KeypadEnter || key == KeyCode.Return)
                DoubleClickedItem(m_SelectedID);
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            if (selectedIds.Count > 0)
                m_SelectedID = selectedIds[0];
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(0, -1);
            m_Rows.Clear();
            m_Paths.Clear();

            foreach (var guid in AssetDatabase.FindAssets("t:Prefab"))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var splits = path.Split('/');
                var depth = splits.Length - 2;

                if (splits[0] != "Assets")
                    break;

                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                AddFoldersItems(splits);
                AddPrefabItem(asset, depth);
            }

            SetupParentsAndChildrenFromDepths(root, m_Rows);

            return root;
        }

        protected override float GetCustomRowHeight(int row, TreeViewItem item)
        {
            // Hide folders during search
            if (!IsPrefabAsset(item.id, out _) && hasSearch)
                return 0;

            return 20;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var rect = args.rowRect;
            var item = args.item;

            var isRenderable = IsRenderable(item.id);
            var isSelected = IsSelected(item.id);
            var isFocused = HasFocus() && isSelected;
            var isPrefab = IsPrefabAsset(item.id, out var prefab);
            var isFolder = !isPrefab;

            if (isFolder && hasSearch)
                return;

            if (isFolder)
            {
                if (rect.Contains(e_Event.mousePosition) && e_Event.type == EventType.MouseUp)
                {
                    SetSelection(new List<int> { item.id });
                    SetFocus();
                }
            }

            var labelStyle = isFocused ? s_WhiteLabel : EditorStyles.label;
            var contentIndent = GetContentIndent(item);

            customFoldoutYOffset = 2;
            m_ItemContent.text = item.displayName;

            rect.x += contentIndent;
            rect.width -= contentIndent;

            var iconRect = new Rect(rect) { width = 20 };

            if (isPrefab)
            {
                var type = PrefabUtility.GetPrefabAssetType(prefab);
                var onIcon = type == PrefabAssetType.Regular ? s_PrefabOnIcon : s_PrefabVariantOnIcon;

                var labelRect = new Rect(rect);

                if (isRenderable)
                {
                    var previewRect = new Rect(rect) { width = 32, height = 32 };

                    if (!m_PreviewCache.TryGetValue(item.id, out var previewTexture))
                    {
                        m_ItemPreview.CreatePreviewForTarget(prefab);
                        m_ItemPreview.RenderInteractivePreview(previewRect);

                        if (m_ItemPreview.outputTexture)
                            CachePreview(item.id);
                    }

                    if (!previewTexture)
                        Repaint();
                    else
                        GUI.DrawTexture(iconRect, previewTexture, ScaleMode.ScaleAndCrop);

                    labelRect.x += iconRect.width;
                    labelRect.width -= iconRect.width + 24;

                    GUI.Label(labelRect, args.label, labelStyle);

                    if (isSelected)
                    {
                        var prefabIconRect = new Rect(iconRect) { x = rect.xMax - 24 };
                        GUI.Label(prefabIconRect, isFocused ? onIcon : item.icon);
                    }
                }
                else
                {
                    m_ItemContent.image = isSelected ? onIcon : item.icon;
                    GUI.Label(rect, m_ItemContent, labelStyle);
                }
            }
            else
            {
                m_ItemContent.image = isFocused ? s_FolderOnIcon : s_FolderIcon;
                GUI.Label(rect, m_ItemContent, labelStyle);
            }
        }


        private bool FoldoutOverride(Rect position, bool expandedState, GUIStyle style)
        {
            position.width = Screen.width;
            position.height = 20;
            position.y -= 2;

            expandedState = GUI.Toggle(position, expandedState, GUIContent.none, style);

            return expandedState;
        }

        private void CachePreview(int itemId)
        {
            var copy = new RenderTexture(m_ItemPreview.outputTexture);
            var previous = RenderTexture.active;
            Graphics.Blit(m_ItemPreview.outputTexture, copy);
            RenderTexture.active = previous;
            m_PreviewCache.Add(itemId, copy);
        }

        private bool IsPrefabAsset(int id, out GameObject prefab)
        {
            var obj = EditorUtility.InstanceIDToObject(id);

            if (obj is GameObject go)
            {
                prefab = go;
                return true;
            }

            prefab = null;
            return false;
        }

        private void AddFoldersItems(string[] splits)
        {
            for (int i = 1; i < splits.Length - 1; i++)
            {
                var split = splits[i];

                if (!m_Paths.Contains(split))
                {
                    m_Rows.Add(new TreeViewItem(split.GetHashCode(), i - 1, " " + split) { icon = s_FolderIcon });
                    m_Paths.Add(split);
                }
            }
        }

        private void AddPrefabItem(GameObject asset, int depth)
        {
            var id = asset.GetInstanceID();
            var content = new GUIContent(EditorGUIUtility.ObjectContent(asset, asset.GetType()));

            if (GameObjectPreview.HasRenderableParts(asset))
                m_RenderableItems.Add(id);

            m_Rows.Add(new TreeViewItem(id, depth, content.text)
            {
                icon = content.image as Texture2D
            });
        }
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Strungerhulder.EditorTools.SceneSelectorTool.Internal;

namespace Strungerhulder.EditorTools.SceneSelectorTool
{
    public partial class SceneSelector : EditorWindow
    {
        private class PreferencesWindow : EditorWindow
        {
            private class Styles
            {
                public GUIStyle itemBorder;
                public GUIStyle buttonVisibilityOn;
                public GUIStyle buttonVisibilityOff;
            }

            public static float colorMarkerFieldSize = Mathf.Ceil(Helper.k_ColorMarkerNormalSize * 1.41f + 8.0f);

            private const string k_WindowCaption = "Scene Selector Preferences";
            private const float k_HeaderHeight = 0.0f;
            private const float k_ItemHeight = 24.0f;
            private const float k_VisibilityButtonSize = 16.0f;

            private static readonly Color s_ColorMarkerFieldSize = new Color(1.0f, 1.0f, 1.0f, 0.16f);

            private SceneSelector m_Owner;
            private ColorSelectorWindow m_ColorSelectorWindow;
            private ReorderableList m_ItemsReorderableList;
            private Styles m_Styles;
            private Vector2 m_WindowScrollPosition;

            private List<Item> m_Items => m_Owner.m_Storage.items;


            public static PreferencesWindow Open(SceneSelector owner)
            {
                var window = GetWindow<PreferencesWindow>(true, k_WindowCaption, true);

                window.Init(owner);
                return window;
            }

            private void OnEnable() => wantsMouseMove = true;

            private void OnDisable()
            {
                m_Owner.SaveStorage();

                if (m_ColorSelectorWindow != null)
                    m_ColorSelectorWindow.Close();
            }

            private void OnGUI()
            {
                EnsureStyles();
                Helper.RepaintOnMouseMove(this);
                DrawWindow();
            }


            public void RepaintAll()
            {
                RepaintOwner();
                Repaint();
            }


            private void Init(SceneSelector owner)
            {
                m_Owner = owner;
                CreateReorderableList();
            }

            private void CreateReorderableList()
            {
                m_ItemsReorderableList = new ReorderableList(m_Items, typeof(Item), true, true, false, false);
                m_ItemsReorderableList.drawElementCallback = DrawItem;
                m_ItemsReorderableList.drawElementBackgroundCallback = DrawItemBackground;
                m_ItemsReorderableList.onReorderCallback = OnReorder;
                m_ItemsReorderableList.headerHeight = k_HeaderHeight;
                m_ItemsReorderableList.elementHeight = k_ItemHeight;
            }

            private void DrawWindow()
            {
                using (var scrollScope = new EditorGUILayout.ScrollViewScope(m_WindowScrollPosition))
                {
                    GUILayout.Space(4.0f);
                    m_ItemsReorderableList.DoLayoutList();
                    m_WindowScrollPosition = scrollScope.scrollPosition;
                }
            }

            private void DrawItem(Rect rect, int index, bool isActive, bool isFocused)
            {
                var item = m_Items[index];
                var gameScene = item.gameSceneSO;

                if (gameScene != null)
                {
                    var colorMarkerRect = rect;
                    colorMarkerRect.width = colorMarkerRect.height;

                    if (Helper.DrawColorMarker(colorMarkerRect, item.color, true, true))
                    {
                        var colorSelectorRect = GUIUtility.GUIToScreenRect(colorMarkerRect);
                        m_ColorSelectorWindow = ColorSelectorWindow.Open(colorSelectorRect, this, item);
                    }

                    var itemLabelRect = rect;
                    itemLabelRect.x += colorMarkerRect.width;
                    itemLabelRect.width -= k_VisibilityButtonSize + colorMarkerRect.width;

                    GUI.Label(itemLabelRect, gameScene.name);

                    var visibilityButtonRect = new Rect(rect);
                    visibilityButtonRect.width = k_VisibilityButtonSize;
                    visibilityButtonRect.height = k_VisibilityButtonSize;
                    visibilityButtonRect.x = itemLabelRect.x + itemLabelRect.width;
                    visibilityButtonRect.y += (rect.height - visibilityButtonRect.height) * 0.5f;

                    var visibilityStyle = item.isVisible
                    ? m_Styles.buttonVisibilityOn
                    : m_Styles.buttonVisibilityOff;

                    if (GUI.Button(visibilityButtonRect, GUIContent.none, visibilityStyle))
                    {
                        item.isVisible = !item.isVisible;
                        RepaintOwner();
                    }
                }
            }

            private void DrawItemBackground(Rect rect, int index, bool isActive, bool isFocused)
            {
                ReorderableList.defaultBehaviours.DrawElementBackground(rect, index, isActive, isFocused, true);
                using (Helper.ReplaceColor.With(s_ColorMarkerFieldSize))
                {
                    GUI.Box(rect, GUIContent.none, m_Styles.itemBorder);
                }
            }

            private void OnReorder(ReorderableList _) => RepaintOwner();

            private void RepaintOwner() => m_Owner.Repaint();

            private void EnsureStyles()
            {
                if (m_Styles == null)
                {
                    m_Styles = new Styles();

                    m_Styles.itemBorder = new GUIStyle(GUI.skin.GetStyle("HelpBox"));

                    m_Styles.buttonVisibilityOn = new GUIStyle(GUI.skin.label);
                    m_Styles.buttonVisibilityOn.padding = new RectOffset(0, 0, 0, 0);
                    m_Styles.buttonVisibilityOn.normal.background = EditorGUIUtility.FindTexture("d_scenevis_visible");
                    m_Styles.buttonVisibilityOn.hover.background = EditorGUIUtility.FindTexture("d_scenevis_visible_hover");

                    m_Styles.buttonVisibilityOff = new GUIStyle(GUI.skin.label);
                    m_Styles.buttonVisibilityOff.padding = new RectOffset(0, 0, 0, 0);
                    m_Styles.buttonVisibilityOff.normal.background = EditorGUIUtility.FindTexture("d_scenevis_hidden");
                    m_Styles.buttonVisibilityOff.hover.background = EditorGUIUtility.FindTexture("d_scenevis_hidden_hover");
                }
            }
        }
    }
}

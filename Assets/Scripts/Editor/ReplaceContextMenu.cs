using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Strungerhulder.EditorTools.Replacer
{
    internal class ReplaceContextMenu
    {
        private static Type s_HierarchyType;

        private static EditorWindow s_FocusedWindow;
        private static IMGUIContainer s_HierarchyGUI;

        private static Vector2 s_MousePosition;
        private static bool s_HasExecuted;


        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            s_HierarchyType = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");

            EditorApplication.update += TrackFocusedHierarchy;
        }

        private static void TrackFocusedHierarchy()
        {
            if (s_FocusedWindow != EditorWindow.focusedWindow)
            {
                s_FocusedWindow = EditorWindow.focusedWindow;

                if (s_FocusedWindow?.GetType() == s_HierarchyType)
                {
                    if (s_HierarchyGUI != null)
                        s_HierarchyGUI.onGUIHandler -= OnFocusedHierarchyGUI;

                    s_HierarchyGUI = s_FocusedWindow.rootVisualElement.parent.Query<IMGUIContainer>();
                    s_HierarchyGUI.onGUIHandler += OnFocusedHierarchyGUI;
                }
            }
        }

        private static void OnFocusedHierarchyGUI()
        {
            // As Event.current is null during context-menu callback, we need to track mouse position on hierarchy GUI
            s_MousePosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        }

        [MenuItem("GameObject/Replace", true, priority = 0)]
        private static bool ReplaceSelectionValidate() => Selection.gameObjects.Length > 0;

        [MenuItem("GameObject/Replace", priority = 0)]
        private static void ReplaceSelection()
        {
            if (s_HasExecuted)
                return;

            var rect = new Rect(s_MousePosition, new Vector2(240, 360));

            ReplacePrefabSearchPopup.Show(rect);

            EditorApplication.delayCall += () => s_HasExecuted = false;
        }
    }
}

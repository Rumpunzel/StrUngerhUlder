using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Strungerhulder.EditorTools
{
    /// <summary>
    /// This editor window allows you to quickly locate and edit any ScriptableObject assets in the project folder.
    /// </summary>
    class ScriptableObjectBrowser : EditorWindow
    {
        private static GUIStyle s_ButtonStyle;

        private SortedDictionary<string, System.Type> m_Types = new SortedDictionary<string, System.Type>();
        private Dictionary<string, ScriptableObject> m_Assets = new Dictionary<string, ScriptableObject>();
        private Vector2 m_TypeScrollViewPosition;
        private Vector2 m_AssetScrollViewPosition;
        private int m_TypeIndex;
        private int m_LastAssetIndex;
        private bool m_ShowingTypes = true;


        public static GUIStyle ButtonStyle
        {
            get
            {
                if (s_ButtonStyle == null)
                {
                    s_ButtonStyle = EditorStyles.toolbarButton;
                    s_ButtonStyle.alignment = TextAnchor.MiddleLeft;
                }

                return s_ButtonStyle;
            }
        }


        private void OnEnable()
        {
            LoadData();
        }

        private void OnFocus()
        {
            LoadData();
        }

        private void LoadData()
        {
            if (m_ShowingTypes)
                GetTypes();
            else
                GetAssets();
        }

        [MenuItem("Strungerhulder/Scriptable Object Browser")]
        private static void ShowWindow()
        {
            GetWindow<ScriptableObjectBrowser>("Scriptable Objects");
        }

        private void OnGUI()
        {
            if (m_ShowingTypes)
            {
                GUILayout.Label("Scriptable Object Types", EditorStyles.largeLabel);

                if (GUILayout.Button("Refresh List"))
                    GetTypes();

                DrawTypeButtons();
            }
            else
            {
                GUILayout.Label(GetNiceName(m_Types.ElementAt(m_TypeIndex).Key), EditorStyles.largeLabel);

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Refresh List"))
                    GetAssets();

                if (GUILayout.Button("Back to Types"))
                {
                    GetTypes();
                    m_ShowingTypes = true;
                }

                GUILayout.EndHorizontal();

                DrawAssetButtons();
            }
        }

        /// <summary>
        /// Draws a scroll view list of Buttons for each ScriptableObject type.
        /// </summary>
        private void DrawTypeButtons()
        {
            m_TypeScrollViewPosition = GUILayout.BeginScrollView(m_TypeScrollViewPosition);

            for (int i = 0; i < m_Types.Count; i++)
            {
                if (GUILayout.Button(GetNiceName(m_Types.ElementAt(i).Key), EditorStyles.foldout))
                {
                    m_TypeIndex = i;
                    GetAssets();
                    m_ShowingTypes = false;
                }
            }

            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Draws a scroll view list of Buttons for each ScriptableObject asset file of selected type. 
        /// </summary>
        private void DrawAssetButtons()
        {
            m_AssetScrollViewPosition = GUILayout.BeginScrollView(m_AssetScrollViewPosition);

            for (int i = 0; i < m_Assets.Count; i++)
            {
                if (GUILayout.Button(GetNiceName(m_Assets.ElementAt(i).Value.name), ButtonStyle))
                {
                    Selection.activeObject = m_Assets.ElementAt(i).Value;

                    if (m_LastAssetIndex == i)
                        EditorGUIUtility.PingObject(m_Assets.ElementAt(i).Value);

                    m_LastAssetIndex = i;
                }
            }

            GUILayout.EndScrollView();
        }

        /// <summary>
        /// Gets all ScriptableObject types in project.
        /// </summary>
        private void GetTypes()
        {
            string[] GUIDs = AssetDatabase.FindAssets(
                "t:ScriptableObject",
                new string[] { "Assets/ScriptableObjects"
            });
            ScriptableObject[] SOs = new ScriptableObject[GUIDs.Length];

            for (int i = 0; i < GUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(GUIDs[i]);
                SOs[i] = (ScriptableObject)AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject));
            }

            m_Types.Clear();

            for (int i = 0; i < SOs.Length; i++)
            {
                string typeKey = SOs[i].GetType().Name;

                if (!m_Types.ContainsKey(typeKey))
                    m_Types.Add(typeKey, SOs[i].GetType());
            }
        }

        /// <summary>
        /// Gets all ScriptableObject asset files of selected type.
        /// </summary>
        private void GetAssets()
        {
            string[] GUIDs = AssetDatabase.FindAssets("t:" + m_Types.ElementAt(m_TypeIndex).Value.FullName);

            m_Assets.Clear();

            for (int i = 0; i < GUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(GUIDs[i]);
                var SO = AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableObject)) as ScriptableObject;

                m_Assets.Add(path, SO);
            }
        }

        /// <summary>
        /// Formats string of text to look prettier and more readable.
        /// </summary>
        private string GetNiceName(string text)
        {
            string niceText = text;

            niceText = ObjectNames.NicifyVariableName(niceText);
            niceText = niceText.Replace(" SO", "");
            niceText = niceText.Replace("-", " ");
            niceText = niceText.Replace("_", " ");

            return niceText;
        }
    }
}

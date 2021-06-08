using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Strungerhulder.EditorTools.SceneSelectorTool.Internal;
using Strungerhulder.SceneManagement.ScriptableObjects;

namespace Strungerhulder.EditorTools.SceneSelectorTool
{
    public partial class SceneSelector : EditorWindow, IHasCustomMenu
    {
        private const string k_PreferencesKey = "Strungerhulder.SceneSelector.Preferences";
        private const int k_ItemContentLeftPadding = 32;
        private static readonly GUIContent k_OpenPreferencesItemContent = new GUIContent("Open Preferences");

        private Styles m_Styles;
        private Storage m_Storage;
        private PreferencesWindow m_PreferencesWindow;
        private Vector2 m_WindowScrollPosition;
        private bool m_HasEmptyItems;

        private List<Item> m_Items => m_Storage.items;
        private Dictionary<string, Item> m_ItemsMap => m_Storage.itemsMap;


        [MenuItem("Strungerhulder/Scene Selector")]
        private static void Open() => GetWindow<SceneSelector>();

        private void OnEnable()
        {
            wantsMouseMove = true;
            LoadStorage();
            PopulateItems();
        }

        private void OnDisable()
        {
            if (m_PreferencesWindow != null)
                m_PreferencesWindow.Close();

            SaveStorage();
        }

        private void OnGUI()
        {
            EnsureStyles();
            Helper.RepaintOnMouseMove(this);
            RemoveEmptyItemsIfRequired();
            DrawWindow();
        }

        private void DrawWindow()
        {
            using (var scrollScope = new EditorGUILayout.ScrollViewScope(m_WindowScrollPosition))
            {
                GUILayout.Space(4.0f);
                DrawItems();
                m_WindowScrollPosition = scrollScope.scrollPosition;
            }

            if (GUILayout.Button("Reset list"))
            {
                //Force deletion of the storage
                m_Storage = new Storage();
                EditorPrefs.SetString(k_PreferencesKey, "");

                OnEnable(); //search the project and populate the scene list again
            }
        }

        private void DrawItems()
        {
            foreach (var item in m_Items)
            {
                DrawItem(item);
            }
        }

        private void DrawItem(Item item)
        {
            if (item.isVisible)
            {
                var gameSceneSO = item.gameSceneSO;

                if (gameSceneSO != null)
                {
                    if (GUILayout.Button(gameSceneSO.name, m_Styles.item))
                    {
                        Helper.OpenSceneSafe(gameSceneSO);
                    }

                    var colorMarkerRect = GUILayoutUtility.GetLastRect();
                    colorMarkerRect.width = colorMarkerRect.height;
                    colorMarkerRect.x += (m_Styles.item.padding.left - colorMarkerRect.width) * 0.5f;
                    Helper.DrawColorMarker(colorMarkerRect, item.color);
                }
                else
                {
                    // In case GameSceneSO was removed (see RemoveEmptyItemsIfRequired)
                    m_HasEmptyItems = true;
                }
            }
        }

        private void LoadStorage()
        {
            m_Storage = new Storage();

            if (EditorPrefs.HasKey(k_PreferencesKey))
            {
                var preferencesJSON = EditorPrefs.GetString(k_PreferencesKey);
                EditorJsonUtility.FromJsonOverwrite(preferencesJSON, m_Storage);
            }
        }

        private void SaveStorage()
        {
            var preferencesJSON = EditorJsonUtility.ToJson(m_Storage);
            EditorPrefs.SetString(k_PreferencesKey, preferencesJSON);
        }

        private void PopulateItems()
        {
            var gameSceneSOs = new List<GameSceneSO>();
            Helper.FindAssetsByType(gameSceneSOs);

            foreach (var gameSceneSO in gameSceneSOs)
            {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(gameSceneSO, out var guid, out long _))
                {
                    if (m_ItemsMap.TryGetValue(guid, out var item))
                        item.gameSceneSO = gameSceneSO;
                    else
                    {
                        item = new Item()
                        {
                            gameSceneSO = gameSceneSO,
                            guid = guid,
                            color = Helper.GetDefaultColor(gameSceneSO)
                        };

                        m_Items.Add(item);
                        m_ItemsMap.Add(guid, item);
                    }
                }
            }
        }

        private void RemoveEmptyItemsIfRequired()
        {
            if (m_HasEmptyItems)
            {
                for (int i = m_Items.Count - 1; i >= 0; --i)
                {
                    var sceneItem = m_Items[i];

                    if (sceneItem == null || sceneItem.gameSceneSO == null)
                    {
                        m_Items.RemoveAt(i);
                        m_ItemsMap.Remove(sceneItem.guid);
                    }
                }
            }
            m_HasEmptyItems = false;
        }


        private void EnsureStyles()
        {
            if (m_Styles == null)
            {
                m_Styles = new Styles();

                m_Styles.item = "MenuItem";
                m_Styles.item.padding.left = k_ItemContentLeftPadding;
            }
        }

        private void OpenPreferences() => m_PreferencesWindow = PreferencesWindow.Open(this);

        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(k_OpenPreferencesItemContent, false, OpenPreferences);
        }
    }
}

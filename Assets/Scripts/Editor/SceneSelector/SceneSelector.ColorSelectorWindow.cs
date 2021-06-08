using UnityEditor;
using UnityEngine;
using Strungerhulder.EditorTools.SceneSelectorTool.Internal;

namespace Strungerhulder.EditorTools.SceneSelectorTool
{
    public partial class SceneSelector : EditorWindow
    {
        private class ColorSelectorWindow : EditorWindow
        {
            private static readonly float s_CellSize = PreferencesWindow.colorMarkerFieldSize * 2.0f;
            private static readonly Color s_CellBackColor = new Color(0.0f, 0.0f, 0.0f, 0.1f);
            private static readonly Vector2 s_CellOffset = new Vector2(1.0f, 1.0f);
            private static readonly Vector2Int s_Count = new Vector2Int(5, 5);

            private PreferencesWindow m_Owner;
            private Color[,] m_Colors;
            private Item m_Item;


            public static ColorSelectorWindow Open(Rect rect, PreferencesWindow owner, Item item)
            {
                var window = CreateInstance<ColorSelectorWindow>();
                window.Init(rect, owner, item);

                return window;
            }

            private void Init(Rect rect, PreferencesWindow owner, Item item)
            {
                var size = (Vector2)s_Count * s_CellSize;

                ShowAsDropDown(rect, size);
                m_Owner = owner;
                m_Item = item;
            }

            private void OnEnable()
            {
                wantsMouseMove = true;
                InitColors();
            }

            private void OnGUI()
            {
                Helper.RepaintOnMouseMove(this);
                DrawMarkers();
            }

            private void DrawMarkers()
            {
                var size = new Vector2(s_CellSize, s_CellSize);

                for (int x = 0; x < s_Count.x; ++x)
                {
                    for (int y = 0; y < s_Count.y; ++y)
                    {
                        var color = m_Colors[x, y];
                        var position = size * new Vector2(x, y);
                        var rect = new Rect(position, size);
                        {
                            var cellBackRect = rect;
                            cellBackRect.position += s_CellOffset;
                            cellBackRect.size -= s_CellOffset * 2.0f;
                            EditorGUI.DrawRect(cellBackRect, s_CellBackColor);
                        }
                        if (Helper.DrawColorMarker(rect, color, true, true))
                        {
                            m_Item.color = color;
                            m_Owner.RepaintAll();
                            Close();
                        }
                    }
                }
            }

            private void InitColors()
            {
                var count = s_Count.x * s_Count.y;
                m_Colors = new Color[s_Count.x, s_Count.y];

                for (int x = 0; x < s_Count.x; ++x)
                {
                    var h = x * s_Count.y;

                    for (int y = 0; y < s_Count.y; ++y)
                    {
                        float hue = (float)(h + y) / count;
                        m_Colors[x, y] = Color.HSVToRGB(hue, 1.0f, 1.0f);
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

namespace Strungerhulder.Grids
{
    public class CustomGrid
    {
        private int m_XSize, m_YSize;
        private float m_CellSize;
        private bool m_MirrorX, m_MirrorY;

        private int[,] m_GridArray;
#if UNITY_EDITOR
        private TextMesh[,] m_DebugTextArray;
#endif


        public CustomGrid(int xSize, int ySize, float cellSize, bool mirrorX = true, bool mirrorY = true)
        {
            m_XSize = xSize;
            m_YSize = ySize;
            m_CellSize = cellSize;
            m_MirrorX = mirrorX;
            m_MirrorY = mirrorY;

            m_GridArray = new int[m_XSize * (m_MirrorX ? 2 : 1), m_YSize * (m_MirrorY ? 2 : 1)];
#if UNITY_EDITOR
            m_DebugTextArray = new TextMesh[m_XSize * (m_MirrorX ? 2 : 1), m_YSize * (m_MirrorY ? 2 : 1)];
#endif

            for (int i = 0; i < m_GridArray.GetLength(0); i++)
            {
                int x = GetActualXAxis(i);

                for (int j = 0; j < m_GridArray.GetLongLength(1); j++)
                {
                    int y = GetActualYAxis(j);
#if UNITY_EDITOR
                    m_DebugTextArray[i, j] = UtilsClass.CreateWorldText(
                        m_GridArray[i, j].ToString(),
                        null,
                        GetWorldPosition(x, y) + new Vector3(cellSize, 0f, cellSize) * .5f,
                        10,
                        Color.white,
                        TextAnchor.MiddleCenter
                    );
#endif

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white);
                }
            }

            Debug.DrawLine(GetWorldPosition(xSize, 0), GetWorldPosition(xSize, m_YSize), Color.white);
            Debug.DrawLine(GetWorldPosition(0, m_YSize), GetWorldPosition(m_XSize, m_YSize), Color.white);
        }


        public void SetValue(int x, int y, int value)
        {
            if (!(x >= 0 && y >= 0 && x < m_XSize && y < m_YSize))
                return;

            x = GetActualXAxis(x);
            y = GetActualYAxis(y);

            m_GridArray[x, y] = value;
#if UNITY_EDITOR
            m_DebugTextArray[x, y].text = m_GridArray[x, y].ToString();
#endif
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetValue(x, y, value);
        }


        //private int GetCoordinate()

        private Vector3 GetWorldPosition(int x, int y) => new Vector3(x, 0f, y) * m_CellSize;

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / m_CellSize);
            y = Mathf.FloorToInt(worldPosition.y / m_CellSize);

            x = GetActualXAxis(x);
            y = GetActualYAxis(y);
        }

        private int GetActualXAxis(int x) => (m_MirrorX ? x - m_XSize : x);

        private int GetActualYAxis(int y) => (m_MirrorY ? y - m_XSize : y);
    }
}

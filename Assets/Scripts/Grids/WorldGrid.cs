using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strungerhulder.Grids
{
    public class WorldGrid : MonoBehaviour
    {
        [SerializeField] private int m_GridXDimension;
        [SerializeField] private int m_GridYDimension;
        [SerializeField] private float m_CellSize;

        private CustomGrid m_Grid;


        // Start is called before the first frame update
        private void Start()
        {
            m_Grid = new CustomGrid(m_GridXDimension, m_GridYDimension, m_CellSize);
        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}

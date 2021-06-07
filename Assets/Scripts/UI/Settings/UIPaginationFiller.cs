using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Strungerhulder.UI.Settings
{
    public class UIPaginationFiller : MonoBehaviour
    {
        [SerializeField] private Image m_ImagePaginationPrefab = default;
        [SerializeField] private Transform m_ParentPagination = default;

        [SerializeField] private Sprite m_EmptyPagination = default;
        [SerializeField] private Sprite m_FilledPagination = default;

        private List<Image> m_InstantiatedImages = default;


        private void Start()
        {
            m_InstantiatedImages = new List<Image>();
        }


        public void SetPagination(int paginationCount, int selectedPaginationIndex)
        {
            if (m_InstantiatedImages == null)
                m_InstantiatedImages = new List<Image>();

            //instanciate pagination images from the prefab
            int maxCount = Mathf.Max(paginationCount, m_InstantiatedImages.Count);

            if (maxCount > 0)
            {
                for (int i = 0; i < maxCount; i++)
                {
                    if (i >= m_InstantiatedImages.Count)
                    {
                        Image instantiatedImage = Instantiate(m_ImagePaginationPrefab, m_ParentPagination);
                        m_InstantiatedImages.Add(instantiatedImage);
                    }

                    if (i < paginationCount)
                        m_InstantiatedImages[i].gameObject.SetActive(true);
                    else
                        m_InstantiatedImages[i].gameObject.SetActive(false);
                }

                SetCurrentPagination(selectedPaginationIndex);
            }
        }

        public void SetCurrentPagination(int selectedPaginationIndex)
        {
            if (m_InstantiatedImages.Count > selectedPaginationIndex)
            {
                for (int i = 0; i < m_InstantiatedImages.Count; i++)
                {
                    if (i == selectedPaginationIndex)
                    {
                        m_InstantiatedImages[i].sprite = m_FilledPagination;
                    }
                    else
                    {
                        m_InstantiatedImages[i].sprite = m_EmptyPagination;
                    }
                }
            }
            else
                Debug.LogError("Error in pagination number");
        }
    }
}

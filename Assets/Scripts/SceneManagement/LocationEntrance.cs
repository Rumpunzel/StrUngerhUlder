using UnityEngine;
using Strungerhulder.SceneManagement.ScriptableObjects;

namespace Strungerhulder.SceneManagement
{
    public class LocationEntrance : MonoBehaviour
    {
        [Header("Asset References")]
        [SerializeField] private PathSO m_EntrancePath;

        public PathSO EntrancePath => m_EntrancePath;
    }
}

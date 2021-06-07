using UnityEngine;

namespace Strungerhulder
{
    public class LocationEntrance : MonoBehaviour
    {
        [Header("Asset References")]
        [SerializeField] private PathSO m_EntrancePath;

        public PathSO EntrancePath => m_EntrancePath;
    }
}

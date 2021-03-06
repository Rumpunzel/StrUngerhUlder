using UnityEngine;
using Strungerhulder.SceneManagement.ScriptableObjects;

namespace Strungerhulder.RuntimeAnchors
{
    [CreateAssetMenu(fileName = "New PathAnchor", menuName = "Runtime Anchors/Path")]
    public class PathAnchor : RuntimeAnchorBase
    {
        [HideInInspector] public bool isSet = false; // Any script can check if the transform is null before using it, by just checking this bool

        private PathSO m_Path;
        public PathSO Path
        {
            get { return m_Path; }
            set
            {
                m_Path = value;
                isSet = m_Path != null;
            }
        }

        public void OnDisable()
        {
            m_Path = null;
            isSet = false;
        }
    }
}

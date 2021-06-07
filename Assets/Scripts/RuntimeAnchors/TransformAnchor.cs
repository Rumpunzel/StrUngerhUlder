using UnityEngine;

namespace Strungerhulder.RuntimeAnchors
{
    [CreateAssetMenu(menuName = "Runtime Anchors/Transform")]
    public class TransformAnchor : RuntimeAnchorBase
    {
        [HideInInspector] public bool isSet = false; // Any script can check if the transform is null before using it, by just checking this bool

        private Transform m_Transform;

        public Transform Transform
        {
            get { return m_Transform; }
            set
            {
                m_Transform = value;
                isSet = m_Transform != null;
            }
        }

        public void OnDisable()
        {
            m_Transform = null;
            isSet = false;
        }
    }
}

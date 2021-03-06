using UnityEngine;
using Strungerhulder.Input;

namespace Strungerhulder.Cameras
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private float m_ShoulderHeight = 1f;
        [SerializeField] private float m_DistanceOffGround = 23f;
        [SerializeField] private float m_DistanceFromFollow = 36f;

        [Space]
        [SerializeField] private float m_CameraAngleOffset = 45f;
        [SerializeField] private float m_CameraTurnAngle = 90f;
        [SerializeField] private float m_CameraTurnSmoothing = .02f;

        [Space]
        [SerializeField] private float m_CameraMinZoom = 1f;
        [SerializeField] private float m_CameraMaxZoom = 16f;
        [SerializeField] private float m_CameraScrollSpeed = .02f;



        private Transform m_FollowTransform;
        private Vector3 m_CameraPosition;
        private Vector3 m_Offset;
        private float m_CameraAngle;
        private int m_CameraTurnIndex = 0;
        private float m_CameraZoom = 1f;


        private void Start()
        {
            m_CameraAngleOffset *= Mathf.Deg2Rad;
            m_CameraTurnAngle *= Mathf.Deg2Rad;
            m_CameraAngle = m_CameraAngleOffset;
        }

        private void Update()
        {
            if (!m_FollowTransform)
                return;

            m_CameraAngle = Mathf.Lerp(m_CameraAngle, m_CameraTurnIndex * m_CameraTurnAngle + m_CameraAngleOffset, m_CameraTurnSmoothing);
            m_CameraZoom = Mathf.Clamp(m_CameraZoom, 1f / m_CameraMaxZoom, 1f / m_CameraMinZoom);

            m_Offset = new Vector3(
                m_DistanceFromFollow * Mathf.Cos(m_CameraAngle) * Mathf.Sqrt(m_CameraZoom),
                0f,
                m_DistanceFromFollow * Mathf.Sin(m_CameraAngle) * Mathf.Sqrt(m_CameraZoom)
            );

            float thirdPersonFactor = m_FollowTransform.position.y * (1f - m_CameraZoom);

            m_CameraPosition = m_FollowTransform.position + m_Offset;
            m_CameraPosition.y = m_ShoulderHeight + m_DistanceOffGround * m_CameraZoom + thirdPersonFactor;

            this.transform.position = m_CameraPosition;

            this.transform.LookAt(new Vector3(
                m_FollowTransform.position.x,
                m_ShoulderHeight + thirdPersonFactor,
                m_FollowTransform.position.z
            ));
        }


        public void RotateCamera(bool rotateRight) => m_CameraTurnIndex += rotateRight ? 1 : -1;

        public void ZoomCamera(float value)
        {
            m_CameraZoom = Mathf.Lerp(m_CameraZoom, m_CameraZoom - value * m_CameraScrollSpeed, m_CameraTurnSmoothing);
        }


        public Transform FollowTransform
        {
            get { return m_FollowTransform; }
            set { m_FollowTransform = value; }
        }
    }
}

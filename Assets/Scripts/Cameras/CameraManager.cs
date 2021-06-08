using UnityEngine;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.Input;
using Strungerhulder.RuntimeAnchors;

namespace Strungerhulder.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private InputReader m_InputReader;
        [SerializeField] private FollowCamera m_MainCamera;

        [SerializeField] private TransformAnchor m_CameraTransformAnchor = default;

        [Header("Listening on channels")]
        [Tooltip("The CameraManager listens to this event, fired by objects in any scene, to adapt camera position")]
        [SerializeField] private TransformEventChannelSO m_FrameObjectChannel = default;


        public void SetupProtagonistVirtualCamera(Transform target)
        {
            m_MainCamera.FollowTransform = target;
        }

        private void OnEnable()
        {
            //inputReader.cameraMoveEvent += OnCameraMove;
            m_InputReader.RotateCamera += OnCameraRotate;
            m_InputReader.ZoomCamera += OnCameraZoom;

            if (m_FrameObjectChannel != null)
                m_FrameObjectChannel.onEventRaised += OnFrameObjectEvent;

            m_CameraTransformAnchor.Transform = m_MainCamera.transform;
        }

        private void OnDisable()
        {
            //inputReader.cameraMoveEvent -= OnCameraMove;
            m_InputReader.RotateCamera -= OnCameraRotate;
            m_InputReader.ZoomCamera -= OnCameraZoom;

            if (m_FrameObjectChannel != null)
                m_FrameObjectChannel.onEventRaised -= OnFrameObjectEvent;
        }


        public void OnCameraRotate(float direction) => m_MainCamera.RotateCamera(direction > 0);

        public void OnCameraZoom(float value) => m_MainCamera.ZoomCamera(value);


        private void OnFrameObjectEvent(Transform value)
        {
            SetupProtagonistVirtualCamera(value);
        }
    }
}

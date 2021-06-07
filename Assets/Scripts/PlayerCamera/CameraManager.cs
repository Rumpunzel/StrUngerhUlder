using UnityEngine;
using Strungerhulder.Events.ScriptableObjects;

namespace Strungerhulder.PlayerCamera
{
    public class CameraManager : MonoBehaviour
    {
        public InputReader inputReader;
        public Camera mainCamera;

        [SerializeField] private TransformAnchor m_CameraTransformAnchor = default;

        [Header("Listening on channels")]
        [Tooltip("The CameraManager listens to this event, fired by objects in any scene, to adapt camera position")]
        [SerializeField] private TransformEventChannelSO m_FrameObjectChannel = default;


        public void SetupProtagonistVirtualCamera(Transform target)
        {
            mainCamera.GetComponent<FollowCamera>().FollowTransform = target;
        }

        private void OnEnable()
        {
            //inputReader.cameraMoveEvent += OnCameraMove;

            if (m_FrameObjectChannel != null)
                m_FrameObjectChannel.onEventRaised += OnFrameObjectEvent;

            m_CameraTransformAnchor.Transform = mainCamera.transform;
        }

        private void OnDisable()
        {
            //inputReader.cameraMoveEvent -= OnCameraMove;

            if (m_FrameObjectChannel != null)
                m_FrameObjectChannel.onEventRaised -= OnFrameObjectEvent;
        }

        private void OnFrameObjectEvent(Transform value)
        {
            SetupProtagonistVirtualCamera(value);
        }
    }
}

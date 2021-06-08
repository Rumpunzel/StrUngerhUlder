using UnityEngine;
using UnityEngine.Events;
using Strungerhulder.Input;

namespace Strungerhulder.UI
{
    public class UIPause : MonoBehaviour
    {
        public UnityAction resumed = default;
        public UnityAction settingsScreenOpened = default;
        public UnityAction backToMainRequested = default;

        [SerializeField] private UIButtonSetter _ResumeButton = default;
        [SerializeField] private UIButtonSetter m_SettingsButton = default;
        [SerializeField] private UIButtonSetter m_BackToMenuButton = default;

        [SerializeField] private InputReader m_InputReader = default;


        private void OnEnable()
        {
            _ResumeButton.SetButton(true);
            m_InputReader.MenuCloseEvent += Resume;
            _ResumeButton.clicked += Resume;
            m_SettingsButton.clicked += OpenSettingsScreen;
            m_BackToMenuButton.clicked += BackToMainMenuConfirmation;
        }

        private void OnDisable()
        {
            m_InputReader.MenuCloseEvent -= Resume;

            _ResumeButton.clicked -= Resume;
            m_SettingsButton.clicked -= OpenSettingsScreen;
            m_BackToMenuButton.clicked -= BackToMainMenuConfirmation;
        }


        public void CloseScreen() => resumed.Invoke();


        private void Resume() => resumed.Invoke();
        private void OpenSettingsScreen() => settingsScreenOpened.Invoke();
        private void BackToMainMenuConfirmation() => backToMainRequested.Invoke();
    }
}

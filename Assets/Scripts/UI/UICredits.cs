using UnityEngine;
using UnityEngine.Events;
using Strungerhulder.Input;

namespace Strungerhulder.UI
{
    public class UICredits : MonoBehaviour
    {
        public UnityAction closeCreditsAction;

        [SerializeField] private InputReader m_InputReader = default;


        private void OnEnable() => m_InputReader.MenuCloseEvent += CloseCreditsScreen;
        private void OnDisable() => m_InputReader.MenuCloseEvent -= CloseCreditsScreen;


        public void SetCreditsScreen() { }
        public void CloseCreditsScreen() => closeCreditsAction.Invoke();
    }
}

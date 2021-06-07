using UnityEngine;
using UnityEngine.Events;

namespace Strungerhulder.UI
{
    public class UICredits : MonoBehaviour
    {
        public UnityAction closeCreditsAction;

        [SerializeField] private InputReader m_InputReader = default;


        private void OnEnable() => m_InputReader.menuCloseEvent += CloseCreditsScreen;
        private void OnDisable() => m_InputReader.menuCloseEvent -= CloseCreditsScreen;


        public void SetCreditsScreen() { }
        public void CloseCreditsScreen() => closeCreditsAction.Invoke();
    }
}

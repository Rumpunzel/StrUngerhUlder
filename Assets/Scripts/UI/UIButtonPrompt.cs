using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Strungerhulder.UI
{
    public class UIButtonPrompt : MonoBehaviour
    {
        [SerializeField] Image m_InteractionKeyBG = default;

        [SerializeField] TextMeshProUGUI m_InteractionKeyText = default;

        [SerializeField] Sprite m_ControllerSprite = default;
        [SerializeField] Sprite m_KeyboardSprite = default;

        [SerializeField] string m_InteractionKeyboardCode = default;
        [SerializeField] string m_InteractionJoystickKeyCode = default;


        public void SetButtonPrompt(bool isKeyboard)
        {
            if (!isKeyboard)
            {
                m_InteractionKeyBG.sprite = m_ControllerSprite;
                m_InteractionKeyText.text = m_InteractionJoystickKeyCode;
            }
            else
            {
                m_InteractionKeyBG.sprite = m_KeyboardSprite;
                m_InteractionKeyText.text = m_InteractionKeyboardCode;
            }
        }
    }
}

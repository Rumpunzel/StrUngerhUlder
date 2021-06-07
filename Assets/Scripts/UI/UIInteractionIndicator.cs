using UnityEngine;
using UnityEngine.Localization.Components;
using Strungerhulder.UI.ScriptableObjects;

namespace Strungerhulder.UI
{
    public class UIInteractionIndicator : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent m_InteractionName = default;

        [SerializeField] private UIButtonPrompt m_ButtonPromptSetter = default;


        public void FillInteractionPanel(InteractionSO interactionItem)
        {
            m_InteractionName.StringReference = interactionItem.InteractionName;

            bool isKeyboard = true;
            //	bool isKeyboard = !(Input.GetJoystickNames() != null && Input.GetJoystickNames().Length > 0);
            m_ButtonPromptSetter.SetButtonPrompt(isKeyboard);
        }
    }
}

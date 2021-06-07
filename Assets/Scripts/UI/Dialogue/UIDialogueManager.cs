using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.Dialogues.ScriptableObjects;

namespace Strungerhulder.UI.Dialogue
{
    public class UIDialogueManager : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent m_LineText = default;
        [SerializeField] private LocalizeStringEvent m_ActorNameText = default;
        [SerializeField] private UIDialogueChoicesManager m_ChoicesManager = default;
        [SerializeField] private DialogueChoicesChannelSO m_ShowChoicesEvent = default;


        private void Start() => m_ShowChoicesEvent.onEventRaised += ShowChoices;


        public void SetDialogue(LocalizedString dialogueLine, ActorSO actor)
        {
            m_ChoicesManager.gameObject.SetActive(false);
            m_LineText.StringReference = dialogueLine;
            m_ActorNameText.StringReference = actor.ActorName;
        }


        private void ShowChoices(List<Choice> choices)
        {
            m_ChoicesManager.FillChoices(choices);
            m_ChoicesManager.gameObject.SetActive(true);
        }

        private void HideChoices() => m_ChoicesManager.gameObject.SetActive(false);
    }
}

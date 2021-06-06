using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Localization;
using UnityEditor.Localization;

public enum DialogueType
{
	startDialogue,
	winDialogue,
	loseDialogue,
	defaultDialogue,

}

public enum ChoiceActionType
{
	doNothing,
	continueWithStep,

}

/// <summary>
/// A Dialogue is a list of consecutive DialogueLines. They play in sequence using the input of the player to skip forward.
/// In future versions it might contain support for branching conversations.
/// </summary>
[CreateAssetMenu(fileName = "newDialogue", menuName = "Dialogues/Dialogue Data")]
public class DialogueDataSO : ScriptableObject
{
	[SerializeField] private ActorSO m_Actor = default;
	[SerializeField] private List<LocalizedString> m_DialogueLines = default;
	[SerializeField] private List<Choice> m_Choices = default;
	[SerializeField] private DialogueType m_DialogueType = default;

	public ActorSO Actor => m_Actor;
	public List<LocalizedString> DialogueLines => m_DialogueLines;
	public List<Choice> Choices => m_Choices;
	public DialogueType DialogueType => m_DialogueType;


#if UNITY_EDITOR
	//TODO: Add support for branching conversations
	// Maybe add 2 (or more) special line slots which represent a choice in a conversation
	// Each line would also have an event associated, or another Dialogue
	private void OnEnable() => SetDialogueLines();

	private void SetDialogueLines()
	{
		m_DialogueLines.Clear();

		StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection("Questline Dialogue");

		if (collection != null)
		{
			int index = 0;
			LocalizedString dialogueLine = null;

			do
			{
				index++;
				string key = "L" + index + "-" + this.name;

				if (collection.SharedData.Contains(key))
				{
					dialogueLine = new LocalizedString() { TableReference = "Questline Dialogue", TableEntryReference = key };
					m_DialogueLines.Add(dialogueLine);
				}
				else
					dialogueLine = null;

			} while (dialogueLine != null);
		}
	}
#endif
}


[Serializable]
public class Choice
{
    public LocalizedString Response => m_Response;
    public DialogueDataSO NextDialogue => m_NextDialogue;
    public ChoiceActionType ActionType => m_ActionType;

	[SerializeField] private LocalizedString m_Response = default;
	[SerializeField] private DialogueDataSO m_NextDialogue = default;
	[SerializeField] private ChoiceActionType m_ActionType = default;
}

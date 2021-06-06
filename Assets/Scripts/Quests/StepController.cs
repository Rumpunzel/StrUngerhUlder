using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This script needs to be put on the actor, and takes care of the current step to accomplish.
// the step contains a dialogue and maybe an event.

public class StepController : MonoBehaviour
{
	[HideInInspector]
    public bool isInDialogue; //Consumed by the state machine


    [Header("Data")]
	[SerializeField] private ActorSO m_Actor = default;
	[SerializeField] private DialogueDataSO m_DefaultDialogue = default;
	[SerializeField] private QuestManagerSO m_QuestData = default;

	[Header("Listening to channels")]
	//[SerializeField] private DialogueActorChannelSO m_InteractionEvent = default;
	[SerializeField] private VoidEventChannelSO m_WinDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO m_LoseDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO m_EndDialogueEvent = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private DialogueDataChannelSO m_StartDialogueEvent = default;
	

	// Check if character is active. An active character is the character concerned by the step.
	private DialogueDataSO m_CurrentDialogue;


	// Start a dialogue when interaction
	// Some Steps need to be instantanious. And do not need the interact button.
	// When interaction again, restart same dialogue.
	public void InteractWithCharacter()
	{
		DialogueDataSO displayDialogue = m_QuestData.InteractWithCharacter(m_Actor, false, false);
		//Debug.Log("dialogue " + displayDialogue + "actor" + m_Actor);

		if (displayDialogue != null)
		{
			m_CurrentDialogue = displayDialogue;
			StartDialogue();
		}
		else
			PlayDefaultDialogue();
	}


    private void PlayDefaultDialogue()
    {
        if (m_DefaultDialogue != null)
        {
            m_CurrentDialogue = m_DefaultDialogue;
            StartDialogue();
        }
    }

	private void StartDialogue()
	{
		m_StartDialogueEvent.RaiseEvent(m_CurrentDialogue);
		m_EndDialogueEvent.onEventRaised += EndDialogue;

		StopConversation();

		m_WinDialogueEvent.onEventRaised += PlayWinDialogue;
		m_LoseDialogueEvent.onEventRaised += PlayLoseDialogue;
		isInDialogue = true;
	}

	private void EndDialogue()
	{
		m_EndDialogueEvent.onEventRaised -= EndDialogue;
		m_WinDialogueEvent.onEventRaised -= PlayWinDialogue;
		m_LoseDialogueEvent.onEventRaised -= PlayLoseDialogue;

		ResumeConversation();

		isInDialogue = false;
	}

	private void PlayLoseDialogue()
	{
		if (m_QuestData != null)
		{
			DialogueDataSO displayDialogue = m_QuestData.InteractWithCharacter(m_Actor, true, false);

			if (displayDialogue != null)
			{
				m_CurrentDialogue = displayDialogue;
				StartDialogue();
			}
		}
	}

	private void PlayWinDialogue()
	{
		if (m_QuestData != null)
		{
			DialogueDataSO displayDialogue = m_QuestData.InteractWithCharacter(m_Actor, true, true);

			if (displayDialogue != null)
			{
				m_CurrentDialogue = displayDialogue;
				StartDialogue();
			}
		}
	}

	private void StopConversation()
	{/*
		GameObject[] talkingTo = gameObject.GetComponent<NPC>().talkingTo;

		if (talkingTo != null)
		{
			foreach (GameObject npc in talkingTo)
			{
                npc.GetComponent<NPC>().npcState = NPCState.Idle;
			}
		}*/
	}

	private void ResumeConversation()
	{/*
		GameObject[] talkingTo = GetComponent<NPC>().talkingTo;

		if (talkingTo != null)
		{
            foreach (GameObject npc in talkingTo)
			{
				npc.GetComponent<NPC>().npcState = NPCState.Talk;
			}
		}*/
	}
}

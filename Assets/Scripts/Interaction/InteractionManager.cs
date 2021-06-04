using System.Collections.Generic;
using UnityEngine;

public enum InteractionType { None = 0, PickUp, Cook, Talk };

public class InteractionManager : MonoBehaviour
{
	[HideInInspector] public InteractionType currentInteractionType; //This is checked by conditions in the StateMachine
	[SerializeField] private InputReader m_InputReader = default;
	//To store the object we are currently interacting with
	private LinkedList<Interaction> m_PotentialInteractions = new LinkedList<Interaction>();

	//Events for the different interaction types
	[Header("Broadcasting on")]
	[SerializeField] private ItemEventChannelSO m_OnObjectPickUp = default;
	//[SerializeField] private VoidEventChannelSO m_OnCookingStart = default;
	//[SerializeField] private DialogueActorChannelSO m_StartTalking = default;
	//UI event
	[SerializeField] private InteractionUIEventChannelSO m_ToggleInteractionUI = default;

	[Header("Listening to")]
	[SerializeField] private VoidEventChannelSO m_OnInteractionEnded = default;

	private void OnEnable()
	{
		m_InputReader.interactEvent += OnInteractionButtonPress;
		m_OnInteractionEnded.OnEventRaised += OnInteractionEnd;
	}

	private void OnDisable()
	{
		m_InputReader.interactEvent -= OnInteractionButtonPress;
		m_OnInteractionEnded.OnEventRaised -= OnInteractionEnd;
	}

	// Called mid-way through the AnimationClip of collecting
	private void Collect()
	{
		GameObject itemObject = m_PotentialInteractions.First.Value.interactableObject;
		m_PotentialInteractions.RemoveFirst();

		if (m_OnObjectPickUp != null)
		{
			Item currentItem = itemObject.GetComponent<CollectableItem>().GetItem();
			m_OnObjectPickUp.RaiseEvent(currentItem);
		}

		Destroy(itemObject); //TODO: maybe move this destruction in a more general manger, to implement a removal SFX

		RequestUpdateUI(false);
	}

	private void OnInteractionButtonPress()
	{
		if (m_PotentialInteractions.Count == 0)
			return;

		currentInteractionType = m_PotentialInteractions.First.Value.type;

		switch (m_PotentialInteractions.First.Value.type)
		{
			case InteractionType.Cook:
				/*if (m_OnCookingStart != null)
				{
					m_OnCookingStart.RaiseEvent();
					m_InputReader.EnableMenuInput();
				}*/
				break;

			case InteractionType.Talk:
				/*if (m_StartTalking != null)
				{
					m_PotentialInteractions.First.Value.interactableObject.GetComponent<StepController>().InteractWithCharacter();
					m_InputReader.EnableDialogueInput();
				}*/
				break;

				//No need to do anything for Pickup type, the StateMachine will transition to the state
				//and then the AnimationClip will call Collect()
		}
	}

	//Called by the Event on the trigger collider on the child GO called "InteractionDetector"
	public void OnTriggerChangeDetected(bool entered, GameObject obj)
	{
		if (entered)
			AddPotentialInteraction(obj);
		else
			RemovePotentialInteraction(obj);
	}

	private void AddPotentialInteraction(GameObject obj)
	{
		Interaction newPotentialInteraction = new Interaction(InteractionType.None, obj);

		if (obj.CompareTag("Pickable"))
		{
			newPotentialInteraction.type = InteractionType.PickUp;
		}
		else if (obj.CompareTag("CookingPot"))
		{
			newPotentialInteraction.type = InteractionType.Cook;
		}
		else if (obj.CompareTag("NPC"))
		{
			newPotentialInteraction.type = InteractionType.Talk;
		}

		if (newPotentialInteraction.type != InteractionType.None)
		{
			m_PotentialInteractions.AddFirst(newPotentialInteraction);
			RequestUpdateUI(true);
		}
	}

	private void RemovePotentialInteraction(GameObject obj)
	{
		LinkedListNode<Interaction> currentNode = m_PotentialInteractions.First;
		while (currentNode != null)
		{
			if (currentNode.Value.interactableObject == obj)
			{
				m_PotentialInteractions.Remove(currentNode);
				break;
			}
			currentNode = currentNode.Next;
		}

		RequestUpdateUI(m_PotentialInteractions.Count > 0);
	}

	private void RequestUpdateUI(bool visible)
	{
		if (visible)
			m_ToggleInteractionUI.RaiseEvent(true, m_PotentialInteractions.First.Value.type);
		else
			m_ToggleInteractionUI.RaiseEvent(false, InteractionType.None);
	}

	private void OnInteractionEnd()
	{
		switch (currentInteractionType)
		{
			case InteractionType.Cook:
			case InteractionType.Talk:
				//We show the UI after cooking or talking, in case player wants to interact again
				RequestUpdateUI(true);
				break;
		}

		m_InputReader.EnableGameplayInput();
	}
}

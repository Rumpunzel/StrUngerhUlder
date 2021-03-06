using System;
using UnityEngine;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.Inventories;
using Strungerhulder.Inventories.ScriptableObjects;
using Strungerhulder.Characters;
using Strungerhulder.Input;

namespace Strungerhulder.Interactions
{
    public enum InteractionType
    {
        None = 0,
        PickUp,
        Attack,
    };

    [RequireComponent(typeof(Protagonist))]
    public class InteractionManager : MonoBehaviour
    {
        [HideInInspector] public Interaction currentInteraction; //This is checked by conditions in the StateMachine

        [SerializeField] private InputReader m_InputReader = default;


        [Header("Interactable Tags")]
        public string PickableTag = "Pickable";


        [Header("Area Attributes")]
        [SerializeField] private LayerMask m_WhatToCheck;
        [Range(0f, 16f)] [SerializeField] private float m_PerceptionRange = 5f;
        [Range(.1f, 10f)] [SerializeField] private float m_InteractionRange = 1f;


        //Events for the different interaction types
        [Header("Broadcasting on")]
        [SerializeField] private ItemEventChannelSO m_OnObjectPickUp = default;
        [SerializeField] private ItemEventChannelSO m_OnObjectAttack = default;
        //[SerializeField] private VoidEventChannelSO m_OnCookingStart = default;
        //[SerializeField] private DialogueActorChannelSO m_StartTalking = default;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO m_OnInteractionEnded = default;


        private Protagonist m_Protagonist;
        private Interaction m_NearestInteraction;
        private bool m_InteractButtonPressed = false;


        private void OnEnable()
        {
            m_InputReader.InteractEvent += OnInteractEvent;
            m_InputReader.InteractCanceledEvent += OnInteractCanceledEvent;

            m_OnInteractionEnded.OnEventRaised += OnInteractionEnd;
        }

        private void OnDisable()
        {
            m_InputReader.InteractEvent -= OnInteractEvent;
            m_InputReader.InteractCanceledEvent -= OnInteractCanceledEvent;

            m_OnInteractionEnded.OnEventRaised -= OnInteractionEnd;
        }

        private void Awake()
        {
            m_Protagonist = GetComponent<Protagonist>();
        }

        private void Update()
        {
            m_NearestInteraction = null;
            if (m_InteractButtonPressed) InteractWithNearest();
        }


        // Called mid-way through the AnimationClip of collecting
        private void LookAtItem()
        {
            GameObject itemObject = currentInteraction.interactableObject;
            this.transform.LookAt(itemObject.transform);
        }

        // Called mid-way through the AnimationClip of collecting
        private void Collect()
        {
            GameObject itemObject = currentInteraction.interactableObject;

            if (m_OnObjectPickUp != null)
            {
                Item currentItem = itemObject.GetComponent<CollectableItem>().GetItem();
                m_OnObjectPickUp.RaiseEvent(currentItem);
            }

            currentInteraction = null;
            Destroy(itemObject); //TODO: maybe move this destruction in a more general manger, to implement a removal SFX

            RequestUpdateUI(false);
        }

        private void Attack()
        {
            GameObject itemObject = currentInteraction.interactableObject;

            if (m_OnObjectAttack != null)
            {
                Item currentItem = itemObject.GetComponent<CollectableItem>().GetItem();
                m_OnObjectAttack.RaiseEvent(currentItem);
            }

            currentInteraction = null;
            Destroy(itemObject); //TODO: maybe move this destruction in a more general manger, to implement a removal SFX

            RequestUpdateUI(false);
        }

        private void InteractWithNearest()
        {
            m_NearestInteraction = NearestInteraction(m_PerceptionRange);

            if (m_NearestInteraction == null)
            {
                //currentInteraction = null;
                return;
            }

            Vector3 distance = m_NearestInteraction.interactableObject.transform.position - transform.position;
            // Not in range
            if (distance.magnitude > m_InteractionRange)
            {
                //if (m_Protagonist.movementInput != Vector3.zero)
                //	return;

                m_Protagonist.destinationInput = m_NearestInteraction.interactableObject.transform.position - (distance.normalized * m_InteractionRange * .5f);
                m_Protagonist.movingToDestination = true;
                return;
            }

            currentInteraction = m_NearestInteraction;

            /*switch (currentInteraction.type)
            {
                case InteractionType.Cook:
                    if (m_OnCookingStart != null)
                    {
                        m_OnCookingStart.RaiseEvent();
                        m_InputReader.EnableMenuInput();
                    }
                    break;

                case InteractionType.Talk:
                    if (m_StartTalking != null)
                    {
                        m_PotentialInteractions.First.Value.interactableObject.GetComponent<StepController>().InteractWithCharacter();
                        m_InputReader.EnableDialogueInput();
                    }
                    break;

                //No need to do anything for Pickup type, the StateMachine will transition to the state
                //and then the AnimationClip will call Collect()
            }*/
        }

        private Interaction NearestInteraction(float rangeToCheck)
        {
            Interaction nearest = null;
            float nearDist = float.PositiveInfinity;
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, rangeToCheck, m_WhatToCheck);

            foreach (Collider collider in hitColliders)
            {
                Interaction newPotentialInteraction = new Interaction(InteractionType.None, collider.gameObject);

                if (newPotentialInteraction.interactableObject.CompareTag(PickableTag))
                {
                    newPotentialInteraction.type = InteractionType.PickUp;
                }
                else if (newPotentialInteraction.interactableObject.CompareTag("Tree"))
                {
                    newPotentialInteraction.type = InteractionType.Attack;
                }
                /*else if (obj.CompareTag("CookingPot"))
                {
                    newPotentialInteraction.type = InteractionType.Cook;
                }
                else if (obj.CompareTag("NPC"))
                {
                    newPotentialInteraction.type = InteractionType.Talk;
                }*/

                if (newPotentialInteraction.type != InteractionType.None)
                    RequestUpdateUI(true);
                else
                    continue;


                Vector3 offset = this.transform.position - newPotentialInteraction.interactableObject.transform.position;
                float thisDist = offset.sqrMagnitude;

                if (thisDist < nearDist)
                {
                    nearDist = thisDist;
                    nearest = newPotentialInteraction;
                }
            }

            return nearest;
        }

        private void RequestUpdateUI(bool visible)
        {
            /*if (visible)
                m_ToggleInteractionUI.RaiseEvent(true, m_PotentialInteractions.First.Value.type);
            else
                m_ToggleInteractionUI.RaiseEvent(false, InteractionType.None);*/
        }

        private void OnInteractEvent() => m_InteractButtonPressed = true;

        private void OnInteractCanceledEvent()
        {
            m_InteractButtonPressed = false;

            if (!(currentInteraction == null || currentInteraction.type == InteractionType.None))
                m_Protagonist.destinationInput = m_Protagonist.transform.position;
        }

        private void OnInteractionEnd()
        {
            /*switch (currentInteraction.type)
            {
                case InteractionType.Cook:
                case InteractionType.Talk:
                    //We show the UI after cooking or talking, in case player wants to interact again
                    RequestUpdateUI(true);
                    break;
            }*/

            m_InputReader.EnableGameplayInput();
        }
    }
}

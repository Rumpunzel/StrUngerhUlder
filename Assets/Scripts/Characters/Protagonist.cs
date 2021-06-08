﻿using System;
using UnityEngine;
using Strungerhulder.Input;
using Strungerhulder.RuntimeAnchors;

namespace Strungerhulder.Characters
{
    /// <summary>
    /// <para>This component consumes input on the InputReader and stores its values. The input is then read, and manipulated, by the StateMachines's Actions.</para>
    /// </summary>
    public class Protagonist : MonoBehaviour
    {
        [SerializeField] private TransformAnchor m_GameplayCameraTransform;
        [SerializeField] private InputReader m_InputReader = default;

        [Space]
        [SerializeField] private LayerMask m_WorldLayer;


        // These fields are read and manipulated by the StateMachine actions
        #region StateMachine Fields
        [Space]
        public Strungerhulder.Charaters.StateMachines.ScriptableObjects.CharacterMovementStatsSO movementStats = default;

        [NonSerialized] public bool movingToDestination = false; // Set after a destination input

        [NonSerialized] public Vector3 movementInput; // Initial input coming from the Protagonist script
        [NonSerialized] public Vector3 movementVector; // Final movement vector, manipulated by the StateMachine actions

        [NonSerialized] public Vector3 destinationInput; // Initial click input coming from the Protagonist script
        [NonSerialized] public Vector3 destinationPoint; // Final destination point, manipulated by the StateMachine actions

        [NonSerialized] public bool isRunning; // Used when using the keyboard to run, brings the normalised speed to 1

        [NonSerialized] public bool jumpInput;

        [NonSerialized] public bool extraActionInput;
        [NonSerialized] public bool attackInput;

        [NonSerialized] public ControllerColliderHit lastHit;
        #endregion


        private bool m_GettingPointFromMouse;

        private Vector3 m_MovementInput;
        private Vector3 m_MovementVector;

        private Vector3 m_DestinationInput;
        private Vector3 m_DestinationPoint;

        private Vector2 m_InputVector;
        private float m_PreviousSpeed;



        private void OnControllerColliderHit(ControllerColliderHit hit) => lastHit = hit;

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable()
        {
            m_InputReader.MoveEvent += OnMove;
            m_InputReader.MoveToPointEvent += OnMoveToPoint;
            m_InputReader.MoveToPointCanceledEvent += OnMoveToPointCanceled;

            m_InputReader.JumpEvent += OnJumpInitiated;
            m_InputReader.JumpCanceledEvent += OnJumpCanceled;

            m_InputReader.StartedRunning += OnStartedRunning;
            m_InputReader.StoppedRunning += OnStoppedRunning;
            //m_InputReader.AttackEvent += OnStartedAttack;
            //...
        }

        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable()
        {
            m_InputReader.MoveEvent -= OnMove;
            m_InputReader.MoveToPointEvent -= OnMoveToPoint;
            m_InputReader.MoveToPointCanceledEvent -= OnMoveToPointCanceled;

            m_InputReader.JumpEvent -= OnJumpInitiated;
            m_InputReader.JumpCanceledEvent -= OnJumpCanceled;

            m_InputReader.StartedRunning -= OnStartedRunning;
            m_InputReader.StoppedRunning -= OnStoppedRunning;
            //m_InputReader.AttackEvent -= OnStartedAttack;
            //...
        }

        private void Update()
        {
            if (!movingToDestination)
                destinationInput = transform.position;

            if (m_GettingPointFromMouse)
            {
                movementInput = Vector3.zero;
                GetPointFromMouse();
            }
            else
                RecalculateMovement();
        }


        public Vector3 GetAdjustedMovement()
        {
            Vector3 adjustedMovement;

            if (m_GameplayCameraTransform)
            {
                //Get the two axes from the camera and flatten them on the XZ plane
                Vector3 cameraForward = m_GameplayCameraTransform.Transform.forward;
                cameraForward.y = 0f;
                Vector3 cameraRight = m_GameplayCameraTransform.Transform.right;
                cameraRight.y = 0f;

                //Use the two axes, modulated by the corresponding inputs, and construct the final vector
                adjustedMovement = cameraRight.normalized * m_InputVector.x + cameraForward.normalized * m_InputVector.y;
            }
            else
            {
                //No CameraManager exists in the scene, so the input is just used absolute in world-space
                Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
                adjustedMovement = new Vector3(m_InputVector.x, 0f, m_InputVector.y);
            }

            return adjustedMovement;
        }


        private void RecalculateMovement()
        {
            float targetSpeed = Mathf.Clamp01(m_InputVector.magnitude);

            //Accelerate/decelerate
            targetSpeed = Mathf.Lerp(m_PreviousSpeed, targetSpeed, Time.deltaTime * movementStats.moveAcceleration);
            movementInput = GetAdjustedMovement().normalized * targetSpeed;

            m_PreviousSpeed = targetSpeed;
        }


        private void GetPointFromMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(m_InputReader.MousePosition());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, m_WorldLayer))
                destinationInput = hit.point;
        }


        //---- EVENT LISTENERS ----

        private void OnMove(Vector2 movement) => m_InputVector = movement;

        private void OnMoveToPoint() => m_GettingPointFromMouse = true;
        private void OnMoveToPointCanceled() => m_GettingPointFromMouse = false;

        private void OnJumpInitiated() => jumpInput = true;
        private void OnJumpCanceled() => jumpInput = false;

        private void OnStoppedRunning() => isRunning = false;
        private void OnStartedRunning() => isRunning = true;


        private void OnStartedAttack() => attackInput = true;
        // Triggered from Animation Event
        public void ConsumeAttackInput() => attackInput = false;
    }
}

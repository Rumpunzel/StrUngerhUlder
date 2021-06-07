using System;
using UnityEngine;
using Strungerhulder.Input;

namespace Strungerhulder.Characters
{
    /// <summary>
    /// <para>This component consumes input on the InputReader and stores its values. The input is then read, and manipulated, by the StateMachines's Actions.</para>
    /// </summary>
    public class Protagonist : MonoBehaviour
    {
        #region Constants
        public const float GRAVITY_MULTIPLIER = 2.2f;
        public const float MAX_FALL_SPEED = -50f;
        public const float MAX_RISE_SPEED = 100f;
        //public const float GRAVITY_COMEBACK_MULTIPLIER = .03f;
        //public const float GRAVITY_DIVIDER = .9f;
        public const float AIR_RESISTANCE = 5f;
        public const float TURN_RATE = 500f;
        #endregion

        public TransformAnchor gameplayCameraTransform;

        [SerializeField] private InputReader m_InputReader = default;

        [Space]
        [SerializeField] private LayerMask m_WorldLayer;


        //These fields are read and manipulated by the StateMachine actions
        #region StateMachine Fields
        [NonSerialized] public bool movingToDestination = false; //Set after a destination input

        [NonSerialized] public Vector3 movementInput; //Initial input coming from the Protagonist script
        [NonSerialized] public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions

        [NonSerialized] public Vector3 destinationInput; //Initial click input coming from the Protagonist script
        [NonSerialized] public Vector3 destinationPoint; //Final destination point, manipulated by the StateMachine actions

        [NonSerialized] public bool isRunning; // Used when using the keyboard to run, brings the normalised speed to 1

        [NonSerialized] public bool jumpInput;

        [NonSerialized] public bool extraActionInput;
        [NonSerialized] public bool attackInput;

        [NonSerialized] public ControllerColliderHit lastHit;
        #endregion


        private bool m_GettingPointFromMouse;

        private Vector2 m_InputVector;
        private float m_PreviousSpeed;


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            lastHit = hit;
        }

        //Adds listeners for events being triggered in the InputReader script
        private void OnEnable()
        {
            m_InputReader.moveEvent += OnMove;
            m_InputReader.moveToPointEvent += OnMoveToPoint;
            m_InputReader.moveToPointCanceledEvent += OnMoveToPointCanceled;

            m_InputReader.jumpEvent += OnJumpInitiated;
            m_InputReader.jumpCanceledEvent += OnJumpCanceled;

            m_InputReader.startedRunning += OnStartedRunning;
            m_InputReader.stoppedRunning += OnStoppedRunning;
            //m_InputReader.attackEvent += OnStartedAttack;
            //...
        }

        //Removes all listeners to the events coming from the InputReader script
        private void OnDisable()
        {
            m_InputReader.moveEvent -= OnMove;
            m_InputReader.moveToPointEvent -= OnMoveToPoint;
            m_InputReader.moveToPointCanceledEvent -= OnMoveToPointCanceled;

            m_InputReader.jumpEvent -= OnJumpInitiated;
            m_InputReader.jumpCanceledEvent -= OnJumpCanceled;

            m_InputReader.startedRunning -= OnStartedRunning;
            m_InputReader.stoppedRunning -= OnStoppedRunning;
            //m_InputReader.attackEvent -= OnStartedAttack;
            //...
        }

        private void Update()
        {
            if (!movingToDestination)
                destinationInput = transform.position;

            if (m_GettingPointFromMouse)
                GetPointFromMouse();
            else
                RecalculateMovement();
        }


        private void RecalculateMovement()
        {
            float targetSpeed = 0f;
            Vector3 adjustedMovement;

            if (gameplayCameraTransform)
            {
                //Get the two axes from the camera and flatten them on the XZ plane
                Vector3 cameraForward = gameplayCameraTransform.Transform.forward;
                cameraForward.y = 0f;
                Vector3 cameraRight = gameplayCameraTransform.Transform.right;
                cameraRight.y = 0f;

                //Use the two axes, modulated by the corresponding inputs, and construct the final vector
                adjustedMovement = cameraRight.normalized * m_InputVector.x +
                    cameraForward.normalized * m_InputVector.y;
            }
            else
            {
                //No CameraManager exists in the scene, so the input is just used absolute in world-space
                Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
                adjustedMovement = new Vector3(m_InputVector.x, 0f, m_InputVector.y);
            }

            //Fix to avoid getting a Vector3.zero vector, which would result in the player turning to x:0, z:0
            if (m_InputVector.sqrMagnitude == 0f)
                adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

            //Accelerate/decelerate
            targetSpeed = Mathf.Clamp01(m_InputVector.magnitude);

            if (targetSpeed > 0f)
            {
                // This is used to set the speed to the maximum if holding the Shift key,
                // to allow keyboard players to "run"
                if (isRunning)
                    targetSpeed = 1f;

                if (attackInput)
                    targetSpeed = .05f;
            }

            targetSpeed = Mathf.Lerp(m_PreviousSpeed, targetSpeed, Time.deltaTime * 4f);

            movementInput = adjustedMovement.normalized * targetSpeed;

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

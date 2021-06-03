using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActor;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(PerceptionArea))]
[RequireComponent(typeof(Animator))]
public class ActorStateMachine : MonoBehaviour
{
    public enum k_STATES {
        Idle,
        Walking,
        Running,
        Sprinting,
        Jumping,
        Mining,
        Working,
        Gathering,
        Hurt,
        Dead,
    }

    [Header("State Attributes")]
    [Range(0f, 2f)] [SerializeField] private float m_SprintPercentage = 1.1f;
    [Range(0f, 1f)] [SerializeField] private float m_RunPercentage = .5f;

    private CharacterMovement m_CharacterMovement;
    private PerceptionArea m_PerceptionArea;
    private Animator m_Animator;
    private k_STATES m_CurrentState;

    # region Inputs
    private Vector3 m_DestinationInput;
    private bool m_SeekingDestination = false;
    private Vector3 m_DirectionInput = Vector3.zero;

    private bool m_SprintInput = false;
    private bool m_JumpInput = false;

    private bool m_LookForObjectInput = false;
    # endregion

    private Vector3 m_HorizontalVelocity = Vector3.zero;
    private Vector3 m_VerticalVelocity = Vector3.zero;
    private bool m_IsGrounded;
    private bool m_CanReceiveInput = true;


    private void Awake()
	{
        m_CharacterMovement = GetComponent<CharacterMovement>();
        m_PerceptionArea = GetComponent<PerceptionArea>();
		m_Animator = GetComponent<Animator>();
	}

    // Start is called before the first frame update
    private void Start()
    {
        EnterState(k_STATES.Idle);
    }

    private void Update()
    {
        m_CanReceiveInput = true;

        if (m_LookForObjectInput) HandleInteraction();
        HandleMovement();

        ParseStates();
    }



    # region Input Properties
    public Vector3 DestinationInput {
        get { return m_DestinationInput; }
        set {
            if (!m_CanReceiveInput) return;
            m_DestinationInput = value;
            m_SeekingDestination = true;
        }
    }
    public Vector3 DirectionInput {
        get { return m_DirectionInput; }
        set {
            if (!m_CanReceiveInput) return;
            m_DirectionInput = value;
            m_SeekingDestination = false;
        }
    }
    public bool SprintInput {
        get { return m_SprintInput; }
        set {
            if (!m_CanReceiveInput) return;
            m_SprintInput = value;
        }
    }
    public bool JumpInput {
        get { return m_JumpInput; }
        set {
            if (!m_CanReceiveInput) return;
            m_JumpInput = value;
        }
    }
    public bool LookForObjectInput {
        get { return m_LookForObjectInput; }
        set {
            m_LookForObjectInput = value;
        }
    }
    # endregion


    # region Movement
    private void HandleMovement()
    {
        m_IsGrounded = m_CharacterMovement.IsGrounded;
        JumpInput = JumpInput && m_IsGrounded;

        m_HorizontalVelocity = CanMove() ? Move() : m_CharacterMovement.MoveTowardDestination(Vector3.zero);;

        if (JumpInput && m_IsGrounded && m_CurrentState < k_STATES.Jumping)
        {
            JumpInput = false;
            m_IsGrounded = false;
            m_VerticalVelocity = m_CharacterMovement.Jump();
        }
    }

    private bool CanMove() {
        return m_CurrentState < k_STATES.Dead;
    }

    private Vector3 Move()
    {
        if (!m_SeekingDestination)
        {
            return m_CharacterMovement.MoveInDirection(m_DirectionInput, SprintInput);
        }
        else if (m_IsGrounded)
        {
            return m_CharacterMovement.MoveTowardDestination(m_DestinationInput, SprintInput);
        }

        return Vector3.zero;
    }
    # endregion


    # region Interaction
    private void HandleInteraction()
    {
        PerceptionArea.SpottedTransform spottedTransform = m_PerceptionArea.CheckAreaForObjects();

        if (spottedTransform == null) return;

        if (!m_SeekingDestination) DirectionInput = Vector3.zero;

        if (spottedTransform.transform)
        {
            DestinationInput = this.transform.position;
        }
        else
        {
            DestinationInput = spottedTransform.position;
        }

        m_CanReceiveInput = false;
    }
    # endregion


    # region State Machine
    private void ParseStates()
    {
        k_STATES newState;

        if (!m_IsGrounded)
        {
            newState = k_STATES.Jumping;
        }
        else if (m_HorizontalVelocity.magnitude > m_SprintPercentage)
        {
            newState = k_STATES.Sprinting;
        }
        else if (m_HorizontalVelocity.magnitude > m_RunPercentage)
        {
            newState = k_STATES.Running;
        }
        else if (m_HorizontalVelocity.magnitude > .01f)
        {
            newState = k_STATES.Walking;
        }
        else
        {
            newState = k_STATES.Idle;
        }
        
        if (newState != m_CurrentState) EnterState(newState);
    }

    private void EnterState(k_STATES newState)
    {
        if (newState == m_CurrentState) return;

        m_CurrentState = newState;
        UpdateAnimator(newState);
    }

    private void UpdateAnimator(k_STATES state)
    {
        switch (state)
        {
            default:
                m_Animator.SetTrigger(state.ToString());
                break;
        }
    }
    #endregion
}

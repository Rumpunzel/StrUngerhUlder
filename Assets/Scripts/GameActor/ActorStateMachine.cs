using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Public Members")]
    public bool SprintInput = false;
    public bool JumpInput = false;
    public bool LookForObjectInput = false;

    private CharacterMovement m_CharacterMovement;
    private PerceptionArea m_PerceptionArea;
    private Animator m_Animator;
    private k_STATES m_CurrentState;

    private Vector3 m_DestinationInput;
    private bool m_SeekingDestination = false;
    private Vector3 m_DirectionInput = Vector3.zero;

    private Vector3 m_HorizontalVelocity = Vector3.zero;
    private Vector3 m_VerticalVelocity = Vector3.zero;
    private bool m_IsGrounded;


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

    private void FixedUpdate()
    {
        m_IsGrounded = m_CharacterMovement.IsGrounded;
        JumpInput = JumpInput && m_IsGrounded;

        m_HorizontalVelocity = CanMove() ? Move() : m_CharacterMovement.MoveTowardDestination(Vector3.zero);;

        if (JumpInput && m_IsGrounded && m_CurrentState >= k_STATES.Jumping)
        {
            JumpInput = false;
            m_VerticalVelocity = m_CharacterMovement.Jump();

            EnterState(k_STATES.Jumping);
        }

        ParseStates();
    }


    public Vector3 DestinationInput {
        get { return m_DestinationInput; }
        set {
            m_DestinationInput = value;
            m_SeekingDestination = true;
        }
    }
    public Vector3 DirectionInput {
        get { return m_DirectionInput; }
        set {
            m_DirectionInput = value;
            m_SeekingDestination = false;
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

    private void ParseStates()
    {
        k_STATES newState;

        if (m_HorizontalVelocity.magnitude > m_SprintPercentage)
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
        
        if (m_IsGrounded && newState != m_CurrentState) EnterState(newState);
    }

    public float Jump(float jumpForce)
    {
        // Cannot Jump
        if (m_CurrentState >= k_STATES.Jumping) return 0f;
        
        EnterState(k_STATES.Jumping);
        return jumpForce;
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
}

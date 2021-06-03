using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ActorStateMachine))]
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    public bool SprintInput = false;
    public bool JumpInput = false;

    [SerializeField] private float m_MovementSpeed = 10f;
    [SerializeField] private float m_SprintModifier = 1.5f;
    [SerializeField] private float m_JumpHeight = 1.0f;

    private CharacterController m_CharacterController;
    private ActorStateMachine m_StateMachine;
    private NavMeshAgent m_NavMeshAgent;

    private Vector3 m_DestinationInput = Vector3.zero;
    private bool m_SeekingDestination = false;
    private Vector3 m_DirectionInput = Vector3.zero;

    private Vector3 m_HorizontalVelocity = Vector3.zero;
    private Vector3 m_VerticalVelocity = Vector3.zero;
    private bool m_IsGrounded;


    private void Awake()
	{
        m_CharacterController = GetComponent<CharacterController>();
        m_StateMachine = GetComponent<ActorStateMachine>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
	}

    private void FixedUpdate()
    {
        CheckGrounded();
        ProcessActions();
    }


    public Vector3 DestinationInut {
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

            if (m_NavMeshAgent.enabled)
            {
                m_NavMeshAgent.velocity = Vector3.zero;
            }
        }
    }


    /// <summary>
    /// Checks whether the character is on the ground and updates <see cref="IsGrounded"/>
    /// </summary>
    private void CheckGrounded()
    {
        m_IsGrounded = m_CharacterController.isGrounded;
        JumpInput = JumpInput && m_IsGrounded;
        if (m_IsGrounded && m_VerticalVelocity.y < 0f) m_VerticalVelocity.y = 0f;
        
        m_StateMachine.IsGrounded = m_IsGrounded;
    }

    /// <summary>
    /// Processes input actions and converts them into movement
    /// </summary>
    private void ProcessActions()
    {
        if (m_StateMachine.CanMove())
        {
            // Movement
            if (!m_SeekingDestination)
            {
                Move();
            }
            else if (m_IsGrounded)
            {
                MoveTowardDestination();
            }

            // Jump
            if (JumpInput && m_IsGrounded)
            {
                JumpInput = false;
                ToggleManualMovement(true);
                m_VerticalVelocity.y += m_StateMachine.Jump(Mathf.Sqrt(m_JumpHeight * -3.0f * Physics.gravity.y));
            }
        }

        if (m_CharacterController.enabled)
        {
            m_VerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
            m_CharacterController.Move(m_VerticalVelocity * Time.deltaTime);
        }
    }

    private void MoveTowardDestination()
    {
        ToggleManualMovement(false);
        m_NavMeshAgent.speed = m_MovementSpeed * (SprintInput ? m_SprintModifier : 1f);
        m_NavMeshAgent.destination = m_DestinationInput;

        m_StateMachine.Velocity = m_NavMeshAgent.velocity / m_MovementSpeed;
    }

    private void Move()
	{
        ToggleManualMovement(true);
        m_HorizontalVelocity = Vector3.Lerp(m_HorizontalVelocity, m_DirectionInput * m_MovementSpeed * (SprintInput ? m_SprintModifier : 1f), m_NavMeshAgent.acceleration * Time.deltaTime);

        if (m_HorizontalVelocity != Vector3.zero) this.transform.forward = Vector3.Lerp(this.transform.forward, m_HorizontalVelocity, (m_NavMeshAgent.angularSpeed / 60f) * Time.fixedDeltaTime);
        m_CharacterController.Move(m_HorizontalVelocity * Time.deltaTime);

        m_StateMachine.Velocity = m_HorizontalVelocity / m_MovementSpeed;
    }

    private void ToggleManualMovement(bool manual)
    {
        m_CharacterController.enabled = manual;
        m_NavMeshAgent.enabled = !manual;
    }
}

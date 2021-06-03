using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Attributes")]
    [SerializeField] private float m_MovementSpeed = 10f;
    [SerializeField] private float m_SprintModifier = 1.5f;
    [SerializeField] private float m_JumpHeight = 1.0f;

    [Header("Public Members")]
    public bool IsGrounded;

    private CharacterController m_CharacterController;
    private NavMeshAgent m_NavMeshAgent;

    private Vector3 m_HorizontalVelocity;
    private Vector3 m_VerticalVelocity;


    private void Awake()
	{
        m_CharacterController = GetComponent<CharacterController>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
	}

    private void FixedUpdate()
    {
        if (m_CharacterController.enabled)
        {
            m_VerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
            m_CharacterController.Move(m_VerticalVelocity * Time.deltaTime);
        }

        CheckGrounded();
    }


    public bool CheckGrounded()
    {
        IsGrounded = m_CharacterController.isGrounded;
        if (IsGrounded && m_VerticalVelocity.y < 0f) m_VerticalVelocity.y = 0f;
        
        return IsGrounded;
    }

    public Vector3 Jump()
    {
        ToggleManualMovement(true);
        m_VerticalVelocity.y += Mathf.Sqrt(m_JumpHeight * -3.0f * Physics.gravity.y);

        return m_VerticalVelocity;
    }

    public Vector3 MoveTowardDestination(Vector3 destination, bool sprinting = false)
    {
        ToggleManualMovement(false);
        m_NavMeshAgent.speed = m_MovementSpeed * (sprinting ? m_SprintModifier : 1f);
        m_NavMeshAgent.destination = destination;

        return m_NavMeshAgent.velocity / m_MovementSpeed;
    }

    public Vector3 MoveInDirection(Vector3 direction, bool sprinting = false)
	{
        ToggleManualMovement(true);
        m_HorizontalVelocity = Vector3.Lerp(m_HorizontalVelocity, direction * m_MovementSpeed * (sprinting ? m_SprintModifier : 1f), m_NavMeshAgent.acceleration * Time.deltaTime);

        if (m_HorizontalVelocity != Vector3.zero) this.transform.forward = Vector3.Lerp(this.transform.forward, m_HorizontalVelocity, (m_NavMeshAgent.angularSpeed / 60f) * Time.fixedDeltaTime);
        m_CharacterController.Move(m_HorizontalVelocity * Time.deltaTime);

        return m_HorizontalVelocity / m_MovementSpeed;
    }


    private void ToggleManualMovement(bool manual)
    {
        if (m_NavMeshAgent.enabled && manual)
        {
            m_NavMeshAgent.velocity = Vector3.zero;
        }

        m_CharacterController.enabled = manual;
        m_NavMeshAgent.enabled = !manual;
    }
}

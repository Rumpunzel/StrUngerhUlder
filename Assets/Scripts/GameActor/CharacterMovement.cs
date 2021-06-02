using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ActorStateMachine))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed = 10f;
    [SerializeField] private float m_SprintModifier = 1.5f;
    [SerializeField] private float m_JumpHeight = 1.0f;

    public Vector3 Destination {
        get { return m_Destination; }
        set {
            m_Destination = value;
            m_SeekingDestination = true;
        }
    }
    public Vector3 Direction {
        get { return m_Direction; }
        set {
            m_Direction = value;
            m_SeekingDestination = false;
        }
    }
    public bool Sprinting {
        get { return m_Sprinting; }
        set { m_Sprinting = value; }
    }

    private Vector3 m_Destination = Vector3.zero;
    private bool m_SeekingDestination = false;
    private Vector3 m_Direction = Vector3.zero;
    private bool m_Sprinting = false;

    private CharacterController m_CharacterController;
    private ActorStateMachine m_StateMachine;

    private Vector3 m_HorizontalVelocity = Vector3.zero;
    private Vector3 m_VerticalVelocity = Vector3.zero;
    private bool m_IsGrounded;


    private void Awake()
	{
        m_CharacterController = GetComponent<CharacterController>();
        m_StateMachine = GetComponent<ActorStateMachine>();
	}

    void FixedUpdate()
    {
        // Ground Check
        m_IsGrounded = m_CharacterController.isGrounded;
        if (m_IsGrounded && m_VerticalVelocity.y < 0f) m_VerticalVelocity.y = 0f;

        if (m_SeekingDestination)
        {
            MoveTowardDestination();
        }
        else
        {
            Move();
        }

        m_VerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
        m_CharacterController.Move(m_VerticalVelocity * Time.deltaTime);
    }


    public void Jump()
    {
        if (m_IsGrounded)
        {
            m_VerticalVelocity.y += m_StateMachine.Jump(Mathf.Sqrt(m_JumpHeight * -3.0f * Physics.gravity.y));
            m_IsGrounded = false;
        }
    }


    private void MoveTowardDestination()
    {

    }

    private void Move()
	{
        m_HorizontalVelocity = m_StateMachine.Move(m_Direction * (m_Sprinting ? m_SprintModifier : 1f), m_IsGrounded) * m_MovementSpeed;
        if (m_HorizontalVelocity != Vector3.zero) gameObject.transform.forward = m_HorizontalVelocity;
        m_CharacterController.Move(m_HorizontalVelocity * Time.deltaTime);
    }
}

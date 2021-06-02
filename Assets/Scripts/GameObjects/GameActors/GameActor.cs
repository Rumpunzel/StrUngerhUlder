using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ActorStateMachine))]
public class GameActor : GameObject
{
    [SerializeField] private float m_MovementSpeed = 10f;
    [SerializeField] private float m_SprintModifier = 1.5f;
    [SerializeField] private float m_JumpHeight = 1.0f;

    public Vector3 m_Direction = Vector3.zero;
    public bool m_Sprinting = false;
    public bool m_Jumping = false;

    private CharacterController m_CharacterController;
    private ActorStateMachine m_StateMachine;

    private Vector3 m_HorizontalVelocity = Vector3.zero;
    private Vector3 m_VerticalVelocity = Vector3.zero;
    private bool m_IsGrounded;


    override protected void Awake()
	{
        base.Awake();

        m_CharacterController = GetComponent<CharacterController>();
        m_StateMachine = GetComponent<ActorStateMachine>();
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }


    private void Move()
	{
        // Ground Check
        m_IsGrounded = m_CharacterController.isGrounded;
        m_Jumping = m_Jumping && m_IsGrounded;
        if (m_IsGrounded && m_VerticalVelocity.y < 0f) m_VerticalVelocity.y = 0f;

        // Horizontal Movement
        m_HorizontalVelocity = m_StateMachine.Move(m_Direction * (m_Sprinting ? m_SprintModifier : 1f), m_IsGrounded) * m_MovementSpeed;
        if (m_HorizontalVelocity != Vector3.zero) gameObject.transform.forward = m_HorizontalVelocity;
        m_CharacterController.Move(m_HorizontalVelocity * Time.deltaTime);

        // Vertical Movement
        if (m_Jumping && m_IsGrounded)
        {
            m_VerticalVelocity.y += m_StateMachine.Jump(Mathf.Sqrt(m_JumpHeight * -3.0f * Physics.gravity.y));
            m_Jumping = false;
        }
        m_VerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
        m_CharacterController.Move(m_VerticalVelocity * Time.deltaTime);
    }
}

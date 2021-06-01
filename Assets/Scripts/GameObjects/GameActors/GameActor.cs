using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(ActorStateMachine))]
[RequireComponent(typeof(PlayerInput))]
public class GameActor : GameObject
{
    [SerializeField] private float m_MovementSpeed = 10f;

    private CharacterController m_CharacterController;
    private ActorStateMachine m_StateMachine;
    private PlayerInput m_Input;

    private Vector3 m_Velocity = Vector3.zero;
    private bool m_IsGrounded;


    override protected void Awake()
	{
        base.Awake();

        m_CharacterController = GetComponent<CharacterController>();
        m_StateMachine = GetComponent<ActorStateMachine>();
        m_Input = GetComponent<PlayerInput>();
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


    public void Move()
	{
        m_IsGrounded = m_CharacterController.isGrounded;
        if (m_IsGrounded && m_Velocity.y < 0f) m_Velocity.y = 0f;

        m_Velocity = m_StateMachine.Move(m_Input.m_Movement) * m_MovementSpeed;

        if (m_Velocity != Vector3.zero) gameObject.transform.forward = m_Velocity;

        m_Velocity += Physics.gravity * Time.deltaTime;
        m_CharacterController.Move(m_Velocity * Time.deltaTime);
    }
}

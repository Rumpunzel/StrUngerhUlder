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
        Move(m_Input.m_Movement * m_MovementSpeed);
    }


    public void Move(Vector3 direction)
	{
        m_Velocity = m_StateMachine.Move(direction);
        m_CharacterController.SimpleMove(m_Velocity);
    }
}

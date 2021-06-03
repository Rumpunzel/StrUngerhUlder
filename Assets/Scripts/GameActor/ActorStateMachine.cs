using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Range(0, 2f)] [SerializeField] private float m_SprintPercentage = 1.1f;
    [Range(0, 1f)] [SerializeField] private float m_RunPercentage = .5f;

    private Animator m_Animator;
    private k_STATES m_CurrentState;
    public bool IsGrounded;
    public Vector3 Velocity;


    private void Awake()
	{
		m_Animator = GetComponent<Animator>();
	}

    // Start is called before the first frame update
    void Start()
    {
        EnterState(k_STATES.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        k_STATES newState;

        if (Velocity.magnitude > m_SprintPercentage)
        {
            newState = k_STATES.Sprinting;
        }
        else if (Velocity.magnitude > m_RunPercentage)
        {
            newState = k_STATES.Running;
        }
        else if (Velocity.magnitude > .01f)
        {
            newState = k_STATES.Walking;
        }
        else
        {
            newState = k_STATES.Idle;
        }
        
        if (IsGrounded && newState != m_CurrentState) EnterState(newState);
    }


    public bool CanMove() {
        return m_CurrentState < k_STATES.Dead;
    }

    public float Jump(float jumpForce)
    {
        // Cannot Jump
        if (m_CurrentState >= k_STATES.Jumping) return 0f;
        
        EnterState(k_STATES.Jumping);
        return jumpForce;
    }

    public float Damage(float damage)
    {
        return damage;
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

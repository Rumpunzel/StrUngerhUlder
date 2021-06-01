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
    }

    [Range(0, 2f)] [SerializeField] private float m_SprintPercentage = 1.5f;
    [Range(0, 1f)] [SerializeField] private float m_RunPercentage = .5f;

    private Animator m_Animator;
    private k_STATES m_CurrentState;
    private Vector3 m_Direction;


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
        
    }


    public void ChangeTo(k_STATES newState)
    {
        if (newState == m_CurrentState) return;

        EnterState(newState);
    }

    public Vector3 Move(Vector3 directionVector, bool grounded)
    {
        k_STATES newState;
        m_Direction = directionVector;

        if (m_Direction.magnitude > m_SprintPercentage)
        {
            newState = k_STATES.Sprinting;
        }
        else if (m_Direction.magnitude > m_RunPercentage)
        {
            newState = k_STATES.Running;
        }
        else if (m_Direction.magnitude > .01f)
        {
            newState = k_STATES.Walking;
        }
        else
        {
            newState = k_STATES.Idle;
        }
        
        if (grounded && newState != m_CurrentState) ChangeTo(newState);
        
        return m_Direction;
    }

    public float Jump(float jumpForce)
    {
        // Cannot Jump
        if (m_CurrentState >= k_STATES.Jumping) return 0f;
        
        ChangeTo(k_STATES.Jumping);
        return jumpForce;
    }

    public float Damage(float damage)
    {
        return damage;
    }


    private void EnterState(k_STATES newState)
    {
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

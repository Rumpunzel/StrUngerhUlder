using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStateMachine : MonoBehaviour
{
    public enum k_STATES {
        Idle,
        Walking,
        DISABLED,
        Dead,
    }

    protected k_STATES m_CurrentState;


    private void Awake()
	{
		
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
        if (newState == m_CurrentState || m_CurrentState >= k_STATES.Dead) return;

        EnterState(newState);
    }

    public Vector3 Move(Vector3 directionVector)
    {
        // Cannot Move
        if (!CanAct()) return Vector3.zero;
        
        k_STATES newState;

        if (directionVector.magnitude > .01f)
        {
            newState = k_STATES.Walking;
        }
        else
        {
            newState = k_STATES.Idle;
        }
        
        if (newState != m_CurrentState) ChangeTo(newState);
        
        return directionVector;
    }

    public float Damage(float damage)
    {
        return damage;
    }

    public bool Die(GameObject sender)
    {
        // Not Already Dead
        if (m_CurrentState < k_STATES.Dead)
        {
            ChangeTo(k_STATES.Dead);
        }

        return true;
    }


    private void EnterState(k_STATES newState)
    {
        m_CurrentState = newState;
    }

    private bool CanAct()
    {
        return (m_CurrentState < k_STATES.DISABLED);
    }
}

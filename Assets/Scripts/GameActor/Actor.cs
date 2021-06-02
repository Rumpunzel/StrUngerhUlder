using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Actor : MonoBehaviour
{
    private NavMeshAgent m_NavAgent;
    private Vector3 m_Destination;


    void Awake()
    {
        m_NavAgent = GetComponent<NavMeshAgent>();
    }

    
    public Vector3 Destination {
        get { return m_Destination; }
        set {
            m_Destination = value;
            m_NavAgent.destination = m_Destination;
        }
    }
}

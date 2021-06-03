using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorStateMachine))]
public class PerceptionArea : MonoBehaviour
{
    [Header("Area Attributes")]
    [SerializeField] private LayerMask m_WhatToCheck;
    [Range(0f, 16f)] [SerializeField] private float m_PerceptionRange = 5f;
    [Range(0f, 10f)] [SerializeField] private float m_InteractionRange = 1f;

    [Header("Public Members")]
    public bool LookForObject = false;

    private ActorStateMachine m_ActorStateMachine;
    private Transform m_NearestCollider = null;


    private void Awake()
    {
        m_ActorStateMachine = GetComponent<ActorStateMachine>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_NearestCollider) m_NearestCollider.GetComponent<cakeslice.Outline>().eraseRenderer = true;
        m_NearestCollider = null;

        if (LookForObject)
        {
            m_NearestCollider = CheckForColliders(m_PerceptionRange, m_WhatToCheck);
            if (m_NearestCollider) m_NearestCollider.GetComponent<cakeslice.Outline>().eraseRenderer = false;
        }

        if (m_NearestCollider) InteractWithCollider();
    }


    private Transform CheckForColliders(float rangeToCheck, LayerMask whatToCheck)
    {
        Transform nearestCollider = null;
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, rangeToCheck, whatToCheck);
        float nearDist = float.PositiveInfinity;

        foreach (Collider collider in hitColliders) {
            Vector3 offset = this.transform.position - collider.transform.position;
            float thisDist = offset.sqrMagnitude;

            if (thisDist < nearDist) {
                nearDist = thisDist;
                nearestCollider = collider.transform;
            }
        }

        return nearestCollider;
    }

    private void InteractWithCollider()
    {
        if (Vector3.Distance(this.transform.position, m_NearestCollider.position) <= m_InteractionRange)
        {
            
        }
        else
        {
            m_ActorStateMachine.DestinationInput = m_NearestCollider.position;
        }
    }
}

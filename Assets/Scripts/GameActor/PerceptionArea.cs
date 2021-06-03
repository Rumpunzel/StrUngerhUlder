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

    private Transform m_NearestCollider = null;


    // Update is called once per frame
    private void Update()
    {
        if (m_NearestCollider) m_NearestCollider.GetComponent<cakeslice.Outline>().eraseRenderer = true;
        m_NearestCollider = null;
    }


    public SpottedTransform CheckAreaForObjects()
    {
        m_NearestCollider = CheckForColliders(m_PerceptionRange, m_WhatToCheck);

        if (m_NearestCollider)
        {
            m_NearestCollider.GetComponent<cakeslice.Outline>().eraseRenderer = false;
            return InteractWithCollider();
        }

        return null;
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

    private SpottedTransform InteractWithCollider()
    {
        if (Vector3.Distance(this.transform.position, m_NearestCollider.position) <= m_InteractionRange)
        {
            return new SpottedTransform(m_NearestCollider);
        }
        else
        {
            return new SpottedTransform(m_NearestCollider.position);
        }
    }


    public class SpottedTransform
    {
        public Transform transform = null;
        public Vector3 position;

        public SpottedTransform(Transform t)
        {
            this.transform = t;
            this.position = transform.position;
        }

        public SpottedTransform(Vector3 p)
        {
            this.position = p;
        }
    }
}

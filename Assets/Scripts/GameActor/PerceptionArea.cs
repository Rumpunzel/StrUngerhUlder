using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PerceptionArea : MonoBehaviour
{
    public bool LookForObject = false;

    [SerializeField] private LayerMask m_WhatToCheck;
    [Range(0f, 16f)] [SerializeField] private float m_PerceptionRange = 5f;

    private Transform m_NearestCollider = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForColliders();
    }


    private void CheckForColliders()
    {
        if (m_NearestCollider) m_NearestCollider.GetComponent<cakeslice.Outline>().eraseRenderer = true;
        m_NearestCollider = null;

        if (!LookForObject) return;

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, m_PerceptionRange, m_WhatToCheck);
        float nearDist = float.PositiveInfinity;

        foreach (Collider collider in hitColliders) {
            Vector3 offset = this.transform.position - collider.transform.position;
            float thisDist = offset.sqrMagnitude;

            if (thisDist < nearDist) {
                nearDist = thisDist;
                m_NearestCollider = collider.transform;
            }
        }

        if (m_NearestCollider) m_NearestCollider.GetComponent<cakeslice.Outline>().eraseRenderer = false;
    }
}

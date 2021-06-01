using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionArea : MonoBehaviour
{
    [SerializeField] private LayerMask m_WhatToCheck;

    private ArrayList m_CollidersInArea = new ArrayList(32);
    private Collider m_NearestCollider = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_NearestCollider) m_NearestCollider.GetComponent<cakeslice.Outline>().eraseRenderer = true;
        m_NearestCollider = null;

        float nearestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider collider in m_CollidersInArea)
        {
            float distance = Vector3.Distance(collider.transform.position, currentPosition);
            if (distance < nearestDistance)
            {
                m_NearestCollider = collider;
                nearestDistance = distance;
            }
        }

        if (m_NearestCollider) m_NearestCollider.GetComponent<cakeslice.Outline>().eraseRenderer = false;
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null) return;

        if (((1 << collider.gameObject.layer) & m_WhatToCheck) != 0)
        {
            m_CollidersInArea.Add(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider == null) return;

        if (((1 << collider.gameObject.layer) & m_WhatToCheck) != 0)
        {
            m_CollidersInArea.Remove(collider);
        }
    }
}

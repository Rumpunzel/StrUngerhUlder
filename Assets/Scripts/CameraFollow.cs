using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_FollowTransform;
    [SerializeField] private float m_DistanceOffGround = 10f;
    [SerializeField] private float m_DistanceFromFollow = 10f;
    [SerializeField] private float m_StartingYAngle = 45f;
    

    private Camera m_MainCamera;
    

    private void Awake()
    {
        m_MainCamera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        float angle = m_StartingYAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(m_DistanceFromFollow * Mathf.Cos(angle), m_DistanceOffGround, m_DistanceFromFollow * Mathf.Sin(angle));
        this.transform.position = m_FollowTransform.position + offset;
        this.transform.LookAt(m_FollowTransform);
    }
}

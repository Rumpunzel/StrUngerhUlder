using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_FollowTransform;
    [SerializeField] private Vector3 m_Offset = new Vector3(10f, 10f, 10f);

    private Camera m_MainCamera;
    

    private void Awake()
    {
        m_MainCamera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        this.transform.position = m_FollowTransform.position + m_Offset;
        this.transform.LookAt(m_FollowTransform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_FollowTransform;
    [SerializeField] private float m_DistanceOffGround = 10f;
    [SerializeField] private float m_DistanceFromFollow = 10f;
    [SerializeField] private float m_CameraStartingAngle = 45f;
    [SerializeField] private float m_CameraTurnSpeed = 1f;
    

    private Camera m_MainCamera;
    private Vector3 m_Offset;
    private float m_CameraAngle;
    

    private void Awake()
    {
        m_CameraAngle = m_CameraStartingAngle * Mathf.Deg2Rad;
        m_MainCamera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        m_CameraAngle += Input.GetAxisRaw("CameraHorizontal") * m_CameraTurnSpeed * Time.deltaTime;
        m_Offset = new Vector3(m_DistanceFromFollow * Mathf.Cos(m_CameraAngle), m_DistanceOffGround, m_DistanceFromFollow * Mathf.Sin(m_CameraAngle));
        this.transform.position = m_FollowTransform.position + m_Offset;
        this.transform.LookAt(m_FollowTransform);
    }
}

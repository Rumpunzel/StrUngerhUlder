using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 m_Movement = Vector3.zero;

    private Camera m_MainCamera;
    private Vector3 m_CameraForward;
    private Vector3 m_CameraRight;


    void Update()
    {
        GetInputs();
    }

    protected void GetInputs()
    {
        m_MainCamera = Camera.main;
        m_CameraForward = m_MainCamera.transform.forward;
        m_CameraRight = m_MainCamera.transform.right;
        m_CameraForward.y = 0f;
        m_CameraRight.y = 0f;
        m_CameraForward.Normalize();
        m_CameraRight.Normalize();

        m_Movement = m_CameraForward * Input.GetAxisRaw("Vertical") + m_CameraRight * Input.GetAxisRaw("Horizontal");
    }
}

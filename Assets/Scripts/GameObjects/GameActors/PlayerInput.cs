using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameActor))]
public class PlayerInput : MonoBehaviour
{
    private GameActor m_GameActor;
    private Camera m_MainCamera;
    private Vector3 m_CameraForward;
    private Vector3 m_CameraRight;

    private Vector3 m_Movement = Vector3.zero;


    void Awake()
	{
        m_GameActor = GetComponent<GameActor>();
	}

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
        if (m_Movement.magnitude > 1f) m_Movement.Normalize();

        m_GameActor.m_Direction = m_Movement;
        m_GameActor.m_Sprinting = Input.GetButton("Sprint");
        if (Input.GetButtonDown("Jump")) m_GameActor.m_Jumping = true;
    }
}

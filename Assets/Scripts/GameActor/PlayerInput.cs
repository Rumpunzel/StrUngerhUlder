using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerInput : MonoBehaviour
{
    private CharacterMovement m_CharacterMovement;
    private Camera m_MainCamera;
    private Vector3 m_CameraForward;
    private Vector3 m_CameraRight;

    private Vector3 m_Direction = Vector3.zero;


    void Awake()
	{
        m_CharacterMovement = GetComponent<CharacterMovement>();
	}


    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();

        m_MainCamera = Camera.main;
        m_CameraForward = m_MainCamera.transform.forward;
        m_CameraRight = m_MainCamera.transform.right;
        m_CameraForward.y = 0f;
        m_CameraRight.y = 0f;
        m_CameraForward.Normalize();
        m_CameraRight.Normalize();

        m_Direction = m_CameraRight * inputMovement.x + m_CameraForward * inputMovement.y;
        if (m_Direction.magnitude > 1f) m_Direction.Normalize();

        m_CharacterMovement.Direction = m_Direction;
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        if (value.started) m_CharacterMovement.Sprinting = true;
        
        if (value.canceled) m_CharacterMovement.Sprinting = false;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started) m_CharacterMovement.Jump();
    }
}

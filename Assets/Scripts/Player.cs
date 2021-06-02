using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [SerializeField] private Actor m_PlayerActor;

    private Camera m_PlayerCamera;
    private Vector3 m_CameraForward;
    private Vector3 m_CameraRight;

    private CharacterMovement m_PlayerMovement;
    private Vector3 m_Direction = Vector3.zero;


    private void Start()
    {
        m_PlayerCamera = Camera.main;
        PlayerActor = m_PlayerActor;
    }


    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();

        m_CameraForward = m_PlayerCamera.transform.forward;
        m_CameraRight = m_PlayerCamera.transform.right;
        m_CameraForward.y = 0f;
        m_CameraRight.y = 0f;
        m_CameraForward.Normalize();
        m_CameraRight.Normalize();

        m_Direction = m_CameraRight * inputMovement.x + m_CameraForward * inputMovement.y;
        if (m_Direction.magnitude > 1f) m_Direction.Normalize();

        m_PlayerMovement.Direction = m_Direction;
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        if (value.started) m_PlayerMovement.Sprinting = true;
        
        if (value.canceled) m_PlayerMovement.Sprinting = false;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started) m_PlayerMovement.Jump();
    }

    public void OnInteract(InputAction.CallbackContext value)
    {

    }


    public Actor PlayerActor {
        get { return m_PlayerActor; }
        set {
            m_PlayerActor = value;
            m_PlayerCamera.GetComponent<CameraFollow>().FollowTransform = m_PlayerActor.transform;
            m_PlayerMovement = m_PlayerActor.GetComponent<CharacterMovement>();
        }
    }
}

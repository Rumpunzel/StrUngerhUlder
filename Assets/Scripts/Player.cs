using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerObject;

    private Camera m_MainCamera;
    private Vector3 m_CameraForward;
    private Vector3 m_CameraRight;

    private CharacterMovement m_PlayerMovement;
    private Vector3 m_Direction = Vector3.zero;


    private void Start()
    {
        m_MainCamera = Camera.main;
        PlayerObject = m_PlayerObject;
    }


    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();

        m_CameraForward = m_MainCamera.transform.forward;
        m_CameraRight = m_MainCamera.transform.right;
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


    public GameObject PlayerObject {
        get { return m_PlayerObject; }
        set {
            m_PlayerObject = value;
            m_MainCamera.GetComponent<CameraFollow>().FollowTransform = m_PlayerObject.transform;
            m_PlayerMovement = m_PlayerObject.GetComponent<CharacterMovement>();
        }
    }
}

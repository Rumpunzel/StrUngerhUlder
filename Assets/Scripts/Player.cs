using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerObject;
    [SerializeField] private LayerMask m_WorldLayer;

    private Camera m_PlayerCamera;
    private Vector3 m_CameraForward;
    private Vector3 m_CameraRight;

    private CharacterMovement m_PlayerMovement;
    private Vector3 m_MouseDestination;
    private bool m_Walk = false;
    private Vector3 m_Direction;


    private void Start()
    {
        m_PlayerCamera = Camera.main;
        PlayerObject = m_PlayerObject;
    }

    private void Update()
    {
        if (m_Walk) WalkToPoint();
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

    public void OnMoveToPoint(InputAction.CallbackContext value)
    {
        if (value.performed) m_Walk = true;
        if (value.canceled) m_Walk = false;
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
            m_PlayerCamera.GetComponent<CameraFollow>().FollowTransform = m_PlayerObject.transform;
            m_PlayerMovement = m_PlayerObject.GetComponent<CharacterMovement>();
        }
    }


    private void WalkToPoint()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = 25f;
        print(mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(m_PlayerCamera.ScreenToWorldPoint(mousePosition), m_PlayerCamera.transform.forward, out hit, 100f, m_WorldLayer))
        {
            m_MouseDestination = hit.point;
            m_PlayerMovement.Destination = m_MouseDestination;
            print(m_MouseDestination);
            print(hit.collider.gameObject.name);
        }
    }
}

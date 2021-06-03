using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerObject;
    [SerializeField] private LayerMask m_WorldLayer;

    private Camera m_PlayerCamera;
    private Vector3 m_CameraForward;
    private Vector3 m_CameraRight;

    private CharacterMovement m_PlayerMovement;
    private PerceptionArea m_PlayerPerception;

    private Vector3 m_MouseDestination;
    private Vector3 m_Direction;
    private bool m_WalkToPoint = false;


    private void Start()
    {
        m_PlayerCamera = Camera.main;
        PlayerObject = m_PlayerObject;
    }

    private void Update()
    {
        if (m_WalkToPoint) WalkToPoint();
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
        
        m_PlayerMovement.DirectionInput = m_Direction;
    }

    public void OnMoveToPoint(InputAction.CallbackContext value)
    {
        if (value.performed) m_WalkToPoint = true;
        if (value.canceled) m_WalkToPoint = false;
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        if (value.started) m_PlayerMovement.SprintInput = true;
        if (value.canceled) m_PlayerMovement.SprintInput = false;
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started) m_PlayerMovement.JumpInput = true;
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if (value.started) m_PlayerPerception.LookForObject = true;
        if (value.canceled) m_PlayerPerception.LookForObject = false;
    }


    public GameObject PlayerObject {
        get { return m_PlayerObject; }
        set {
            m_PlayerObject = value;
            m_PlayerCamera.GetComponent<CameraFollow>().FollowTransform = m_PlayerObject.transform;
            m_PlayerMovement = m_PlayerObject.GetComponent<CharacterMovement>();
            m_PlayerPerception = m_PlayerObject.GetComponent<PerceptionArea>();
        }
    }


    private void WalkToPoint()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = m_PlayerCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100f, m_WorldLayer))
        {
            m_MouseDestination = hit.point;
            m_PlayerMovement.DestinationInut = m_MouseDestination;
        }
    }
}

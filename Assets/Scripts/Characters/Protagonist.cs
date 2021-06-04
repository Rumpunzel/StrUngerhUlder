using System;
using UnityEngine;

/// <summary>
/// <para>This component consumes input on the InputReader and stores its values. The input is then read, and manipulated, by the StateMachines's Actions.</para>
/// </summary>
public class Protagonist : MonoBehaviour
{
	[SerializeField] private InputReader m_InputReader = default;

    public TransformAnchor gameplayCameraTransform;

	[SerializeField] private VoidEventChannelSO m_OpenInventoryChannel = default;

	private Vector2 m_InputVector;
	private float m_PreviousSpeed;

	//These fields are read and manipulated by the StateMachine actions
	[NonSerialized] public bool jumpInput;
	[NonSerialized] public bool extraActionInput;
	[NonSerialized] public bool attackInput;
	[NonSerialized] public Vector3 movementInput; //Initial input coming from the Protagonist script
	[NonSerialized] public Vector3 movementVector; //Final movement vector, manipulated by the StateMachine actions
	[NonSerialized] public ControllerColliderHit lastHit;
	[NonSerialized] public bool isRunning; // Used when using the keyboard to run, brings the normalised speed to 1

	public const float GRAVITY_MULTIPLIER = 2.2f;
	public const float MAX_FALL_SPEED = -50f;
	public const float MAX_RISE_SPEED = 100f;
	//public const float GRAVITY_COMEBACK_MULTIPLIER = .03f;
	//public const float GRAVITY_DIVIDER = .9f;
	public const float AIR_RESISTANCE = 5f;

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		lastHit = hit;
	}

	//Adds listeners for events being triggered in the InputReader script
	private void OnEnable()
	{
        m_InputReader.jumpEvent += OnJumpInitiated;
        m_InputReader.jumpCanceledEvent += OnJumpCanceled;
        m_InputReader.moveEvent += OnMove;
        m_InputReader.startedRunning += OnStartedRunning;
        m_InputReader.stoppedRunning += OnStoppedRunning;
        //m_InputReader.attackEvent += OnStartedAttack;
        //...
    }

	//Removes all listeners to the events coming from the InputReader script
	private void OnDisable()
	{
        m_InputReader.jumpEvent -= OnJumpInitiated;
        m_InputReader.jumpCanceledEvent -= OnJumpCanceled;
        m_InputReader.moveEvent -= OnMove;
        m_InputReader.startedRunning -= OnStartedRunning;
        m_InputReader.stoppedRunning -= OnStoppedRunning;
        //m_InputReader.attackEvent -= OnStartedAttack;
        //...
    }

	private void Update()
	{
		RecalculateMovement();
	}

	private void RecalculateMovement()
	{
		float targetSpeed = 0f;
		Vector3 adjustedMovement;

		if (gameplayCameraTransform)
		{
			//Get the two axes from the camera and flatten them on the XZ plane
			Vector3 cameraForward = gameplayCameraTransform.Transform.forward;
			cameraForward.y = 0f;
			Vector3 cameraRight = gameplayCameraTransform.Transform.right;
			cameraRight.y = 0f;

			//Use the two axes, modulated by the corresponding inputs, and construct the final vector
			adjustedMovement = cameraRight.normalized * m_InputVector.x +
				cameraForward.normalized * m_InputVector.y;
		}
		else
		{
			//No CameraManager exists in the scene, so the input is just used absolute in world-space
			Debug.LogWarning("No gameplay camera in the scene. Movement orientation will not be correct.");
			adjustedMovement = new Vector3(m_InputVector.x, 0f, m_InputVector.y);
		}

		//Fix to avoid getting a Vector3.zero vector, which would result in the player turning to x:0, z:0
		if (m_InputVector.sqrMagnitude == 0f)
			adjustedMovement = transform.forward * (adjustedMovement.magnitude + .01f);

		//Accelerate/decelerate
		targetSpeed = Mathf.Clamp01(m_InputVector.magnitude);

		if (targetSpeed > 0f)
		{
			// This is used to set the speed to the maximum if holding the Shift key,
			// to allow keyboard players to "run"
			if (isRunning)
				targetSpeed = 1f;

			if (attackInput)
				targetSpeed = .05f;
		}
		
		targetSpeed = Mathf.Lerp(m_PreviousSpeed, targetSpeed, Time.deltaTime * 4f);

		movementInput = adjustedMovement.normalized * targetSpeed;
        if (movementInput != Vector3.zero)
		{
            Quaternion toRotation = Quaternion.LookRotation(movementInput, Vector3.up);
        	this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, toRotation, 480f * Time.deltaTime);
		}

		m_PreviousSpeed = targetSpeed;
	}

	//---- EVENT LISTENERS ----

	private void OnMove(Vector2 movement)
	{
		m_InputVector = movement;
	}

	private void OnJumpInitiated()
	{
		jumpInput = true;
	}

	private void OnJumpCanceled()
	{
		jumpInput = false;
	}

	private void OnStoppedRunning() => isRunning = false;

	private void OnStartedRunning() => isRunning = true;


	private void OnStartedAttack() => attackInput = true;

	// Triggered from Animation Event
	public void ConsumeAttackInput() => attackInput = false;
}

using UnityEngine;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "ApplyMovementVector", menuName = "State Machines/Actions/Apply Movement Vector")]
public class ApplyMovementVectorActionSO : StateActionSO<ApplyMovementVectorAction> { }

public class ApplyMovementVectorAction : StateAction
{
	//Component references
	private Protagonist m_ProtagonistScript;
	private CharacterController m_CharacterController;

	public override void Awake(StateMachine stateMachine)
	{
		m_ProtagonistScript = stateMachine.GetComponent<Protagonist>();
		m_CharacterController = stateMachine.GetComponent<CharacterController>();
	}

	public override void OnUpdate()
	{
		m_CharacterController.Move(m_ProtagonistScript.movementVector * Time.deltaTime);
		m_ProtagonistScript.movementVector = m_CharacterController.velocity;
	}
}

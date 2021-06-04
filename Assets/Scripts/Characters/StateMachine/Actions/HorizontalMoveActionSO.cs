using UnityEngine;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HorizontalMove", menuName = "State Machines/Actions/Horizontal Move")]
public class HorizontalMoveActionSO : StateActionSO<HorizontalMoveAction>
{
	[Tooltip("Horizontal XZ plane speed multiplier")]
	public float speed = 8f;
}

public class HorizontalMoveAction : StateAction
{
	//Component references
	private Protagonist m_ProtagonistScript;
	private new HorizontalMoveActionSO m_OriginSO => (HorizontalMoveActionSO)base.OriginSO; // The SO this StateAction spawned from

	public override void Awake(StateMachine stateMachine)
	{
		m_ProtagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		m_ProtagonistScript.movementVector.x = m_ProtagonistScript.movementInput.x * m_OriginSO.speed;
		m_ProtagonistScript.movementVector.z = m_ProtagonistScript.movementInput.z * m_OriginSO.speed;
	}
}

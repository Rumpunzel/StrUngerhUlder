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
	private Protagonist m_Protagonist;
	private new HorizontalMoveActionSO m_OriginSO => (HorizontalMoveActionSO)base.OriginSO; // The SO this StateAction spawned from

	public override void Awake(StateMachine stateMachine)
	{
		m_Protagonist = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		m_Protagonist.movementVector.x = m_Protagonist.movementInput.x * m_OriginSO.speed;
		m_Protagonist.movementVector.z = m_Protagonist.movementInput.z * m_OriginSO.speed;
	}
}

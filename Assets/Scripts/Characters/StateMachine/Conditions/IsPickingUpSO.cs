using UnityEngine;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is Picking Up")]
public class IsPickingUpSO : StateConditionSO<IsPickingUpCondition> { }

public class IsPickingUpCondition : Condition
{
	//Component references
	private InteractionManager m_InteractScript;

	public override void Awake(StateMachine stateMachine)
	{
		m_InteractScript = stateMachine.GetComponent<InteractionManager>();
	}

	protected override bool Statement()
	{
		if (m_InteractScript.currentInteractionType == InteractionType.PickUp)
		{
			// Consume it
			m_InteractScript.currentInteractionType = InteractionType.None;

			return true;
		}
		else
		{
			return false;
		}
	}
}

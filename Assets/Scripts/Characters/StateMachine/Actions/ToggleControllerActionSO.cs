using System;
using UnityEngine;
using UnityEngine.AI;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;
using Moment = Strungerhulder.StateMachine.StateAction.SpecificMoment;

/// <summary>
/// Flexible StateActionSO for the StateMachine which allows to set any parameter on the Animator, in any moment of the state (OnStateEnter, OnStateExit, or each OnUpdate).
/// </summary>
[CreateAssetMenu(fileName = "ToggleControllerAction", menuName = "State Machines/Actions/Toggle Controller")]
public class ToggleControllerActionSO : StateActionSO
{
    public enum ControllerType
    {
        CharacterController,
		NavMeshAgent,
    }

	public ControllerType controllerType = default;

	protected override StateAction CreateAction() => new ToggleControllerAction();
}

public class ToggleControllerAction : StateAction
{
    //Component references
    private Protagonist m_Protagonist;
    private CharacterController m_CharacterController;
    private NavMeshAgent m_NavAgent;

	private new ToggleControllerActionSO m_OriginSO => (ToggleControllerActionSO)base.OriginSO; // The SO this StateAction spawned from


	public override void Awake(StateMachine stateMachine)
	{
        m_Protagonist = stateMachine.GetComponent<Protagonist>();

        m_CharacterController = stateMachine.GetComponent<CharacterController>();

        m_NavAgent = stateMachine.GetComponent<NavMeshAgent>();
        m_NavAgent.angularSpeed = Protagonist.TURN_RATE;
	}

	public override void OnStateEnter()
	{
		switch(m_OriginSO.controllerType)
		{
			case ToggleControllerActionSO.ControllerType.CharacterController:
				m_Protagonist.movingToDestination = false;
                m_CharacterController.enabled = true;
                m_NavAgent.enabled = false;
				break;
            case ToggleControllerActionSO.ControllerType.NavMeshAgent:
                m_Protagonist.movingToDestination = true;
                m_CharacterController.enabled = false;
                m_NavAgent.enabled = true;
                break;
		}
	}

	public override void OnUpdate() { }
}

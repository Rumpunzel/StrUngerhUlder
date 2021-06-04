using System;
using UnityEngine;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;
using Moment = Strungerhulder.StateMachine.StateAction.SpecificMoment;

/// <summary>
/// Flexible StateActionSO for the StateMachine which allows to set any parameter on the Animator, in any moment of the state (OnStateEnter, OnStateExit, or each OnUpdate).
/// </summary>
[CreateAssetMenu(fileName = "AnimatorParameterAction", menuName = "State Machines/Actions/Set Animator Parameter")]
public class AnimatorParameterActionSO : StateActionSO
{
	public ParameterType parameterType = default;
	public string parameterName = default;

	public bool boolValue = default;
	public int intValue = default;
	public float floatValue = default;

	public Moment whenToRun = default; // Allows this StateActionSO type to be reused for all 3 state moments

	protected override StateAction CreateAction() => new AnimatorParameterAction(Animator.StringToHash(parameterName));

	public enum ParameterType
	{
		Bool, Int, Float, Trigger,
	}
}

public class AnimatorParameterAction : StateAction
{
	//Component references
	private Animator m_Animator;
	private AnimatorParameterActionSO OriginSO => (AnimatorParameterActionSO)base.OriginSO; // The SO this StateAction spawned from
	private int m_ParameterHash;

	public AnimatorParameterAction(int parameterHash)
	{
		m_ParameterHash = parameterHash;
	}

	public override void Awake(StateMachine stateMachine)
	{
		m_Animator = stateMachine.GetComponent<Animator>();
	}

	public override void OnStateEnter()
	{
		if (OriginSO.whenToRun == SpecificMoment.OnStateEnter)
			SetParameter();
	}

	public override void OnStateExit()
	{
		if (OriginSO.whenToRun == SpecificMoment.OnStateExit)
			SetParameter();
	}

	private void SetParameter()
	{
		switch (OriginSO.parameterType)
		{
			case AnimatorParameterActionSO.ParameterType.Bool:
				m_Animator.SetBool(m_ParameterHash, OriginSO.boolValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Int:
				m_Animator.SetInteger(m_ParameterHash, OriginSO.intValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Float:
				m_Animator.SetFloat(m_ParameterHash, OriginSO.floatValue);
				break;
			case AnimatorParameterActionSO.ParameterType.Trigger:
				m_Animator.SetTrigger(m_ParameterHash);
				break;
		}
	}

	public override void OnUpdate() { }
}

﻿using System;
using UnityEngine;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;
using Moment = Strungerhulder.StateMachine.StateAction.SpecificMoment;

/// <summary>
/// Flexible StateActionSO for the StateMachine which allows to set any parameter on the Animator, in any moment of the state (OnStateEnter, OnStateExit, or each OnUpdate).
/// </summary>
[CreateAssetMenu(fileName = "AnimatorMoveSpeedAction", menuName = "State Machines/Actions/Set Animator Move Speed")]
public class AnimatorMoveSpeedActionSO : StateActionSO
{
	public string parameterName = default;

	protected override StateAction CreateAction() => new AnimatorMoveSpeedAction(Animator.StringToHash(parameterName));
}

public class AnimatorMoveSpeedAction : StateAction
{
	//Component references
	private Animator m_Animator;
	private Protagonist m_Protagonist;

	private new AnimatorParameterActionSO m_OriginSO => (AnimatorParameterActionSO)base.OriginSO; // The SO this StateAction spawned from
	private int m_ParameterHash;

	public AnimatorMoveSpeedAction(int parameterHash)
	{
		m_ParameterHash = parameterHash;
	}

	public override void Awake(StateMachine stateMachine)
	{
		m_Animator = stateMachine.GetComponent<Animator>();
		m_Protagonist = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		//TODO: do we like that we're using the magnitude here, per frame? Can this be done in a smarter way?
		float normalisedSpeed = m_Protagonist.movementInput.magnitude;
		m_Animator.SetFloat(m_ParameterHash, normalisedSpeed);
	}
}
﻿using UnityEngine;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Time elapsed")]
public class TimeElapsedConditionSO : StateConditionSO<TimeElapsedCondition>
{
    public float m_TimerLength = .5f;
}

public class TimeElapsedCondition : Condition
{
    private float m_StartTime;
    private new TimeElapsedConditionSO m_OriginSO => (TimeElapsedConditionSO)base.OriginSO; // The SO this Condition spawned from

    public override void OnStateEnter()
    {
        m_StartTime = Time.time;
    }

    protected override bool Statement() => Time.time >= m_StartTime + m_OriginSO.m_TimerLength;
}

#if UNITY_EDITOR

using System;
using System.Text;
using UnityEngine;

namespace Strungerhulder.StateMachine.Debugging
{
	/// <summary>
	/// Class specialized in debugging the state transitions, should only be used while in editor mode.
	/// </summary>
	[Serializable]
	internal class StateMachineDebugger
	{
		[SerializeField]
		[Tooltip("Issues a debug log when a state transition is triggered")]
		internal bool debugTransitions = false;

		[SerializeField]
		[Tooltip("List all conditions evaluated, the result is read: ConditionName == BooleanResult [PassedTest]")]
		internal bool appendConditionsInfo = true;

		[SerializeField]
		[Tooltip("List all actions activated by the new State")]
		internal bool appendActionsInfo = true;

		[SerializeField]
		[Tooltip("The current State name [Readonly]")]
		internal string currentState;

		private StateMachine m_StateMachine;
		private StringBuilder m_LogBuilder;
		private string m_TargetState = string.Empty;

		private const string CHECK_MARK = "\u2714";
		private const string UNCHECK_MARK = "\u2718";
		private const string THICK_ARROW = "\u279C";
		private const string SHARP_ARROW = "\u27A4";

		/// <summary>
		/// Must be called together with <c>StateMachine.Awake()</c>
		/// </summary>
		internal void Awake(StateMachine stateMachine)
		{
			m_StateMachine = stateMachine;
			m_LogBuilder = new StringBuilder();

			currentState = stateMachine.m_CurrentState.m_OriginSO.name;
		}

		internal void TransitionEvaluationBegin(string targetState)
		{
			m_TargetState = targetState;

			if (!debugTransitions)
				return;

			m_LogBuilder.Clear();
			m_LogBuilder.AppendLine($"{m_StateMachine.name} state changed");
			m_LogBuilder.AppendLine($"{currentState}  {SHARP_ARROW}  {m_TargetState}");

			if (appendConditionsInfo)
			{
				m_LogBuilder.AppendLine();
				m_LogBuilder.AppendLine($"Transition Conditions:");
			}
		}

		internal void TransitionConditionResult(string conditionName, bool result, bool isMet)
		{
			if (!debugTransitions || m_LogBuilder.Length == 0 || !appendConditionsInfo)
				return;

			m_LogBuilder.Append($"    {THICK_ARROW} {conditionName} == {result}");

			if (isMet)
				m_LogBuilder.AppendLine($" [{CHECK_MARK}]");
			else
				m_LogBuilder.AppendLine($" [{UNCHECK_MARK}]");
		}

		internal void TransitionEvaluationEnd(bool passed, StateAction[] actions)
		{
			if (passed)
				currentState = m_TargetState;

			if (!debugTransitions || m_LogBuilder.Length == 0)
				return;

			if (passed)
			{
				LogActions(actions);
				PrintDebugLog();
			}

			m_LogBuilder.Clear();
		}

		private void LogActions(StateAction[] actions)
		{
			if (!appendActionsInfo)
				return;

			m_LogBuilder.AppendLine();
			m_LogBuilder.AppendLine("State Actions:");

			foreach (StateAction action in actions)
			{
				m_LogBuilder.AppendLine($"    {THICK_ARROW} {action.m_OriginSO.name}");
			}
		}

		private void PrintDebugLog()
		{
			m_LogBuilder.AppendLine();
			m_LogBuilder.Append("--------------------------------");

			Debug.Log(m_LogBuilder.ToString());
		}
	}
}

#endif

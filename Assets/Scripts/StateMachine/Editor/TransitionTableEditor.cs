using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Strungerhulder.StateMachine.ScriptableObjects;
using static UnityEditor.EditorGUILayout;
using Object = UnityEngine.Object;

namespace Strungerhulder.StateMachine.Editor
{
	[CustomEditor(typeof(TransitionTableSO))]
	internal class TransitionTableEditor : UnityEditor.Editor
	{
		// Property with all the transitions.
		private SerializedProperty m_Transitions;

		// m_FromStates and m_TransitionsByFromStates form a State->Transitions dictionary.
		private List<Object> m_FromStates;
		private List<List<TransitionDisplayHelper>> m_TransitionsByFromStates;

		// Index of the state currently toggled on, -1 if none is.
		internal int m_ToggledIndex = -1;

		// Helper class to add new transitions.
		private AddTransitionHelper m_AddTransitionHelper;

		// Editor to display the StateSO inspector.
		private UnityEditor.Editor m_CachedStateEditor;
		private bool m_DisplayStateEditor;

		private void OnEnable()
		{
			m_AddTransitionHelper = new AddTransitionHelper(this);
			Undo.undoRedoPerformed += Reset;
			Reset();
		}

		private void OnDisable()
		{
			Undo.undoRedoPerformed -= Reset;
			m_AddTransitionHelper?.Dispose();
		}

		/// <summary>
		/// Method to fully reset the editor. Used whenever adding, removing and reordering transitions.
		/// </summary>
		internal void Reset()
		{
			serializedObject.Update();
			var toggledState = m_ToggledIndex > -1 ? m_FromStates[m_ToggledIndex] : null;
			m_Transitions = serializedObject.FindProperty("m_Transitions");
			GroupByFromState();
			m_ToggledIndex = toggledState ? m_FromStates.IndexOf(toggledState) : -1;
		}

		public override void OnInspectorGUI()
		{
			if (!m_DisplayStateEditor)
				TransitionTableGUI();
			else
				StateEditorGUI();
		}

		private void StateEditorGUI()
		{
			Separator();

			// Back button
			if (GUILayout.Button(EditorGUIUtility.IconContent("scrollleft"), GUILayout.Width(35), GUILayout.Height(20))
				|| m_CachedStateEditor.serializedObject == null)
			{
				m_DisplayStateEditor = false;
				return;
			}


			Separator();
			EditorGUILayout.HelpBox("Edit the Actions that a State performs per frame. The order represent the order of execution.", MessageType.Info);
			Separator();

			// State name
			EditorGUILayout.LabelField(m_CachedStateEditor.target.name, EditorStyles.boldLabel);
			Separator();
			m_CachedStateEditor.OnInspectorGUI();
		}

		private void TransitionTableGUI()
		{
			Separator();
			EditorGUILayout.HelpBox("Click on any State's name to see the Transitions it contains, or click the Pencil/Wrench icon to see its Actions.", MessageType.Info);
			Separator();

			// For each fromState
			for (int i = 0; i < m_FromStates.Count; i++)
			{
				var stateRect = BeginVertical(ContentStyle.WithPaddingAndMargins);
				EditorGUI.DrawRect(stateRect, ContentStyle.LightGray);

				var transitions = m_TransitionsByFromStates[i];

				// State Header
				var headerRect = BeginHorizontal();
				{
					BeginVertical();
					string label = transitions[0].SerializedTransition.FromState.objectReferenceValue.name;

					if (i == 0)
						label += " (Initial State)";

					headerRect.height = EditorGUIUtility.singleLineHeight;
					GUILayoutUtility.GetRect(headerRect.width, headerRect.height);
					headerRect.x += 5;

					// Toggle
					{
						var toggleRect = headerRect;
						toggleRect.width -= 140;
						m_ToggledIndex =
							EditorGUI.BeginFoldoutHeaderGroup(toggleRect, m_ToggledIndex == i, label, ContentStyle.StateListStyle) ?
							i : m_ToggledIndex == i ? -1 : m_ToggledIndex;
					}

					Separator();
					EndVertical();

					// State Header Buttons
					{
						bool Button(Rect position, string icon) => GUI.Button(position, EditorGUIUtility.IconContent(icon));

						var buttonRect = new Rect(x: headerRect.width - 25, y: headerRect.y, width: 35, height: 20);

						// Move state down
						if (i < m_FromStates.Count - 1)
						{
							if (Button(buttonRect, "scrolldown"))
							{
								ReorderState(i, false);
								EarlyOut();
								return;
							}
							buttonRect.x -= 40;
						}

						// Move state up
						if (i > 0)
						{
							if (Button(buttonRect, "scrollup"))
							{
								ReorderState(i, true);
								EarlyOut();
								return;
							}
							buttonRect.x -= 40;
						}

						// Switch to state editor
						if (Button(buttonRect, "SceneViewTools"))
						{
							DisplayStateEditor(transitions[0].SerializedTransition.FromState.objectReferenceValue);
							EarlyOut();
							return;
						}

						void EarlyOut()
						{
							EndHorizontal();
							EndFoldoutHeaderGroup();
							EndVertical();
							EndHorizontal();
						}
					}
				}
				EndHorizontal();

				if (m_ToggledIndex == i)
				{
					EditorGUI.BeginChangeCheck();
					stateRect.y += EditorGUIUtility.singleLineHeight * 2;

					foreach (var transition in transitions) // Display all the transitions in the state
					{
						if (transition.Display(ref stateRect)) // Return if there were changes
						{
							EditorGUI.EndChangeCheck();
							EndFoldoutHeaderGroup();
							EndVertical();
							EndHorizontal();
							return;
						}
						Separator();
					}

					if (EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();
				}

				EndFoldoutHeaderGroup();
				EndVertical();
				Separator();
			}

			var rect = BeginHorizontal();
			Space(rect.width - 55);

			// Display add transition button
			m_AddTransitionHelper.Display(rect);

			EndHorizontal();
		}

		internal void DisplayStateEditor(Object state)
		{
			if (m_CachedStateEditor == null)
				m_CachedStateEditor = CreateEditor(state, typeof(StateEditor));
			else
				CreateCachedEditor(state, typeof(StateEditor), ref m_CachedStateEditor);

			m_DisplayStateEditor = true;
		}

		/// <summary>
		/// Move a state up or down
		/// </summary>
		/// <param name="index">Index of the state in m_FromStates</param>
		/// <param name="up">Moving up(true) or down(true)</param>
		internal void ReorderState(int index, bool up)
		{
			var toggledState = m_ToggledIndex > -1 ? m_FromStates[m_ToggledIndex] : null;

			if (!up)
				index++;

			var transitions = m_TransitionsByFromStates[index];
			int transitionIndex = transitions[0].SerializedTransition.Index;
			int targetIndex = m_TransitionsByFromStates[index - 1][0].SerializedTransition.Index;
			m_Transitions.MoveArrayElement(transitionIndex, targetIndex);

			ApplyModifications($"Moved {m_FromStates[index].name} State {(up ? "up" : "down")}");

			if (toggledState)
				m_ToggledIndex = m_FromStates.IndexOf(toggledState);
		}

		/// <summary>
		/// Add a new transition. If a transition with the same from and to states is found,
		/// the conditions in the new transition are added to it.
		/// </summary>
		/// <param name="source">Source Transition</param>
		internal void AddTransition(SerializedTransition source)
		{
			SerializedTransition transition;
			if (TryGetExistingTransition(source.FromState, source.ToState, out int fromIndex, out int toIndex))
				transition = m_TransitionsByFromStates[fromIndex][toIndex].SerializedTransition;
			else
			{
				int count = m_Transitions.arraySize;
				m_Transitions.InsertArrayElementAtIndex(count);
				transition = new SerializedTransition(m_Transitions.GetArrayElementAtIndex(count));
				transition.ClearProperties();
				transition.FromState.objectReferenceValue = source.FromState.objectReferenceValue;
				transition.ToState.objectReferenceValue = source.ToState.objectReferenceValue;
			}

			CopyConditions(transition.Conditions, source.Conditions);

			ApplyModifications($"Added transition from {transition.FromState} to {transition.ToState}");

			m_ToggledIndex = fromIndex >= 0 ? fromIndex : m_FromStates.Count - 1;
		}

		/// <summary>
		/// Move a transition up or down
		/// </summary>
		/// <param name="serializedTransition">The transition to move</param>
		/// <param name="up">Move up(true) or down(false)</param>
		internal void ReorderTransition(SerializedTransition serializedTransition, bool up)
		{
			int stateIndex = m_FromStates.IndexOf(serializedTransition.FromState.objectReferenceValue);
			var stateTransitions = m_TransitionsByFromStates[stateIndex];
			int index = stateTransitions.FindIndex(t => t.SerializedTransition.Index == serializedTransition.Index);

			(int currentIndex, int targetIndex) = up ?
				(serializedTransition.Index, stateTransitions[index - 1].SerializedTransition.Index) :
				(stateTransitions[index + 1].SerializedTransition.Index, serializedTransition.Index);

			m_Transitions.MoveArrayElement(currentIndex, targetIndex);

			ApplyModifications($"Moved transition to {serializedTransition.ToState.objectReferenceValue.name} {(up ? "up" : "down")}");

			m_ToggledIndex = stateIndex;
		}

		/// <summary>
		/// Remove a transition.
		/// </summary>
		/// <param name="serializedTransition">Transition to delete.</param>
		internal void RemoveTransition(SerializedTransition serializedTransition)
		{
			int stateIndex = m_FromStates.IndexOf(serializedTransition.FromState.objectReferenceValue);
			var stateTransitions = m_TransitionsByFromStates[stateIndex];
			int count = stateTransitions.Count;
			int index = stateTransitions.FindIndex(t => t.SerializedTransition.Index == serializedTransition.Index);
			int deleteIndex = serializedTransition.Index;

			if (index == 0 && count > 1)
				m_Transitions.MoveArrayElement(stateTransitions[1].SerializedTransition.Index, deleteIndex++);

			m_Transitions.DeleteArrayElementAtIndex(deleteIndex);

			ApplyModifications($"Deleted transition from {serializedTransition.FromState.objectReferenceValue.name} " +
				"to {serializedTransition.ToState.objectReferenceValue.name}");

			if (count > 1)
				m_ToggledIndex = stateIndex;
		}

		internal List<SerializedTransition> GetStateTransitions(Object state)
		{
			return m_TransitionsByFromStates[m_FromStates.IndexOf(state)].Select(t => t.SerializedTransition).ToList();
		}

		private void CopyConditions(SerializedProperty copyTo, SerializedProperty copyFrom)
		{
			for (int i = 0, j = copyTo.arraySize; i < copyFrom.arraySize; i++, j++)
			{
				copyTo.InsertArrayElementAtIndex(j);
				var cond = copyTo.GetArrayElementAtIndex(j);
				var srcCond = copyFrom.GetArrayElementAtIndex(i);
				cond.FindPropertyRelative("ExpectedResult").enumValueIndex = srcCond.FindPropertyRelative("ExpectedResult").enumValueIndex;
				cond.FindPropertyRelative("Operator").enumValueIndex = srcCond.FindPropertyRelative("Operator").enumValueIndex;
				cond.FindPropertyRelative("Condition").objectReferenceValue = srcCond.FindPropertyRelative("Condition").objectReferenceValue;
			}
		}

		private bool TryGetExistingTransition(SerializedProperty from, SerializedProperty to, out int fromIndex, out int toIndex)
		{
			fromIndex = m_FromStates.IndexOf(from.objectReferenceValue);
			toIndex = -1;

			if (fromIndex < 0)
				return false;

			toIndex = m_TransitionsByFromStates[fromIndex].FindIndex(
				transitionHelper => transitionHelper.SerializedTransition.ToState.objectReferenceValue == to.objectReferenceValue);

			return toIndex >= 0;
		}

		private void GroupByFromState()
		{
			var groupedTransitions = new Dictionary<Object, List<TransitionDisplayHelper>>();
			int count = m_Transitions.arraySize;
			for (int i = 0; i < count; i++)
			{
				var serializedTransition = new SerializedTransition(m_Transitions, i);
				if (serializedTransition.FromState.objectReferenceValue == null)
				{
					Debug.LogError("Transition with invalid \"From State\" found in table " + serializedObject.targetObject.name + ", deleting...");
					m_Transitions.DeleteArrayElementAtIndex(i);
					ApplyModifications("Invalid transition deleted");
					return;
				}
				if (serializedTransition.ToState.objectReferenceValue == null)
				{
					Debug.LogError("Transition with invalid \"Target State\" found in table " + serializedObject.targetObject.name + ", deleting...");
					m_Transitions.DeleteArrayElementAtIndex(i);
					ApplyModifications("Invalid transition deleted");
					return;
				}

				if (!groupedTransitions.TryGetValue(serializedTransition.FromState.objectReferenceValue, out var groupedProps))
				{
					groupedProps = new List<TransitionDisplayHelper>();
					groupedTransitions.Add(serializedTransition.FromState.objectReferenceValue, groupedProps);
				}
				groupedProps.Add(new TransitionDisplayHelper(serializedTransition, this));
			}

			m_FromStates = groupedTransitions.Keys.ToList();
			m_TransitionsByFromStates = new List<List<TransitionDisplayHelper>>();

			foreach (var fromState in m_FromStates)
				m_TransitionsByFromStates.Add(groupedTransitions[fromState]);
		}

		private void ApplyModifications(string msg)
		{
			Undo.RecordObject(serializedObject.targetObject, msg);
			serializedObject.ApplyModifiedProperties();
			Reset();
		}
	}
}

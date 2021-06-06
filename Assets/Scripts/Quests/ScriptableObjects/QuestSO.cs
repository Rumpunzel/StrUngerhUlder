using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 51)]
public class QuestSO : ScriptableObject
{
    public int IdQuest => m_IdQuest;
    public List<StepSO> Steps => m_Steps;
    public bool IsDone => m_IsDone;


	[SerializeField] private int m_IdQuest = 0;

	[Tooltip("The collection of Steps composing the Quest")]
	[SerializeField] private List<StepSO> m_Steps = new List<StepSO>();
	[SerializeField] private bool m_IsDone = false;


	public void FinishQuest() => m_IsDone = true;

	public void SetQuestId(int id) => m_IdQuest = id;
}

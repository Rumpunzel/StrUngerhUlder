using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Questline", menuName = "Quests/Questline", order = 51)]
public class QuestlineSO : ScriptableObject
{
    public int IdQuestline => m_IdQuestLine;
    public List<QuestSO> Quests => m_Quests;
    public bool IsDone => m_IsDone;


	[SerializeField] private int m_IdQuestLine = 0;

	[Tooltip("The collection of Quests composing the Questline")]
	[SerializeField] private List<QuestSO> m_Quests = new List<QuestSO>();
	[SerializeField] private bool m_IsDone = false;
	

	public void FinishQuestline() => m_IsDone = true;
	
	public void SetQuestlineId(int id) => m_IdQuestLine = id;
}

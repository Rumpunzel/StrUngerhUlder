using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] private DialogueManager m_DialogueManager = default;
	[SerializeField] private DialogueDataSO m_DialogueData = default;

	private void OnTriggerEnter(Collider other) => m_DialogueManager.DisplayDialogueData(m_DialogueData);
}

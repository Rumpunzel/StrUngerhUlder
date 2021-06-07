using UnityEngine;
using UnityEngine.Localization;

namespace Strungerhulder.Dialogues.ScriptableObjects
{
    /// <summary>
    /// Scriptable Object that represents an "Actor", that is the protagonist of a Dialogue
    /// </summary>
    [CreateAssetMenu(fileName = "newActor", menuName = "Dialogues/Actor")]
    public class ActorSO : ScriptableObject
    {
        public LocalizedString ActorName { get => m_ActorName; }

        [SerializeField] private LocalizedString m_ActorName = default;
    }
}

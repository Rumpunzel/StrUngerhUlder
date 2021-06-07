using UnityEngine;
using UnityEngine.Localization;
using Strungerhulder.Interaction;

namespace Strungerhulder.UI.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Interaction", menuName = "UI/Interaction", order = 51)]
    public class InteractionSO : ScriptableObject
    {
        [Tooltip("The interaction name")]
        [SerializeField] private LocalizedString m_InteractionName = default;

        [Tooltip("The Interaction Type")]
        [SerializeField] private InteractionType m_InteractionType = default;


        public LocalizedString InteractionName => m_InteractionName;
        public InteractionType InteractionType => m_InteractionType;
    }
}

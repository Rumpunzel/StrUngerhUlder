using UnityEngine;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    /// <summary>
    /// All the data concerning the movement of a character.
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterMovementStats", menuName = "State Machines/Character Movement Stats")]
    public class CharacterMovementStatsSO : ScriptableObject
    {
        [Tooltip("Horizontal XZ plane speed multiplier")]
        public float moveSpeed = 12f;
        public float moveAcceleration = 12f;
        [Range(0f, 1f)] public float walkingModifier = .7f;

        [Space]
        [Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
        public float jumpHeight = 1.5f;
        [Tooltip("Desired horizontal movement speed percentage increase while in the air")]
        public float aerialModifier = 1.1f;
        [Tooltip("The acceleration applied to reach the desired speed")]
        public float aerialAcceleration = 200f;

        [Space]
        public float gravityDescendMultiplier = 4f;
        public float gravityAscendMultiplier = 2f;

        [Space]
        [SerializeField] private float m_MaxFallSpeed = 50f;
        [SerializeField] private float m_MaxRiseSpeed = 100f;

        [Space]
        public float airResistance = 5f;
        public float turnRate = 500f;


        public float ValidateVerticalSpeed(float verticalMovement)
        {
            return Mathf.Clamp(verticalMovement, -m_MaxFallSpeed, m_MaxRiseSpeed);
        }
    }
}

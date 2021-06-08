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
        [SerializeField] private float m_MoveSpeed = 12f;
        [SerializeField] private float m_MoveAcceleration = 4f;
        [SerializeField] private float m_RunningModifier = 1.5f;

        [Space]
        [Tooltip("The initial upwards push when pressing jump. This is injected into verticalMovement, and gradually cancelled by gravity")]
        [SerializeField] private float m_JumpHeight = 1.5f;
        [Tooltip("Desired horizontal movement speed percentage increase while in the air")]
        [SerializeField] private float m_AerialModifier = 1.2f;
        [Tooltip("The acceleration applied to reach the desired speed")]
        [SerializeField] private float m_AerialAcceleration = 200f;

        [Space]
        [SerializeField] private float m_GravityDescendMultiplier = 5f;
        [SerializeField] private float m_GravityAscendMultiplier = 1.2f;

        [Space]
        [SerializeField] private float m_MaxFallSpeed = 50f;
        [SerializeField] private float m_MaxRiseSpeed = 100f;

        [Space]
        [SerializeField] private float m_AirResistance = 5f;
        [SerializeField] private float m_TurnRate = 500f;


        public float CalculateHorizontalSpeed(bool isRunning, bool isAerial = false)
        {
            return (moveSpeed * (isAerial ? m_AerialModifier : 1f)) * (isRunning ? runningModifier : 1f);
        }
        public float CalculateVerticalSpeed(float verticalMovement)
        {
            return Mathf.Clamp(verticalMovement, maxFallSpeed, maxRiseSpeed);
        }

        public float moveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }
        public float moveAcceleration { get { return m_MoveAcceleration; } set { m_MoveAcceleration = value; } }
        public float runningModifier { get { return m_RunningModifier; } set { m_RunningModifier = value; } }


        public float jumpHeight { get { return m_JumpHeight; } set { m_JumpHeight = value; } }
        public float aerialAcceleration { get { return m_AerialAcceleration; } set { m_AerialAcceleration = value; } }

        public float gravityDescendMultiplier { get { return m_GravityDescendMultiplier; } set { m_GravityDescendMultiplier = value; } }
        public float gravityAscendMultiplier { get { return m_GravityAscendMultiplier; } set { m_GravityAscendMultiplier = value; } }

        public float maxFallSpeed { get { return -m_MaxFallSpeed; } set { m_MaxFallSpeed = value; } }
        public float maxRiseSpeed { get { return m_MaxRiseSpeed; } set { m_MaxRiseSpeed = value; } }

        public float airResistance { get { return m_AirResistance; } set { m_AirResistance = value; } }
        public float turnRate { get { return m_TurnRate; } set { m_TurnRate = value; } }
    }
}

using UnityEngine;
using Strungerhulder.Characters;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    /// <summary>
    /// This Action handles horizontal movement while in the air, keeping momentum, simulating air resistance, and accelerating towards the desired speed.
    /// </summary>
    [CreateAssetMenu(fileName = "AerialMovement", menuName = "State Machines/Actions/Aerial Movement")]
    public class AerialMovementActionSO : StateActionSO
    {
        [Tooltip("Desired horizontal movement speed while in the air")]
        [Range(0.1f, 100f)] public float speed = 10f;

        [Tooltip("The acceleration applied to reach the desired speed")]
        [Range(0.1f, 100f)] public float acceleration = 20f;

        protected override StateAction CreateAction() => new AerialMovementAction();
    }

    public class AerialMovementAction : StateAction
    {
        private Protagonist m_Protagonist;
        protected new AerialMovementActionSO OriginSO => (AerialMovementActionSO)base.OriginSO;

        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
        }

        public override void OnUpdate()
        {
            Vector3 velocity = m_Protagonist.movementVector;
            Vector3 input = m_Protagonist.movementInput;

            SetVelocityPerAxis(ref velocity.x, input.x, OriginSO.acceleration, OriginSO.speed);
            SetVelocityPerAxis(ref velocity.z, input.z, OriginSO.acceleration, OriginSO.speed);

            m_Protagonist.movementVector = velocity;
        }

        private void SetVelocityPerAxis(ref float currentAxisSpeed, float axisInput, float acceleration, float targetSpeed)
        {
            if (axisInput == 0f)
            {
                if (currentAxisSpeed != 0f)
                    ApplyAirResistance(ref currentAxisSpeed);
            }
            else
            {
                (float absVel, float absInput) = (Mathf.Abs(currentAxisSpeed), Mathf.Abs(axisInput));
                (float signVel, float signInput) = (Mathf.Sign(currentAxisSpeed), Mathf.Sign(axisInput));
                targetSpeed *= absInput;

                if (signVel != signInput || absVel < targetSpeed)
                {
                    currentAxisSpeed += axisInput * acceleration * Time.deltaTime;
                    currentAxisSpeed = Mathf.Clamp(currentAxisSpeed, -targetSpeed, targetSpeed);
                }
                else
                    ApplyAirResistance(ref currentAxisSpeed);
            }
        }

        private void ApplyAirResistance(ref float value)
        {
            float sign = Mathf.Sign(value);

            value -= sign * Protagonist.AIR_RESISTANCE * Time.deltaTime;

            if (Mathf.Sign(value) != sign)
                value = 0;
        }
    }
}

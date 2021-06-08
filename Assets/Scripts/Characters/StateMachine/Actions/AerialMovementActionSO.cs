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
        protected override StateAction CreateAction() => new AerialMovementAction();
    }

    public class AerialMovementAction : StateAction
    {
        private Protagonist m_Protagonist;
        private CharacterMovementStatsSO m_MovementStats;


        public override void Awake(StateMachine stateMachine)
        {
            m_Protagonist = stateMachine.GetComponent<Protagonist>();
            m_MovementStats = m_Protagonist.movementStats;
        }

        public override void OnUpdate()
        {
            Vector3 velocity = m_Protagonist.movementVector;
            Vector3 input = m_Protagonist.GetAdjustedMovement();
            float moveSpeed = m_MovementStats.CalculateHorizontalSpeed(m_Protagonist.isRunning, true);

            SetVelocityPerAxis(ref velocity.x, input.x, m_MovementStats.aerialAcceleration, moveSpeed);
            SetVelocityPerAxis(ref velocity.z, input.z, m_MovementStats.aerialAcceleration, moveSpeed);

            m_Protagonist.movementVector = velocity;
        }


        private void SetVelocityPerAxis(ref float currentAxisSpeed, float axisInput, float acceleration, float targetSpeed)
        {
            (float absVel, float absInput) = (Mathf.Abs(currentAxisSpeed), Mathf.Abs(axisInput));
            (float signVel, float signInput) = (Mathf.Sign(currentAxisSpeed), Mathf.Sign(axisInput));
            targetSpeed *= absInput;

            if (signVel != signInput || absVel < targetSpeed)
            {
                currentAxisSpeed += axisInput * acceleration * Time.deltaTime;
            }

            //currentAxisSpeed = Mathf.Clamp(currentAxisSpeed, -targetSpeed, targetSpeed);

            if (currentAxisSpeed != 0f)
                ApplyAirResistance(ref currentAxisSpeed);
        }

        private void ApplyAirResistance(ref float value)
        {
            float sign = Mathf.Sign(value);

            value -= sign * m_MovementStats.airResistance * Time.deltaTime;

            if (Mathf.Sign(value) != sign)
                value = 0f;
        }
    }
}

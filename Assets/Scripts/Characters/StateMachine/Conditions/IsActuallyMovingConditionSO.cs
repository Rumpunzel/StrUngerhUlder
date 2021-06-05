using UnityEngine;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsActuallyMovingCondition", menuName = "State Machines/Conditions/Is Actually Moving Condition")]
public class IsActuallyMovingConditionSO : StateConditionSO
{
    [SerializeField] private float m_Threshold = 0.02f;

	protected override Condition CreateCondition() => new IsActuallyMovingCondition(m_Threshold);
}

public class IsActuallyMovingCondition : Condition
{
    private float m_Threshold;
    private CharacterController m_CharacterController;

    public override void Awake(StateMachine stateMachine)
    {
        m_CharacterController = stateMachine.GetComponent<CharacterController>();
    }

    public IsActuallyMovingCondition(float threshold)
    {
        m_Threshold = threshold;
    }

    protected override bool Statement()
    {
        return m_CharacterController.velocity.sqrMagnitude > m_Threshold * m_Threshold;
    }
}

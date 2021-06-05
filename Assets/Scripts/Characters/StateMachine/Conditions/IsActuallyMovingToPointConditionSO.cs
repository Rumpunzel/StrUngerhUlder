using UnityEngine;
using UnityEngine.AI;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "IsActuallyMovingToPointCondition", menuName = "State Machines/Conditions/Is Actually Moving To Point Condition")]
public class IsActuallyMovingToPointConditionSO : StateConditionSO
{
    [SerializeField] private float m_Threshold = 0.02f;

	protected override Condition CreateCondition() => new IsActuallyMovingToPointCondition(m_Threshold);
}

public class IsActuallyMovingToPointCondition : Condition
{
    private NavMeshAgent m_NavAgent;
    private float m_Threshold;

    public override void Awake(StateMachine stateMachine)
    {
        m_NavAgent = stateMachine.GetComponent<NavMeshAgent>();
    }

    public IsActuallyMovingToPointCondition(float threshold)
    {
        m_Threshold = threshold;
    }

    protected override bool Statement()
    {
        return m_NavAgent.velocity.sqrMagnitude > m_Threshold * m_Threshold;
    }
}

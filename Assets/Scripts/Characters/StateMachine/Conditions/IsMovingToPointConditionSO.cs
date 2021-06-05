using UnityEngine;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Started Moving To Point")]
public class IsMovingToPointConditionSO : StateConditionSO<IsMovingToPointCondition>
{
    [Tooltip("Minimal distance to the point to actually move")]
    public float minimumDistance = .02f;
    [Tooltip("Maximal manual speed from which navigation can be enabled")]
    public float overrideSpeedThreshold = .2f;
}

public class IsMovingToPointCondition : Condition
{
    private Protagonist m_ProtagonistScript;
    private new IsMovingToPointConditionSO m_OriginSO => (IsMovingToPointConditionSO)base.OriginSO; // The SO this Condition spawned from

    public override void Awake(StateMachine stateMachine)
    {
        m_ProtagonistScript = stateMachine.GetComponent<Protagonist>();
    }

    protected override bool Statement()
    {
        if (m_ProtagonistScript.movementInput.magnitude > m_OriginSO.overrideSpeedThreshold)
            return false;
        
        Vector3 destination = m_ProtagonistScript.destinationInput;
        Vector3 distance = destination - m_ProtagonistScript.transform.position;
        
        return distance.sqrMagnitude > m_OriginSO.minimumDistance;
    }
}

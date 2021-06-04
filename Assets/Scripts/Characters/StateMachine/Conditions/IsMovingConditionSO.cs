using UnityEngine;
using Strungerhulder.StateMachine;
using Strungerhulder.StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Started Moving")]
public class IsMovingConditionSO : StateConditionSO<IsMovingCondition>
{
    public float treshold = 0.02f;
}

public class IsMovingCondition : Condition
{
    private Protagonist m_ProtagonistScript;
    private IsMovingConditionSO m_OriginSO => (IsMovingConditionSO)base.OriginSO; // The SO this Condition spawned from

    public override void Awake(StateMachine stateMachine)
    {
        m_ProtagonistScript = stateMachine.GetComponent<Protagonist>();
    }

    protected override bool Statement()
    {
        Vector3 movementVector = m_ProtagonistScript.movementInput;
        movementVector.y = 0f;
        return movementVector.sqrMagnitude > m_OriginSO.treshold;
    }
}

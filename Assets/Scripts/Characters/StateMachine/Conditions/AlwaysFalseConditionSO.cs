using UnityEngine;
using Strungerhulder.StateMachines;
using Strungerhulder.StateMachines.ScriptableObjects;

namespace Strungerhulder.Charaters.StateMachines.ScriptableObjects
{
    [CreateAssetMenu(menuName = "State Machines/Conditions/Always False")]
    public class AlwaysFalseConditionSO : StateConditionSO<AlwaysFalseCondition> { }

    public class AlwaysFalseCondition : Condition
    {
        protected override bool Statement() => false;
    }
}

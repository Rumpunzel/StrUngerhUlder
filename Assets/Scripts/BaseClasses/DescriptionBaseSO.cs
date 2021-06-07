using UnityEngine;
using Strungerhulder.SavingAndLoading;

/// <summary>
/// Base class for ScriptableObjects that need a public description field.
/// </summary>
public class DescriptionBaseSO : SerializableScriptableObject
{
    [TextArea] public string description;
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class NPCMovementConfigSO : ScriptableObject
{
    [Tooltip("Waypoint stop duration")]
    [SerializeField] private float m_StopDuration;

    [Tooltip("Roaming speed")]
    [SerializeField] private float m_Speed;

    public float Speed => m_Speed;
    public float StopDuration => m_StopDuration;
}

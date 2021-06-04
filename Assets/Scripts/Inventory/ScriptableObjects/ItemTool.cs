using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

// Created with collaboration from:
// https://forum.unity.com/threads/inventory-system.980646/
[CreateAssetMenu(fileName = "Tool", menuName = "Inventory/Tool")]
public class ItemTool : Item
{
    [Tooltip("The tag of the objects this tool is used on")]
    [SerializeField] private string m_UsedOn = default;

    [Tooltip("The amount of damage this tool does when striking")]
    [SerializeField] private float m_Damage = 1f;


    public string UsedOn => m_UsedOn;
    public float Damage => m_Damage;
}

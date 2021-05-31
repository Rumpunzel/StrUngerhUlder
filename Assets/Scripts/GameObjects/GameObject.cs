using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObject : MonoBehaviour
{
    [SerializeField] protected float m_MaxHitPoints = 10f;
    [SerializeField] protected bool m_Indestructible = false;

    protected Rigidbody2D m_Rigidbody2D;
    protected float m_HitPoints;


    protected virtual void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_HitPoints = m_MaxHitPoints;
    }
}

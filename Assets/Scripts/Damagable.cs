using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    [SerializeField] private float m_StartingHealth = 10f;
    [SerializeField] private bool m_Indestructible = false;

    public UnityEvent OnDestroy = new UnityEvent();
    public UnityEvent OnHit = new UnityEvent();

    private float m_CurrentHealth;

    
    private void Start()
    {
        m_CurrentHealth = m_StartingHealth;
    }


    public void Hit(float damage)
    {
        OnHit.Invoke();

        if (m_Indestructible) return;
        
        m_CurrentHealth -= damage;

        if (m_CurrentHealth <= 0)
            Destroy();
    }

    private void Destroy()
    {
        OnDestroy.Invoke();
        Destroy(gameObject);
    }
}

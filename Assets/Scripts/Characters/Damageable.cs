using UnityEngine;
using UnityEngine.Events;
using Strungerhulder.Characters.ScriptableObjects;

namespace Strungerhulder.Characters
{
    public class Damageable : MonoBehaviour
    {
        public bool GetHit { get; set; }
        public bool IsDead { get; set; }

        public UnityAction OnDie;

        [SerializeField] private HealthConfigSO m_HealthConfigSO;
        [SerializeField] private GetHitEffectConfigSO m_GetHitEffectSO;
        [SerializeField] private Renderer m_MainMeshRenderer;
        [SerializeField] private DroppableRewardConfigSO m_DroppableRewardSO;

        private int m_CurrentHealth = default;


        private void Awake()
        {
            m_CurrentHealth = m_HealthConfigSO.MaxHealth;
        }


        public void ReceiveAnAttack(int damage)
        {
            m_CurrentHealth -= damage;
            GetHit = true;

            if (m_CurrentHealth <= 0)
            {
                IsDead = true;

                if (OnDie != null)
                    OnDie.Invoke();
            }
        }

        public DroppableRewardConfigSO DroppableRewardConfig => m_DroppableRewardSO;
        public GetHitEffectConfigSO GetHitEffectConfig => m_GetHitEffectSO;
        public Renderer MainMeshRenderer => m_MainMeshRenderer;

        public int CurrentHealth => m_CurrentHealth;
    }
}

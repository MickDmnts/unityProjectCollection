using System;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [Header("Set in inspector")]
        [SerializeField] private float _healthAmount;
        public float HealthAmount { get => _healthAmount; set => _healthAmount = value; }

        [Header("Set dynamically")]
        [SerializeField] Animator gameobjectAnimator;
        [SerializeField] bool isDead = false;
        public bool IsDead { get => isDead; set => isDead = value; }

        private void Start()
        {
            AssignReferences();
        }

        void AssignReferences()
        {
            gameobjectAnimator = GetComponent<Animator>();
        }

        public void TakeDamage(float damageAmount)
        {
            HealthAmount = Mathf.Max(HealthAmount - damageAmount, 0);

            if (IsHealthZero())
            {
                DeathBehaviour();
            }
        }

        bool IsHealthZero()
        {
            return HealthAmount <= 0 ? true : false; 
        }

        private void DeathBehaviour()
        {
            if (isDead) return;

            isDead = true;
            DeathAnimationTrigger();
        }

        void DeathAnimationTrigger()
        {
            if (gameobjectAnimator == null) return;

            gameobjectAnimator.SetTrigger("die");
        }
    }
}

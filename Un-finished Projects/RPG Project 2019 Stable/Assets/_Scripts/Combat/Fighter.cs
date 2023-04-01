using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Animation;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [Header("Set in Inspector")]
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 3f;
        [SerializeField] float timeBetweenAttacks = 1f;

        [Header("Set on Gameplay")]
        [SerializeField] Health targetHealth;

        [Header("Set Dynamically")]
        [SerializeField] Mover moverScriptRef;
        [SerializeField] ActionScheduler actionSchedulerRef;
        [SerializeField] PlayerAnimationController playerAnimationController;
        [SerializeField] float timeSinceLastAttack;

        private void Awake()
        {
            moverScriptRef = GetComponent<Mover>();
            actionSchedulerRef = GetComponent<ActionScheduler>();
            playerAnimationController = GetComponent<PlayerAnimationController>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (targetHealth == null) return;
            if (targetHealth.IsDead) return;

            if (!CheckDistanceToTarget())
            {
                moverScriptRef.MoveTo(targetHealth.transform.position);
            }
            else
            {
                AttackBehaviour();
                moverScriptRef.Cancel();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                playerAnimationController.StartAttackAnimation();
                timeSinceLastAttack = 0;
            }
        }

        void Hit() //Gets called by animation Event
        {
            if (!IsTargetNull())
            {
                targetHealth.TakeDamage(weaponDamage);
            }
        }

        private bool IsTargetNull()
        {
            return targetHealth == null ? true : false;
        }

        private bool CheckDistanceToTarget()
        {
            return Vector3.Distance(transform.position, targetHealth.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionSchedulerRef.StartAction(this);
            targetHealth = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            CancelAttacking();
        }

        void CancelAttacking()
        {
            playerAnimationController.StopAttackAnimation();
            targetHealth = null;
        }
    }
}

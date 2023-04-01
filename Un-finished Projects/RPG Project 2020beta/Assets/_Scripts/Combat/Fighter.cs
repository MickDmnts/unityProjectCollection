using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [Header("Set in Inspector")]
        [SerializeField] float weaponRange = 2f;

        [Header("Set Dynamically")]
        [SerializeField] Mover moverScriptRef;
        [SerializeField] Transform target;

        private void Awake()
        {
            moverScriptRef = GetComponent<Mover>();
        }

        private void Update()
        {
            if (target == null) return;

            if (!CheckDistanceToTarget())
            {
                moverScriptRef.MoveTo(target.position);
            }
            else
            {
                moverScriptRef.StopMoving();
            }
        }

        private bool CheckDistanceToTarget()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void CancelAttacking()
        {
            target = null;
        }
    }
}

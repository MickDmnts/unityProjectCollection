using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Set dynamically")]
        [SerializeField] Mover moverScript;
        [SerializeField] Fighter fighterScript;

        private void Awake()
        {
            AssingReferences();
        }

        private void AssingReferences()
        {
            moverScript = GetComponent<Mover>();
            fighterScript = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMousePosRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.collider.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (Input.GetMouseButtonDown(1))
                {
                    fighterScript.Attack(target);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            Ray ray = GetMousePosRay();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    moverScript.StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMousePosRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGR.Core;

namespace WGR.Gameplay
{
    public class BulletDetection : MonoBehaviour
    {
        [SerializeField] float radius;
        [SerializeField] LayerMask bulletLayer;

        IInteractable parentInteraction;

        bool isActive = true;

        private void Awake()
        {
            parentInteraction = GetComponentInParent<IInteractable>();
        }

        private void Update()
        {
            if (!isActive) return;

            Collider[] overlaps = new Collider[20];
            int collCount = Physics.OverlapSphereNonAlloc(transform.position, radius, overlaps, bulletLayer);

            foreach (Collider collision in overlaps)
            {
                if (collision == null) continue;

                if (parentInteraction != null)
                    parentInteraction.AttackInteraction();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
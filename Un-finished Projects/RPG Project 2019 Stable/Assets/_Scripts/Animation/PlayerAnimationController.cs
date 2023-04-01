using UnityEngine;
using UnityEngine.AI;

namespace RPG.Animation
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("Set dynamically")]
        [SerializeField] NavMeshAgent playerNavMesh;
        [SerializeField] Animator playerAnimator;

        private void Awake()
        {
            AssignReferences();
        }

        private void AssignReferences()
        {
            playerNavMesh = GetComponent<NavMeshAgent>();
            playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (playerAnimator && playerNavMesh != null)
            {
                UpdateAnimator();
            }
        }

        private void UpdateAnimator()
        {
            Vector3 globalAnimationVelocity = playerNavMesh.velocity;
            Vector3 localAnimationVelocity = transform.InverseTransformDirection(globalAnimationVelocity); //Converts from global to local
            float zSpeed = localAnimationVelocity.z; // We take only the Z axis because that corresponds with our animation
            playerAnimator.SetFloat("forwardSpeed", zSpeed); // We directly set the speed of the blended animation
        }

        public void StartAttackAnimation()
        {
            playerAnimator.SetTrigger("attack");
        }

        public void StopAttackAnimation()
        {
            playerAnimator.SetTrigger("stopAttacking");
        }
    }
}
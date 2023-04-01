using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [Header("Set dynamically")]
        [SerializeField] GameObject playerGO;

        private void Awake()
        {
            AssignReferences();
        }

        void AssignReferences()
        {
            playerGO = GameObject.FindGameObjectWithTag("Player");
        }

        private void LateUpdate()
        {
            MoveCamera();
        }

        void MoveCamera()
        {
            transform.position = playerGO.transform.position;
        }
    }
}

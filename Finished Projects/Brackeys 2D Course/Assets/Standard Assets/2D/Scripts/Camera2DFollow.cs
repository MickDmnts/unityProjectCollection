using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;
        public float cameraVerticalClampimngFactor = -1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;
        float nextTimeToSearch = 0;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }


        // Update is called once per frame
        private void Update()
        {
            if (TargetIsAbsence(target))
            {
                SearchForThePlayer();
                return;
            }

            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            newPos = ClampVerticalCameraPosition(newPos);

            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }

        //=====================My functions ===================== 
        Vector3 ClampVerticalCameraPosition(Vector3 cameraPosition)
        {
            Vector3 clampedCameraPos = new Vector3(cameraPosition.x, Mathf.Clamp(cameraPosition.y, cameraVerticalClampimngFactor, Mathf.Infinity), cameraPosition.z);
            return clampedCameraPos;
        }

        bool TargetIsAbsence(Transform target)
        {
            if (target == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void SearchForThePlayer()
        {
            if (nextTimeToSearch <= Time.time)
            {
                GameObject newTarget = GameObject.FindGameObjectWithTag("Player");
                if (newTarget != null)
                {
                    target = newTarget.transform;
                }
                nextTimeToSearch = Time.time + .5f;
            }
        }
    }
}

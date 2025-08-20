using UnityEngine;

namespace AnimarsCatcher
{
    public class BLASTER_Ani : MonoBehaviour
    {
        public Transform LeftHandIKTrans;
        public Transform RightHandIKTrans;

        private Animator m_Animator;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(1).IsName("Shoot"))
            {
                // 在射击动画中启用IK

                // 左手IK
                m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKTrans.position);
                m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.5f);
                m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIKTrans.rotation);
                m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.5f);

                // 右手IK
                m_Animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandIKTrans.position);
                m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.5f);
                m_Animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandIKTrans.rotation);
                m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0.5f);
            }
            }
    }
}
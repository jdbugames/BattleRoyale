using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class IkHandlerController : MonoBehaviour
    {
        public Transform tr_LookAtPosition;

        public Transform tr_LeftHandPosition;
        public Transform tr_LeftElbowPosition;
        public Transform tr_RightHandPosition;
        public Transform tr_RightElbowPosition;

        public float fl_BodyWeight = 0.5f;

        public float fl_TimeMod = .02f;
        public Vector3 vec_RecoilTarget;
        public float fl_Time;

        public bool bl_Aiming;
        public bool bl_Shooting;

        Animator an_Anim;

        // Use this for initialization
        void Start()
        {
            an_Anim = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            fl_Time += Time.deltaTime * fl_TimeMod;
        }

        public void UpdateRecoil(float fl_MaxRecoil, float fl_ShootingModifier, float fl_LeteralMod)
        {
            float fl_Temp = Mathf.Cos(fl_Time) * fl_MaxRecoil;
            vec_RecoilTarget = new Vector3(0, fl_Temp * (!bl_Shooting ? fl_ShootingModifier : 1f));
        }

        private void OnAnimatorIK(int layerIndex)
        {
            float fl_Aim = (bl_Aiming ? 1f : 0);

            an_Anim.SetLayerWeight(an_Anim.GetLayerIndex("UpperBody"), fl_Aim);

            an_Anim.SetLookAtWeight(1, fl_BodyWeight, 1, 1, (bl_Aiming ? 0 : 1));
            an_Anim.SetLookAtPosition(tr_LookAtPosition.position);

            SetIk(AvatarIKGoal.RightHand, tr_RightHandPosition, AvatarIKHint.RightElbow, tr_RightElbowPosition, fl_Aim, vec_RecoilTarget);

            if(tr_LeftHandPosition != null)
            {
                SetIk(AvatarIKGoal.LeftHand, tr_LeftHandPosition, AvatarIKHint.LeftElbow, tr_LeftElbowPosition, fl_Aim, Vector3.zero);
            }

        }

        private void SetIk(AvatarIKGoal aik_Goal, Transform tr_Target, AvatarIKHint aik_Hint, Transform tr_Restraint, float fl_weight, Vector3 vec_Recoil)
        {
            an_Anim.SetIKHintPositionWeight(aik_Hint, fl_weight);
            an_Anim.SetIKPositionWeight(aik_Goal, fl_weight);
            an_Anim.SetIKRotationWeight(aik_Goal, fl_weight);
            an_Anim.SetIKPosition(aik_Goal, tr_Target.position);
            an_Anim.SetIKRotation(aik_Goal, tr_Target.rotation);
            an_Anim.SetIKHintPosition(aik_Hint, tr_Restraint.position);
        }
    }
}

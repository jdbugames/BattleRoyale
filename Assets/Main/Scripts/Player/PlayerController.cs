using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class PlayerController : MonoBehaviour
    {
        private Transform tr_Transform;
        private Rigidbody rg_Rigidbody;
        private Animator an_Animator;
        CapsuleCollider col_Collider;
        InventaryController ic_Inventary;
        IkHandlerController ihc_IkHandlerController;

        public Transform tr_CameraShoulder;
        public Transform tr_CameraHolder;
        public Transform tr_LookAt;
        private Transform tr_Cam;

        private float fl_RotY = 0f;
        private float fl_RotX = 0f;

        public Transform tr_HandsPivot;
        public Transform tr_RightHand;
        public Transform tr_RightElbow;

        public CharStatsController csc_Stats;

        private States st_States = new States();

        private Vector2 vec_MoveDelta;
        private Vector2 vec_MouseDelta;
        private Vector2 vec_MoveAnim;
        private float fl_DeltaT;

        public InputController _Input;

        // Use this for initialization
        void Start()
        {
            tr_Transform = this.transform;
            rg_Rigidbody = this.GetComponent<Rigidbody>();
            col_Collider = GetComponent<CapsuleCollider>();
            an_Animator = this.GetComponentInChildren<Animator>();
            ihc_IkHandlerController = GetComponentInChildren<IkHandlerController>();
            ic_Inventary = GetComponent<InventaryController>();

            ihc_IkHandlerController.tr_LookAtPosition = tr_LookAt;
            ihc_IkHandlerController.tr_RightHandPosition = tr_RightHand;
            ihc_IkHandlerController.tr_RightElbowPosition = tr_RightElbow;
            tr_Cam = Camera.main.transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            PlayerControl();
            MoveController();
            ItemsControl();
            CameraControl();
            AnimControl();
        }

        //Collect Player Information
        private void PlayerControl()
        {
            _Input.Update();

            float fl_DeltaX = _Input.fl_Check("Horizontal");
            float fl_DeltaZ = _Input.fl_Check("Vertical");
            float fl_MouseX = _Input.fl_Check("Mouse X");
            float fl_MouseY = _Input.fl_Check("Mouse Y");
            st_States.bl_Jumping = _Input.bl_Check("Jump");
            st_States.bl_Crouching = _Input.bl_Check("Crouch");
            st_States.bl_Running = _Input.bl_Check("Run");
            st_States.bl_Interacting = _Input.bl_Check("Interact");
            st_States.bl_Aiming = _Input.bl_Check("Fire2") && !st_States.bl_Running;
            st_States.bl_Shooting = _Input.bl_Check("Fire1") && !st_States.bl_Running;

            vec_MoveDelta = new Vector2(fl_DeltaX, fl_DeltaZ);
            vec_MouseDelta = new Vector2(fl_MouseX, fl_MouseY);
            fl_DeltaT = Time.deltaTime;
        }

        //Move the character
        private void MoveController()
        {
            Vector3 vec_Side = csc_Stats.fl_Speed * vec_MoveDelta.x * fl_DeltaT * tr_Transform.right;
            Vector3 vec_Forward = csc_Stats.fl_Speed * vec_MoveDelta.y * fl_DeltaT * tr_Transform.forward;

            Vector3 vec_EndSpeed = vec_Side + vec_Forward;

            RaycastHit hit;
            st_States.bl_OnGround = Physics.Raycast(this.tr_Transform.position, -tr_Transform.up, out hit, .2f);
            if(st_States.bl_OnGround)
            {
                if(st_States.bl_Crouching)
                {
                    OnCrouch();
                }
                else
                {
                    if(st_States.bl_Running)
                    {
                        vec_EndSpeed *= csc_Stats.fl_RunningSpeedIncrement;
                    }
                }

                if(st_States.bl_Jumping)
                {
                    if (st_States.bl_Crouch)
                    {
                        OnCrouch();
                    }
                    else
                    {
                        Jump();
                    }
                }

                Vector3 vec_Speed = rg_Rigidbody.velocity;
                vec_EndSpeed.y = vec_Speed.y;

                rg_Rigidbody.velocity = vec_EndSpeed;
            }
            else
            {
                if(st_States.bl_Crouch)
                {
                    OnCrouch();
                }
            }

            vec_MoveAnim = vec_MoveDelta * (st_States.bl_Running ? 2 : 1);
        }

        private void ItemsControl()
        {
            Collider[] col_Checking = Physics.OverlapSphere(tr_LookAt.position, 2f, LayerMask.GetMask("Item"));

            if(col_Checking.Length > 0)
            {
                float fl_Near = 2f;
                Collider col_Nearest = null;

                foreach (Collider col_c in col_Checking)
                {
                    Vector3 vec_CollisionPos = col_c.ClosestPoint(tr_LookAt.position);
                    float fl_Distance = Vector3.Distance(vec_CollisionPos, tr_LookAt.position);
                    if (fl_Distance < fl_Near)
                    {
                        col_Nearest = col_c;
                        fl_Near = fl_Distance;
                    }
                }

                if (col_Nearest != null)
                {
                    ItemController ic_Item = col_Nearest.GetComponent<ItemController>();
                    if (ic_Item != null)
                    {
                        ic_Inventary.ivc_ItemViewer.DrawItemViewer(ic_Item.tr_MTransform, tr_Cam);
                        if (st_States.bl_Interacting)
                        {
                            ic_Inventary.Bl_AddItem(ic_Item);
                        }
                    }
                }
            }
            else
            {
                ic_Inventary.ivc_ItemViewer.HideViewer();
            }
            

            //if(gc_GunController != null)
            //{
            //    ihc_IkHandlerController.tr_LeftHandPosition = gc_GunController.tr_LeftHandPosition;
            //    ihc_IkHandlerController.tr_LeftElbowPosition = gc_GunController.tr_LeftElbowPosition;

            //    gc_GunController.DrawCrossHair(tr_Cam);

            //    if(st_States.bl_Shooting)
            //    {
            //        gc_GunController.Shoot();
            //    }

            //    ihc_IkHandlerController.UpdateRecoil(gc_GunController.fl_MaxRecoil, -vec_MoveAnim.x, gc_GunController.fl_ShootingModifier);

            //    Cursor.lockState = (Input.GetKey(KeyCode.Escape) ? CursorLockMode.None : CursorLockMode.Locked);
            //}

        }

        public void Jump()
        {
            rg_Rigidbody.AddForce(tr_Transform.up * csc_Stats.fl_JumpForce);
        }

        public void OnCrouch()
        {
            st_States.bl_Crouch = !st_States.bl_Crouch;
            st_States.bl_Crouching = false;
            float fl_Mult = (st_States.bl_Crouch ? 1 : -1);
            col_Collider.center = col_Collider.center + new Vector3(0, csc_Stats.fl_CrouchPosOffSet, 0) * fl_Mult;
            col_Collider.height += csc_Stats.fl_CrouchHeightOffSet * fl_Mult;
            tr_CameraShoulder.position = tr_CameraShoulder.position + new Vector3(0, csc_Stats.fl_CrouchPosOffSet, 0) * fl_Mult;
        }

        //Move the camera
        private void CameraControl()
        {
            fl_RotY += vec_MouseDelta.y * fl_DeltaT * csc_Stats.fl_RotationSpeed;
            float fl_XRot = vec_MouseDelta.x * fl_DeltaT * csc_Stats.fl_RotationSpeed;

            tr_Transform.Rotate(0, fl_XRot, 0);

            fl_RotY = Mathf.Clamp(fl_RotY, csc_Stats.fl_MinAngle, csc_Stats.fl_MaxAngle);

            Quaternion qt_LocalRotation = Quaternion.Euler(-fl_RotY, 0, 0);
            tr_CameraShoulder.localRotation = qt_LocalRotation;

            tr_Cam.position = Vector3.Lerp(tr_Cam.position, tr_CameraHolder.position, csc_Stats.fl_CameraSpeed * fl_DeltaT);
            tr_Cam.rotation = Quaternion.Lerp(tr_Cam.rotation, tr_CameraHolder.rotation, csc_Stats.fl_CameraSpeed * fl_DeltaT);
        }

        //Animate the character
        private void AnimControl()
        {
            tr_HandsPivot.position = an_Animator.GetBoneTransform(HumanBodyBones.RightShoulder).position;
            Quaternion qt_LocalRotation = Quaternion.Euler(-fl_RotY, tr_HandsPivot.localRotation.y, tr_HandsPivot.localRotation.z);
            tr_HandsPivot.localRotation = qt_LocalRotation;

            an_Animator.SetBool("Ground", st_States.bl_OnGround);
            an_Animator.SetBool("Crouch", st_States.bl_Crouch);

            an_Animator.SetFloat("X", vec_MoveAnim.x);
            an_Animator.SetFloat("Y", vec_MoveAnim.y);

            ihc_IkHandlerController.bl_Aiming = this.st_States.bl_Aiming;
            ihc_IkHandlerController.bl_Shooting = this.st_States.bl_Shooting;
        }
    }


    public class States
    {
        public bool bl_OnGround = false;
        public bool bl_Jumping = false;
        public bool bl_Crouch = false;
        public bool bl_Crouching = false;
        public bool bl_Running = false;
        public bool bl_Aiming = false;
        public bool bl_Shooting = false;
        public bool bl_Interacting = false;
    }
}


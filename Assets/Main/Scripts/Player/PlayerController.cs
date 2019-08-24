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
        IkHandlerController _IkHandlerController;

        public Transform tr_CameraShoulder;
        public Transform tr_CameraHolder;
        public Transform tr_LookAt;
        private Transform tr_Cam;

        private float fl_RotY = 0f;
        private float fl_RotX = 0f;

        public Transform tr_HandsPivot;
        public Transform tr_RightHand;
        public Transform tr_RightElbow;

        public CharStatsController _Stats;

        public bool bl_OnGround = false;
        public bool bl_Jumping = false;
        public bool bl_Crouch = false;
        public bool bl_Crouching = false;
        public bool bl_Running = false;
        public bool bl_Aiming = false;
        public bool bl_Shooting = false;

        public GunController _GunController;

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
            _IkHandlerController = GetComponentInChildren<IkHandlerController>();
            _IkHandlerController.tr_LookAtPosition = tr_LookAt;
            _IkHandlerController.tr_RightHandPosition = tr_RightHand;
            _IkHandlerController.tr_RightElbowPosition = tr_RightElbow;
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
            bl_Jumping = _Input.bl_Check("Jump");
            bl_Crouching = _Input.bl_Check("Crouch");
            bl_Running = _Input.bl_Check("Run");
            bl_Aiming = _Input.bl_Check("Fire2") && !bl_Running;
            bl_Shooting = _Input.bl_Check("Fire1");

            vec_MoveDelta = new Vector2(fl_DeltaX, fl_DeltaZ);
            vec_MouseDelta = new Vector2(fl_MouseX, fl_MouseY);
            fl_DeltaT = Time.deltaTime;
        }

        //Move the character
        private void MoveController()
        {
            Vector3 vec_Side = _Stats.fl_Speed * vec_MoveDelta.x * fl_DeltaT * tr_Transform.right;
            Vector3 vec_Forward = _Stats.fl_Speed * vec_MoveDelta.y * fl_DeltaT * tr_Transform.forward;

            Vector3 vec_EndSpeed = vec_Side + vec_Forward;

            RaycastHit hit;
            bl_OnGround = Physics.Raycast(this.tr_Transform.position, -tr_Transform.up, out hit, .2f);
            if(bl_OnGround)
            {
                if(bl_Crouching)
                {
                    OnCrouch();
                }
                else
                {
                    if(bl_Running)
                    {
                        vec_EndSpeed *= _Stats.fl_RunningSpeedIncrement;
                    }
                }

                if(bl_Jumping)
                {
                    if (bl_Crouch)
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
                if(bl_Crouch)
                {
                    OnCrouch();
                }
            }

            vec_MoveAnim = vec_MoveDelta * (bl_Running ? 2 : 1);
        }

        private void ItemsControl()
        {
            if(_GunController != null)
            {
                _IkHandlerController.tr_LeftHandPosition = _GunController.tr_LeftHandPosition;
                _IkHandlerController.tr_LeftElbowPosition = _GunController.tr_LeftElbowPosition;

                if(bl_Shooting)
                {
                    _GunController.Shoot();
                }
                _IkHandlerController.UpdateRecoil(_GunController.fl_MaxRecoil, -vec_MoveAnim.x, _GunController.fl_ShootingModifier);
            }

            tr_HandsPivot.position = an_Animator.GetBoneTransform(HumanBodyBones.RightShoulder).position;
            Quaternion qt_LocalRotation = Quaternion.Euler(-fl_RotY, tr_HandsPivot.localRotation.y, tr_HandsPivot.localRotation.z);
            tr_HandsPivot.localRotation = qt_LocalRotation;
        }

        public void Jump()
        {
            rg_Rigidbody.AddForce(tr_Transform.up * _Stats.fl_JumpForce);
        }

        public void OnCrouch()
        {
            bl_Crouch = !bl_Crouch;
            bl_Crouching = false;
            float fl_Mult = (bl_Crouch ? 1 : -1);
            col_Collider.center = col_Collider.center + new Vector3(0, _Stats.fl_CrouchPosOffSet, 0) * fl_Mult;
            col_Collider.height += _Stats.fl_CrouchHeightOffSet * fl_Mult;
            tr_CameraShoulder.position = tr_CameraShoulder.position + new Vector3(0, _Stats.fl_CrouchPosOffSet, 0) * fl_Mult;
        }

        //Move the camera
        private void CameraControl()
        {
            fl_RotY += vec_MouseDelta.y * fl_DeltaT * _Stats.fl_RotationSpeed;
            float fl_XRot = vec_MouseDelta.x * fl_DeltaT * _Stats.fl_RotationSpeed;

            tr_Transform.Rotate(0, fl_XRot, 0);

            fl_RotY = Mathf.Clamp(fl_RotY, _Stats.fl_MinAngle, _Stats.fl_MaxAngle);

            Quaternion qt_LocalRotation = Quaternion.Euler(-fl_RotY, 0, 0);
            tr_CameraShoulder.localRotation = qt_LocalRotation;

            tr_Cam.position = Vector3.Lerp(tr_Cam.position, tr_CameraHolder.position, _Stats.fl_CameraSpeed * fl_DeltaT);
            tr_Cam.rotation = Quaternion.Lerp(tr_Cam.rotation, tr_CameraHolder.rotation, _Stats.fl_CameraSpeed * fl_DeltaT);
        }

        //Animate the character
        private void AnimControl()
        {
            an_Animator.SetBool("Ground", bl_OnGround);
            an_Animator.SetBool("Crouch", bl_Crouch);

            an_Animator.SetFloat("X", vec_MoveAnim.x);
            an_Animator.SetFloat("Y", vec_MoveAnim.y);

            _IkHandlerController.bl_Aiming = this.bl_Aiming;
            _IkHandlerController.bl_Shooting = this.bl_Shooting;
        }
    }
}


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
        private Animator anim_Animator;
        CapsuleCollider col_Collider;

        public Transform tr_CameraShoulder;
        public Transform tr_CameraHolder;
        private Transform tr_Cam;

        private float fl_RotY = 0f;
        private float fl_RotX = 0f;

        public CharStatsController _Stats;

        public bool bl_OnGround = false;
        public bool bl_Jumping = false;
        public bool bl_Crouch = false;
        public bool bl_Crouching = false;

        private Vector2 vec_MoveDelta;
        private Vector2 vec_MouseDelta;
        private float fl_DeltaT;

        public InputController _Input;

        // Use this for initialization
        void Start()
        {
            tr_Transform = this.transform;
            rg_Rigidbody = this.GetComponent<Rigidbody>();
            col_Collider = GetComponent<CapsuleCollider>();
            anim_Animator = this.GetComponentInChildren<Animator>();
            tr_Cam = Camera.main.transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            PlayerControl();
            MoveController();
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
            anim_Animator.SetBool("Ground", bl_OnGround);
            anim_Animator.SetBool("Crouch", bl_Crouch);
            anim_Animator.SetFloat("X", vec_MoveDelta.x);
            anim_Animator.SetFloat("Y", vec_MoveDelta.y);
        }
    }
}


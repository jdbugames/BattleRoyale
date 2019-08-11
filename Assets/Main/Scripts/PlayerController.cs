using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tutorial
{
    public class PlayerController : MonoBehaviour
    {
        private Transform tr_Transform;
        private Rigidbody rg_Rigidbody;
        private Animator anim_Animator;

        public Transform tr_CameraShoulder;
        public Transform tr_CameraHolder;
        private Transform tr_Cam;

        private float fl_RotY = 0f;
        private float fl_RotX = 0f;

        public float fl_Speed = 6;
        public float fl_RotationSpeed = 25;
        public float fl_MinAngle = -70;
        public float fl_MaxAngle = 90;
        public float fl_CameraSpeed = 24;

        private Vector2 vec_NewSpeed;

        // Use this for initialization
        void Start()
        {
            tr_Transform = this.transform;
            rg_Rigidbody = this.GetComponent<Rigidbody>();
            anim_Animator = this.GetComponentInChildren<Animator>();
            tr_Cam = Camera.main.transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            PlayerControl();
            CameraControl();
            AnimControl();
        }

        private void PlayerControl()
        {
            Vector3 vec_Speed = rg_Rigidbody.velocity;

            float fl_DeltaX = Input.GetAxis("Horizontal");
            float fl_DeltaZ = Input.GetAxis("Vertical");
            vec_NewSpeed = new Vector2(fl_DeltaX, fl_DeltaZ);
            float fl_DeltaT = Time.deltaTime;

            Vector3 vec_Side = fl_Speed * fl_DeltaX * fl_DeltaT * tr_Transform.right;
            Vector3 vec_Forward = fl_Speed * fl_DeltaZ * fl_DeltaT * tr_Transform.forward;

            Vector3 vec_EndSpeed = vec_Side + vec_Forward;

            rg_Rigidbody.velocity = vec_EndSpeed;
        }

        private void CameraControl()
        {
            float fl_MouseX = Input.GetAxis("Mouse X");
            float fl_MouseY = Input.GetAxis("Mouse Y");
            float fl_DeltaT = Time.deltaTime;

            fl_RotY += fl_MouseY * fl_DeltaT * fl_RotationSpeed;
            float fl_XRot = fl_MouseX * fl_DeltaT * fl_RotationSpeed;

            tr_Transform.Rotate(0, fl_XRot, 0);

            fl_RotY = Mathf.Clamp(fl_RotY, fl_MinAngle, fl_MaxAngle);

            Quaternion qt_LocalRotation = Quaternion.Euler(-fl_RotY, 0, 0);
            tr_CameraShoulder.localRotation = qt_LocalRotation;

            tr_Cam.position = Vector3.Lerp(tr_Cam.position, tr_CameraHolder.position, fl_CameraSpeed * fl_DeltaT);
            tr_Cam.rotation = Quaternion.Lerp(tr_Cam.rotation, tr_CameraHolder.rotation, fl_CameraSpeed * fl_DeltaT);
        }

        private void AnimControl()
        {
            anim_Animator.SetFloat("X", vec_NewSpeed.x);
            anim_Animator.SetFloat("Y", vec_NewSpeed.y);
        }
    }
}


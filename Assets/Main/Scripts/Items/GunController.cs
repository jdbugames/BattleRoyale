using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class GunController : MonoBehaviour
    {
        private Transform tr_CrossHair;

        public Transform tr_ShootPoint;
        public Transform tr_LeftHandPosition;
        public Transform tr_LeftElbowPosition;

        public Transform tr_BulletPrefab;

        public float fl_Range = 10f;
        public float fl_MaxRecoil = 1f;
        public float fl_ShootingModifier = 2f;

        private bool bl_ShowCrossHair = false;

        private void Start()
        {
            tr_CrossHair = GetComponentInChildren<Canvas>().transform;
            tr_CrossHair.gameObject.SetActive(false);
        }

        public void Shoot()
        {
            Instantiate(tr_BulletPrefab, tr_ShootPoint.position, tr_ShootPoint.rotation);
        }

        public void ShowCrossHair()
        {
            tr_CrossHair.gameObject.SetActive(bl_ShowCrossHair = !bl_ShowCrossHair);
        }

        public void DrawCrossHair(Transform tr_Camera)
        {
            if(!bl_ShowCrossHair)
            {
                ShowCrossHair();
            }

            tr_CrossHair.gameObject.SetActive(true);
            Vector3 vec_End = tr_ShootPoint.position + tr_ShootPoint.forward * fl_Range;
            tr_CrossHair.position = Vector3.Lerp(vec_End, tr_Camera.position, 0.9f);
            tr_CrossHair.rotation = tr_Camera.rotation;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class GunController : WeaponController
    {
        private Transform tr_CrossHair;

        public Transform tr_ShootPoint;
        public Transform tr_LeftHandPosition;
        public Transform tr_LeftElbowPosition;

        public BulletController bc_BulletPrefab;

        private bool bl_ShowCrossHair = false;

        protected override void Initialize()
        {
            tr_CrossHair = GetComponentInChildren<Canvas>().transform;
            tr_CrossHair.gameObject.SetActive(false);
        }

        public GunStatsController GetGunStats()
        {
            if (isc_Stats is GunStatsController)
            {
                return isc_Stats as GunStatsController;
            }
            else
            {
                GunStatsController gsc_defect = new GunStatsController();
                isc_Stats = gsc_defect;
                return gsc_defect;
            }
                
        }

        public override void Attack()
        {
            BulletController bc_Bullet = Instantiate<BulletController>(bc_BulletPrefab, tr_ShootPoint.position, tr_ShootPoint.rotation);
            GunStatsController gsc_Stats = GetGunStats();
            bc_Bullet.Initilize(gsc_Stats.fl_Power, gsc_Stats.fl_Damage, gsc_Stats.fl_LifeTime);

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
            Vector3 vec_End = tr_ShootPoint.position + tr_ShootPoint.forward * GetGunStats().fl_Range;
            tr_CrossHair.position = Vector3.Lerp(vec_End, tr_Camera.position, 0.9f);
            tr_CrossHair.rotation = tr_Camera.rotation;
        }
    }
}

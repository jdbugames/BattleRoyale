using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class GunController : MonoBehaviour
    {
        public Transform tr_ShootPoint;
        public Transform tr_LeftHandPosition;
        public Transform tr_LeftElbowPosition;

        public Transform tr_BulletPrefab;

        public float fl_MaxRecoil = 1f;
        public float fl_ShootingModifier = 2f;

        public void Shoot()
        {
            Instantiate(tr_BulletPrefab, tr_ShootPoint.position, tr_ShootPoint.rotation);
        }
    }
}

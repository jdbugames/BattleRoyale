using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    [CreateAssetMenu(fileName ="gunStats", menuName ="BattleRoyale/gun stats")]
    public class GunStatsController : WeaponStatsController
    {
        public BulletController bc_BulletPrefab;

        public float fl_Range = 10f;
        public float fl_MaxRecoil = 1f;
        public float fl_ShootingModifier = 2f;

        public float fl_Power = 100f;
        public float fl_LifeTime = 10f;
    }
}

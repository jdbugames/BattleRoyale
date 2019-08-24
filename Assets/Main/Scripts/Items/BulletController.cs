using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class BulletController : MonoBehaviour
    {
        public float fl_Power = 100f;
        public float fl_LifeTime = 10f;
        private float fl_DeltaTime = 0;
        Rigidbody rg_Bullet;

        private void Start()
        {
            rg_Bullet = GetComponent<Rigidbody>();

            rg_Bullet.velocity = this.transform.forward * fl_Power;
        }

        private void FixedUpdate()
        {
            fl_DeltaTime += Time.deltaTime;

            if(fl_DeltaTime >= fl_LifeTime)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

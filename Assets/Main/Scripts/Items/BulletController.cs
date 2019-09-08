using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class BulletController : MonoBehaviour
    {
        private float fl_LifeTime = 10f;
        private float fl_Damage;
        private float fl_DeltaTime = 0;
        Rigidbody rg_Bullet;


        public void Initilize(float fl_Power, float fl_Damage, float fl_LifeTime)
        {
            rg_Bullet = GetComponent<Rigidbody>();
            rg_Bullet.velocity = this.transform.forward * fl_Power;
            this.fl_Damage = fl_Damage;
            this.fl_LifeTime = fl_LifeTime;

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

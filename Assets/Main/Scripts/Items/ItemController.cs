using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class ItemController : MonoBehaviour
    {
        public ItemStatsController isc_Stats;
        internal Transform tr_MTransform;
        private Rigidbody rg_Rigidbody;
        private SphereCollider sc_Collider;

        public void Start()
        {
            tr_MTransform = this.transform;
            rg_Rigidbody = GetComponent<Rigidbody>();
            sc_Collider = GetComponent<SphereCollider>();
            Initialize();
        }

        protected virtual void Initialize()
        {

        }

        public void Take(Transform tr_SlotPosition)
        {
            tr_MTransform.parent = tr_SlotPosition;
            tr_MTransform.localPosition = Vector3.zero;
            tr_MTransform.localRotation = Quaternion.identity;
            rg_Rigidbody.detectCollisions = false;
            rg_Rigidbody.isKinematic = true;
        }

        public void Drop()
        {
            tr_MTransform.parent = null;
            rg_Rigidbody.detectCollisions = true;
            rg_Rigidbody.isKinematic = false;

            RaycastHit rh_Hit;
            if(Physics.Raycast(tr_MTransform.position, Vector3.down, out rh_Hit))
            {
                tr_MTransform.position = rh_Hit.point;
            }
        }
    }
}

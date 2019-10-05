using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class InventaryGroupViewerController : MonoBehaviour
    {
        internal InventaryGroup ig_ThisGroup;
        internal Transform tr_MTransform;

        public string str_SlotType;

        List<InventaryItemViewerController> iivc_ItemViewers = new List<InventaryItemViewerController>();
        

        public InventaryItemViewerController iivc_ItemViewerPrefab;
        

        public void Initialize(InventaryGroup ig_Group)
        {

        }
    }
}

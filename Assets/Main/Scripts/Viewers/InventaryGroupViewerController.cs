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
            this.tr_MTransform = this.transform;
            this.ig_ThisGroup = ig_Group;
            iivc_ItemViewers = new List<InventaryItemViewerController>(GetComponentsInChildren<InventaryItemViewerController>());

            while (iivc_ItemViewers.Count > 0)
            {
                InventaryItemViewerController iivc_Viewer = iivc_ItemViewers[0];
                Destroy(iivc_Viewer.gameObject);
                iivc_ItemViewers.RemoveAt(0);
            }

            iivc_ItemViewers.Clear();

            for (int i = 0; i < this.ig_ThisGroup.int_MaxCapacity; i++)
            {
                InventaryItemViewerController iivc_itemViewer = Instantiate<InventaryItemViewerController>(iivc_ItemViewerPrefab, this.tr_MTransform);
                iivc_itemViewer.Initialize(this);
                iivc_ItemViewers.Add(iivc_itemViewer);
                if(i < ig_ThisGroup.lst_Items.Count)
                {
                    iivc_itemViewer.SetItem(ig_ThisGroup.lst_Items[i]);
                }
            }
        }
    }
}

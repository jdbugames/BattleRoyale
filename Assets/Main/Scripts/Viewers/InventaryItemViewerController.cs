using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleRoyale
{
    public class InventaryItemViewerController : MonoBehaviour
    {

        InventaryGroupViewerController igvc_InventaryGroupViewer;
        Image img_ItemIcon;

        internal ItemController ic_InvItem;
        internal Transform tr_Mtransform;

        public void Initialize(InventaryGroupViewerController igvc_InventaryGroup)
        {
            this.tr_Mtransform = this.transform;
            this.igvc_InventaryGroupViewer = igvc_InventaryGroup;
            img_ItemIcon = GetComponent<Image>();
        }

        public void SetItem(ItemController ic_Item)
        {
            this.ic_InvItem = ic_Item;
            img_ItemIcon.sprite = (ic_Item != null ? ic_Item.isc_Stats.spr_Icon : null);
        }

        public void OnClickSelect()
        {
            igvc_InventaryGroupViewer.ig_ThisGroup.Select(ic_InvItem);
        }
    }
}

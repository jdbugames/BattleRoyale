using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class InventaryViewerController : MonoBehaviour
    {
        public List<InventaryGroupViewerController> lst_Viewers = new List<InventaryGroupViewerController>();
        Dictionary<String, InventaryGroupViewerController> dic_MappedViewers = new Dictionary<string, InventaryGroupViewerController>();

        internal void Initialize(InventaryController ic_inventaryController)
        {
            foreach(InventaryGroupViewerController v in lst_Viewers)
            {
                if(!dic_MappedViewers.ContainsKey(v.str_SlotType))
                {
                    dic_MappedViewers.Add(v.str_SlotType, v);
                    InventaryGroup ig_Group = ic_inventaryController.GetGroup(v.str_SlotType);

                    if(ig_Group != null)
                    {
                        v.Initialize(ig_Group);
                    }
                }
            }
        }
    }
}

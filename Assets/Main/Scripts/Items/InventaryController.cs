using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class InventaryController : MonoBehaviour
    {
        public ItemViewerController ivc_ItemViewer;
        public List<InventaryGroup> lst_InventaryGroups;

        private Dictionary<string, InventaryGroup> dic_MappedInventary = new Dictionary<string, InventaryGroup>();

        public void Start()
        {
            MapInventaryGroups();
        }

        private void MapInventaryGroups()
        {
            dic_MappedInventary.Clear();
            foreach (InventaryGroup g in lst_InventaryGroups)
            {
                if(!dic_MappedInventary.ContainsKey(g.str_SlotType))
                {
                    dic_MappedInventary.Add(g.str_SlotType, g);
                }
            }
        }

        public bool Bl_AddItem(ItemController Ic_Item)
        {
            if(dic_MappedInventary.ContainsKey(Ic_Item.Stats.str_SlotType))
            {
                return dic_MappedInventary[Ic_Item.Stats.str_SlotType].AddItem(Ic_Item);
            }
            return false;
        }
    }

    [System.Serializable]
    public class InventaryGroup
    {
        public string str_SlotType;
        public Transform tr_RealPosition;
        public int int_MaxCapacity = 3;
        public bool bl_ReplaceSelectedOnMax = true;

        public int int_SelIndex;
        public int int_SelectedIndex
        {
            set { int_SelIndex = value; }
            get { return int_SelIndex; }
        }

        public List<ItemController> lst_Items;

        public ItemController Ic_GetSelected()
        {
            return lst_Items[int_SelIndex];
        }

        public bool AddItem(ItemController Ic_Item)
        {
            if(lst_Items.Count >= int_MaxCapacity)
            {
                if(bl_ReplaceSelectedOnMax)
                {
                    Ic_GetSelected().Drop();
                    lst_Items[int_SelIndex] = Ic_Item;
                    Ic_Item.Take(tr_RealPosition);
                    return true;
                }
                return false;
            }
            else
            {
                lst_Items.Add(Ic_Item);
                Ic_Item.Take(tr_RealPosition);
                return true;
            }
        }
    }
}

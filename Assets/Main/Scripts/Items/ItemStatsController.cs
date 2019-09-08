using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    [CreateAssetMenu(fileName = "itemStats", menuName = "BattleRoyale/item stats")]
    public class ItemStatsController : ScriptableObject
    {
        public string str_ItemName;
        public string str_SlotType;
        public float fl_Weight;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BattleRoyale
{
    public class ItemViewerController : MonoBehaviour
    {
        private Transform tr_MTransform;
        private bool bl_ShowingViewer = false;
        public Vector3 vec_ViewerOffset;

        public TextMeshProUGUI txt_ItemName;
        public Image img_Icon;

        private void Start()
        {
            tr_MTransform = this.transform;
        }

        public void DrawItemViewer(ItemStatsController isc_Stats, Transform tr_ItemPost, Transform tr_Camera)
        {
            tr_MTransform.position = tr_ItemPost.position + vec_ViewerOffset;
            tr_MTransform.LookAt(tr_Camera);
            txt_ItemName.text = isc_Stats.str_ItemName;
            img_Icon.sprite = isc_Stats.spr_Icon;
            ShowViewer();
        }

        public void ShowViewer()
        {
            if (!bl_ShowingViewer)
            {
                bl_ShowingViewer = true;
                gameObject.SetActive(true);
            }
        }

        public void HideViewer()
        {
            if (bl_ShowingViewer)
            {
                bl_ShowingViewer = false;
                gameObject.SetActive(false);
            }
        }

    }
}

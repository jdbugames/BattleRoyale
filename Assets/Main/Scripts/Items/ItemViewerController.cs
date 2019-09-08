using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleRoyale
{
    public class ItemViewerController : MonoBehaviour
    {
        private Transform tr_MTransform;
        private bool bl_ShowingViewer = false;
        public Vector3 vec_ViewerOffset;

        private void Start()
        {
            tr_MTransform = this.transform;
        }

        public void DrawItemViewer(Transform tr_ItemPost, Transform tr_Camera)
        {
            tr_MTransform.position = tr_ItemPost.position + vec_ViewerOffset;
            tr_MTransform.LookAt(tr_Camera);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Fixgames.Volcano
{
    public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [HideInInspector]
        public Transform parentToReturnTo = null;
        private int siblingIndex = 0;   // 드래그 실패시 다시 넣을 위치
        private GameObject placeHolder = null;

        public void OnDestroy()
        {
            if(placeHolder != null)
            {
                Destroy(placeHolder);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            placeHolder = new GameObject();
            placeHolder.transform.SetParent(this.transform.parent);

            LayoutElement le = placeHolder.AddComponent<LayoutElement>();
            le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            le.flexibleWidth = 1;
            le.flexibleHeight = 1;
            placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            siblingIndex = this.transform.GetSiblingIndex();

            parentToReturnTo = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);

            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(parentToReturnTo);
            this.transform.SetSiblingIndex(siblingIndex);
            this.GetComponent<CanvasGroup>().blocksRaycasts = true;

            Destroy(placeHolder);
        }
    }
}

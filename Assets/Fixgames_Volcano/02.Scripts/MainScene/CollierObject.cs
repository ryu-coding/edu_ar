using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class CollierObject : MonoBehaviour
    {
        GameObject Target;
        bool mouseDragging;

        private void OnTriggerStay(Collider other)
        {
            // 실험도구끼리 Coilder 됫을때 DragObject 스크립트에 true를 보낸다
            GameObject.Find("DragManager").SendMessage("SetDeleteObject", true, SendMessageOptions.DontRequireReceiver);
            // 지금 드래그 중인 Object를 가져옴
            Target = GameObject.Find("DragManager").GetComponent<DragObject>().Target();
            if(gameObject != Target)
            {
                // getTarget이외에 충돌된 실험도구를 반환
                GameObject.Find("DragManager").SendMessage("SetColliderObject", this.gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Drag중인지 확인
            mouseDragging = GameObject.Find("DragManager").GetComponent<DragObject>().MouseDragging();
            if(mouseDragging == true)
            {
                // 드래그 중에 collider이 종료됫으면  DragObject 스크립트에 false를 보낸다
                GameObject.Find("DragManager").SendMessage("SetDeleteObject", false, SendMessageOptions.DontRequireReceiver);    
            }
        }
    }
}

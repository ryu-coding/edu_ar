using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class NameTag : MonoBehaviour
    {

        public DragObject dragObject;
        // 실험도구 순서
        public int index;
        //이 버튼이 따라갈 위치
        Transform pos;
        // 카메라와의 거리
        float camDis;
        Vector2 screenPos;
        void Update()
        {
            // AR
            if (GameObject.Find("Controller").GetComponent<Controller>() != null)
            {
                if (GameObject.Find("Controller").GetComponent<Controller>().ObjectP1 == null)
                {
                    return;
                }
            }
            // NoAR
            else
            {
                if (GameObject.Find("Controller").GetComponent<ControllerNoAr>().ObjectP1 == null)
                {
                    return;
                }
            }
            // 따라갈 포지션
            pos = GameObject.FindWithTag("" + index).transform;
            if (pos == null)
            {
                return;
            }
            // 위치 지정
            screenPos = Camera.main.WorldToScreenPoint(pos.position);
            if(dragObject.controller != null)
                this.transform.position = new Vector3(screenPos.x, screenPos.y + 200 * dragObject.fscale);
            else
                this.transform.position = new Vector3(screenPos.x, screenPos.y + 200 * dragObject.fscale * 7);
            // 객체와 카메라간의 거리
            camDis = Vector3.Distance(Camera.main.transform.position, pos.position);
            // 거리별 크기조절
            if (GameObject.Find("Controller").GetComponent<Controller>() != null)
            {
                if (GameObject.Find("Controller").GetComponent<Controller>().ObjectP1 != null)
                {
                    this.transform.localScale = GameObject.Find("Controller").GetComponent<Controller>().ObjectP1.transform.localScale * 2 / camDis;
                } // 버튼의 크기를 물체의 크기 및 카메라와의 거리에 따라 조정합니다.
            }
            else
            {
                if (GameObject.Find("Controller").GetComponent<ControllerNoAr>().ObjectP1 != null)
                {
                    this.transform.localScale = GameObject.Find("Controller").GetComponent<ControllerNoAr>().ObjectP1.transform.localScale * 2 / camDis;
                } // 버튼의 크기를 물체의 크기 및 카메라와의 거리에 따라 조정합니다.
            }

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class FollowTag : MonoBehaviour
    {
        public Transform pos;
        // 카메라와의 거리
        public float camDis;
        Vector2 screenPos;
        string nameTag;
        Controller controller;

        void Start()
        {
            nameTag = this.gameObject.name; //objet 의 이름을 저장.
        }

        // Update is called once per frame
        void Update()
        {
			 pos = GameObject.FindWithTag(nameTag).transform;           
            // 자신의 name과 같은 Tag이름을 가진 object로 위치 지정
            screenPos = Camera.main.WorldToScreenPoint(pos.position);
            this.transform.position = new Vector3(screenPos.x, screenPos.y);
            // 객체와 카메라간의 거리
            camDis = Vector3.Distance(Camera.main.transform.position, pos.position);

            if (GameObject.Find("Controller").GetComponent<Controller>() != null)
            {
                if (GameObject.Find("Controller").GetComponent<Controller>().VolcanoObject != null)
                {
                    this.transform.localScale = GameObject.Find("Controller").GetComponent<Controller>().VolcanoObject.transform.localScale * 2 / camDis;
                } // 버튼의 크기를 물체의 크기 및 카메라와의 거리에 따라 조정합니다.
            }
            else
            {
                if (GameObject.Find("Controller").GetComponent<ControllerNoAr>().VolcanoObject != null)
                {
                    this.transform.localScale = GameObject.Find("Controller").GetComponent<ControllerNoAr>().VolcanoObject.transform.localScale * 2 / camDis;
                } // 버튼의 크기를 물체의 크기 및 카메라와의 거리에 따라 조정합니다.
            }

        }
    }
}

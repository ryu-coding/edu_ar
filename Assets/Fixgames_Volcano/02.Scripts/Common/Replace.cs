using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    // Object를 재배치하는 Script

    public class Replace : MonoBehaviour
    {
        /// <summary>
        /// 재배치하기를 눌렸을때 사용할, 
        /// object(실험 모델과 화산 모델), 움직임을 확인하고 초기 위치를 저장할 변수를 설정.
        /// </summary>
        public GameObject popup, obj1,obj2, ExperimentDrag;
        public Vector3 past;
        float positionX, positionY;
        public bool play = false;

        /// <summary>
        /// 클릭이 되었을때,
        /// Chapter에 따라 해당하는 object의 위치 변경을 실행
        /// </summary>
        public void Onclick()
        {
            if (GameObject.Find("StageManager").GetComponent<Stage>().getStageNum() == 1)
            {
                ExperimentDrag.SetActive(false);
                if (GameObject.Find("Controller").GetComponent<Controller>() != null)
                {
                    past = GameObject.Find("Controller").GetComponent<Controller>().ObjectP1.transform.position;
                }
                else
                {
                    past = GameObject.Find("Controller").GetComponent<ControllerNoAr>().ObjectP1.transform.position;
                }
            }
            else if (GameObject.Find("StageManager").GetComponent<Stage>().getStageNum() == 2)
            {
                obj2.GetComponent<Rotate>().SetRotate(true);
                past = obj2.transform.position;
            }
            if (GameObject.Find("StageManager").GetComponent<Stage>().getStageNum() != 3)
            {
                popup.SetActive(true);
                play = true;
            }
        }
    }
}

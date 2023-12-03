using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fixgames.Volcano
{
    // Object를 재배치하는 Script

    public class ReplacePopup : MonoBehaviour
    {
        /// <summary>
        /// 재배치하기를 눌렸을때 사용할, 
        /// object(실험 모델과 화산 모델), 움직임을 확인하고 초기 위치를 저장할 변수를 설정.
        /// </summary>
        public GameObject popup, replaceObject, obj2, ExperimentDrag;
        public GameObject replace;
        bool isMouseDragging;
        Vector3 past, offsetValue, positionOfScreen;
        float positionX, positionY;
        Quaternion rotation;
        void Update()
        {
            // 재배치를 위한 단계별 오브젝트 선택
            if (GameObject.Find("StageManager").GetComponent<Stage>().StageNum == 1)
            {
                if (GameObject.Find("Controller").GetComponent<Controller>() != null)
                {
                    replaceObject = GameObject.Find("Controller").GetComponent<Controller>().ObjectP1;
                }
                else
                {
                    replaceObject = GameObject.Find("Controller").GetComponent<ControllerNoAr>().ObjectP1;
                }
            }
            else
            {
                replaceObject = obj2;
            }

            // 오브젝트 재배치
            if (Input.GetMouseButtonDown(0))
            {
                if (replaceObject != null)
                {
                    isMouseDragging = true;
                    positionOfScreen = Camera.main.WorldToScreenPoint(replaceObject.transform.position);
                    offsetValue = replaceObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
                }
            }
            if (isMouseDragging)
            {
                Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);

                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;
                if (GameObject.Find("Controller").GetComponent<Controller>() != null)
                {
                    currentPosition.y = replaceObject.transform.position.y;
                }
                replaceObject.transform.position = currentPosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isMouseDragging = false;
            }


        }
        /// <summary>
        /// Object를 움직였으나,
        /// Popup에서 (X)를 눌렸을때, 해당하는 object의 위치를 초기의 위치로 변경한다.
        /// </summary>
        public void Cancel()
        {
            if (GameObject.Find("StageManager").GetComponent<Stage>().getStageNum() == 1)
            {
                ExperimentDrag.SetActive(true);
                popup.SetActive(false);
                replaceObject.transform.position = replace.GetComponent<Replace>().past;
            }

            if (GameObject.Find("StageManager").GetComponent<Stage>().getStageNum() == 2)
            {
                obj2.GetComponent<Rotate>().SetRotate(false);
                popup.SetActive(false);
                obj2.transform.position = replace.GetComponent<Replace>().past;
            }
            isMouseDragging = false;
            replace.GetComponent<Replace>().play = false;
        }

        /// <summary>
        /// Object를 움직였을때,
        /// Popup에서 (확인)를 눌렸을때, 해당하는 object의 위치를 현재 위치로 저장 변경한다.
        /// </summary>

        public void CheckPopup()
        {
            if (GameObject.Find("StageManager").GetComponent<Stage>().getStageNum() == 1)
            {
                if (GameObject.Find("Controller").GetComponent<Controller>() != null)
                {
                    GameObject.Find("Controller").GetComponent<Controller>().Prefab2.transform.position = replaceObject.transform.position;
                }
                else
                {
                    GameObject.Find("Controller").GetComponent<ControllerNoAr>().Prefab2.transform.position = replaceObject.transform.position;
                }
                ExperimentDrag.SetActive(true);
            }

            if (GameObject.Find("StageManager").GetComponent<Stage>().getStageNum() == 2)
            {
                obj2.GetComponent<Rotate>().SetRotate(false);
                if (GameObject.Find("Controller").GetComponent<Controller>() != null)
                {
                    GameObject.Find("Controller").GetComponent<Controller>().Prefab2.transform.position = obj2.transform.position;
                }
                else
                {
                    GameObject.Find("Controller").GetComponent<ControllerNoAr>().Prefab2.transform.position = obj2.transform.position;
                }
            }
            isMouseDragging = false;
            replace.GetComponent<Replace>().play = false;
            popup.SetActive(false);
        }
    }
}

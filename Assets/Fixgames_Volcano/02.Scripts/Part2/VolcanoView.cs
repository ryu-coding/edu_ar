using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    /// <summary>
    /// "단면보기" 버튼을 눌렸을때의 Script
    /// </summary>

    public class VolcanoView : MonoBehaviour
    {
        /// <summary>
        /// 15초 후 생성을 위한, timer 변수 및 popup 그리고 object 등을 변수로 지정.
        /// </summary>

        public GameObject popup, allobj, sectionobj, Controller,jet,AshTag,Ash;
        float theTime;
        float speed = 1;
        bool playing;
        Vector3 Createposition;

        public void ResetObject()
        {
            ///<summary>
            ///Chapter2에 초기 실행을 위해 보조하는 메소드로, 마지막까지 실행 되고 있는 모든 object, 변수, 시간 등을 초기 값으로 변경.
            ///</summary>

            playing = false;

            //화산재Tag 비활성화(초기)
            Ash.SetActive(false);
            AshTag.SetActive(false);

            ///<summary>
            ///ar 여부에 따라 Controller 실행.
            ///</summary>

            Controller control1 = Controller.GetComponent<Controller>();
            if(control1 != null)
            {
                control1.ChangeObject();
            }

            ControllerNoAr control2 = Controller.GetComponent<ControllerNoAr>();
            if(control2 != null)
            {
                control2.ChangeObject();
            }
            //초기의 상태로 되돌림(단면 비활성화 전면 활성화.)
            allobj.SetActive(true);
            sectionobj.SetActive(false);
            theTime = 0; // 단면보기클릭시 싱행되는 시간
        }

        void Update()
        {
            ///<summary>
            ///버튼이 눌려지고난 후 15초 후에 EndPopup창이 생성된다.
            ///</summary>

            if (playing == true)
            {
                theTime += Time.deltaTime * speed;
                if(theTime >= 15)
                {
                    popup.SetActive(true);  
                }
            }
        }

        public void OnClick()
        {
            ///<summary>
            ///단면 보기를 눌렸을 경우,
            ///Update에서 시간을 측정할수 있게 해주며, 
            ///object변경 및 Controller에 따라 설정 값을 변경.
            ///</summary>

            playing = true;
            Createposition = allobj.transform.position; // 화산단면을 생성 전 화산 전면의 위치를 저장.
            jet.SetActive(false);
            allobj.SetActive(false);
            sectionobj.SetActive(true);

            ///<summary>
            ///ar여부에 따라 Controller 실행.
            ///화산 단면의 위치는 화산 전면의 위치를 따라감.
            ///</summary>

            Controller control1 = Controller.GetComponent<Controller>();
            if(control1 != null)
            {
                sectionobj.transform.position = Createposition;
                sectionobj.transform.rotation = Quaternion.identity;  
            }

            ControllerNoAr control2 = Controller.GetComponent<ControllerNoAr>();
            if(control2 != null)
            {
                sectionobj.transform.position = Createposition;
            }
            
            ///<summary>
            ///단면에서의 용암 Controll 활성화.
            ///</summary>
            
            MagmaController magmaController = sectionobj.GetComponent<MagmaController>();
            if(magmaController != null)
            {
                magmaController.ResetAni();
                magmaController.SetMagma(true);
            }
        }
    }
}

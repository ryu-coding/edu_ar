using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    //"단면 보기" 버튼을 활성화 하는 Script.

    public class CreateBtn : MonoBehaviour
    {
        public GameObject btn; // 생성할 button(단면보기) object
        /// <summary>
        /// 시작여부, 실제 시간을 담고, Time에서의 시간을 float형으로 변환 해주기 위한 변수 설정
        /// </summary>
        bool start; 
        float theTime;  
        float speed = 1;
        
        void Start()
        {
            start = true; //Volcano 생성 시 실행
        }
        
        void Update()
        {
            if(start == true)
            {
                /// <summary>
                /// 실제 시간을 곱한값을 theTime에 저장하여,
                /// 화산 폭발이 진행되는 시간(Particle)을 고려하여 15초 후에 생성 .
                /// </summary>
                theTime += Time.deltaTime * speed; 
                if(theTime >= 15)    
                {
                    btn.SetActive(true);// '단면보기' 버튼 활성화
                }
            }
        }

        public void Reset()
        {
            theTime = 0; // timer 초기화
        }
    }
}

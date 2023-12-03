using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    ///<summary>
    /// Popup창을 끄는 용도의 Script.
    ///</summary>

    public class Close : MonoBehaviour
    {
        public GameObject popup;
        public bool check;  //두번째 Chapter에서 Popup창(확인)을 눌린 후, object생성을 위해 확인 하는 변수

        public void Start()
        {
            check = false;  //실행시, false이며, true가 되면 Object(Volcano)를 생성.
        }

        public void OnClick()
        {
            check = true;
            popup.SetActive(false);  // 클릭을 인식하면 popup창을 비활성화 시킴.
        }

        public void SetCheck(bool other)
        {
            check = other;  //외부에서 초기화 시킬 경우, 사용하는 메소드
        }
    }
}

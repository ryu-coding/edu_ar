using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    /// <summary>
    /// 자기 자신(self)는 화면에서 지우는 용도로, 
    /// obj는 어떠한 GameObject같은 것을 생성해주는 Script.
    /// </summary>

    public class ChangeObj : MonoBehaviour
    {
        public GameObject obj;  //Target이 되는 object
        public GameObject self; //자기자신 object

        public void OnClick()
        {
            obj.SetActive(true);
            self.SetActive(false);
        }
    }
}

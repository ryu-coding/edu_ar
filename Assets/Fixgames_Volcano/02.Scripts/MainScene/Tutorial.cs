using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class Tutorial : MonoBehaviour
    {
        public void Click()
        {
            if (GameObject.Find("StageManager").GetComponent<Stage>().getStageNum() == 1)
            {
                // 튜토리얼 ture로 반환
                GameObject.Find("DragManager").GetComponent<DragObject>().tutorial = true;
            }
        }
    }
}

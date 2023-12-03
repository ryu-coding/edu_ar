using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class BlendShape : MonoBehaviour
    {
        SkinnedMeshRenderer skinnedMeshRenderer;
        // 변수
        float blendOne = 0f;
        float blendTwo = 0f;
        float blendThree = 0f;
        float blendSpeed = 1f;
        // 끝났는지 확인
        bool blendOneFinished = false;
        bool blendTwoFinished = false;
        // 실험 순서
        int exTurn = 0;

        void Awake()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }
        // 마지막 값 반환
        public float GetblendThree()
        {
            return blendThree;
        }

        void Update()
        {
            // DragObject의 실험순서
            if (GameObject.Find("DragManager") != null)
                exTurn = GameObject.Find("DragManager").GetComponent<DragObject>().GetExperimentTurn();
            else
                return;

            // 마시멜로 흘러내리는 애니메이션
            if(exTurn == 10)
            {
                // 2번째가 100될때 까지 ++
                if(blendOne < 100f && blendOneFinished != true)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(2, blendOne);
                    blendOne += blendSpeed;
                }
                // 100되면 --
                else
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(2, blendOne);
                    if(blendOne != 0)
                        blendOne -= 1;
                    blendOneFinished = true;
                }
                // 2번째 --하면서 1번째 100될때까지 ++
                if(blendOneFinished == true && blendTwo < 100f && blendTwoFinished != true)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(1, blendTwo);
                    blendTwo += blendSpeed;
                }
                // 1번째 100되면 --
                else
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(1, blendTwo);
                    if(blendTwo >= 99f)
                        blendTwoFinished = true;
                    if(blendTwo != 0)
                        blendTwo -= 1;
                }
                // 1번째 --하면서 0번째 100될때까지 ++
                if(blendTwoFinished == true && blendThree < 100f)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(0, blendThree);
                    blendThree += blendSpeed;
                    if(blendThree >= 99f)
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(0, blendThree);
                        GameObject.Find("DragManager").SendMessage("SetblendNum", blendThree, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            // 포일 접히는 애니메이션
            else if(exTurn == 3 && GameObject.Find("DragManager").GetComponent<DragObject>().BlendStart())
            {
                // 0번째 99될때까지 ++
                if(blendOne <= 99f)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(0, blendOne);
                    blendOne += blendSpeed;
                    if (blendOne == 99)
                    {
                        // 99되면 값 반환
                        skinnedMeshRenderer.SetBlendShapeWeight(0, blendOne);
                        GameObject.Find("DragManager").SendMessage("SetblendNum", blendOne, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }
}

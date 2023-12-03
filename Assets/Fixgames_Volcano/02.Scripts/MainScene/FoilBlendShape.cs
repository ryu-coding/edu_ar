
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fixgames.Volcano
{
    public class FoilBlendShape : MonoBehaviour
    {

        int blendShapeCount;
        SkinnedMeshRenderer skinnedMeshRenderer;
        // 실험순서
        int exTurn = 0;
        //변수
        float blendOne = 0f, blendZero = 99f;
        float blendTwo = 0f;
        float blendThree = 0f;
        // 속도
        float blendSpeed = 1f;
        // 끝났는지 확인
        bool blendOneFinished = false;
        bool blendTwoFinished = false;

        void Awake()
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        public float GetblendThree()
        {
            return blendThree;
        }

        void Update()
        {
            // 실험도구 12번째 순서
            // 마시멜로가 흘러내리면서 포일이 살짝 열리는 애니메이션
            if(GameObject.Find("DragManager") != null)
                exTurn = GameObject.Find("DragManager").GetComponent<DragObject>().GetExperimentTurn();
            if (exTurn == 10)
            {
                // blendOne 100까지 ++
                if (blendOne < 100f && blendOneFinished != true)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(0, blendZero);
                    blendZero -= blendSpeed;

                    skinnedMeshRenderer.SetBlendShapeWeight(1, blendOne);
                    blendOne += blendSpeed;
                }
                // 100됫으면 다음단계 이동
                else
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(1, blendOne);
                    if (blendOne != 0)
                        blendOne -= 1;
                    blendOneFinished = true;
                }
                // blendTwo 100까지 ++
                if (blendOneFinished == true && blendTwo < 100f && blendTwoFinished != true)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(2, blendTwo);
                    blendTwo += blendSpeed;
                }
                // 다음단계이동
                else
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(2, blendTwo);
                    if (blendTwo == 100)
                        blendTwoFinished = true;
                    if (blendTwo != 0)
                        blendTwo -= 1;
                }
                // blendThree 100까지 ++
                if (blendTwoFinished == true && blendThree < 100f)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(3, blendThree);
                    blendThree += blendSpeed;
                }
            }
        }
    }
}

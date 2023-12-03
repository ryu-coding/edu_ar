using UnityEngine;
using System.Collections;

namespace Fixgames.Volcano
{
    /// <summary>
    /// 용암이 흘러내리는 효과 제어 Script.
    /// </summary>
    /// 

    public class ScrollingUVs_Layers : MonoBehaviour
    {
        /// <summary>
        /// 실행 여부,texture의 크기 이름, material
        /// 초기값설정.
        /// </summary>

        public Vector2 uvStartOffset = new Vector2(0f, 0f);
        public Vector2 uvEndOffset = new Vector2(0f, 0.5f);

        public Vector2 uvAnimationRate = new Vector2(0f, 0.01f);

        public float transitionTime = 35f;

        public string textureName = "_lava_mask_02_01";

        private Material matLava_m;

        private bool start = false;

        void Start()
        {
            ///<summary>
            /// 초기에는 리셋을 시킴.
            /// </summary>
            /// 

            matLava_m = GetComponent<Renderer>().sharedMaterial;

            ResetAni();
        }

        void Update()
        {
            ///<summary>
            ///실행이 되었을때 코루틴 호출 실행.
            /// </summary>

            if(start)
            {
                StartCoroutine(DoMountainColorLerp());

                start = false;
            }
        }

        /// <summary>
        /// 용암이 흘러내리는 효과의 코루틴
        /// </summary>

        private IEnumerator DoMountainColorLerp()
        {
            float timeElapsed = 0f;

            while(timeElapsed < transitionTime)
            {
                timeElapsed += Time.deltaTime * Random.Range(0.001f, 1f);

                if(matLava_m != null)
                {
                    matLava_m.SetTextureOffset(textureName, Vector2.Lerp(uvStartOffset, uvEndOffset, timeElapsed / transitionTime));
                }

                yield return null;
            }
        }

        public void StartAni()
        {
            start = true;
        }

        /// <summary>
        /// 초기의 상태(용암이 흘러내지 않은) 되돌림.
        /// </summary>

        public void ResetAni()
        {
            if(matLava_m != null)
            {
                matLava_m.SetTextureOffset(textureName, uvStartOffset);
            }
        }
    }
}

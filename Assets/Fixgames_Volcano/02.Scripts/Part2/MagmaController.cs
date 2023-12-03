using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    /// <summary>
    /// 마그마 애니메이션을 활용하기 위한 Script. 
    /// 화산 단면에 적용된 Script로, 단면 부분과 겹치지 않도록 하여,
    /// 마그마분출이 일어나는 것을 볼 수 있도록 하기 위해서 사용.(기존의 화산에서 element하나 적게 실행. -> 단면을 보는 위치에도 생성됨)
    /// </summary>

    public class MagmaController : MonoBehaviour
    {
        /// <summary>
        /// 용암 효과의 초기 값 설정.
        /// </summary>

        [Header("Value")]
        public bool start = false;

        [Header("Mountain Color")]
        public float transitionMountainColorTime = 10f;
        public Color startColor = Color.black;
        public Color endColor = new Color32(255, 128, 128, 255);
        public Renderer vol_m_01;
        public string textureName_vol_m_01 = "vol_m_01";
        private Material matVol_m_01;

        [Header("Magma Flow")]
        public List<ScrollingUVs_Layers> listMagmaFlow;

        void Start()
        {

            if (vol_m_01 != null)
            {
                for(int ii = 0; ii < vol_m_01.sharedMaterials.Length; ++ii)
                {
                    if(textureName_vol_m_01.Equals(vol_m_01.sharedMaterials[ii].name))
                    {
                        matVol_m_01 = vol_m_01.sharedMaterials[ii];

                        matVol_m_01.SetColor("_EmissionColor", Color.black);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 실행 여부 설정.
        /// </summary>

        public void SetMagma(bool magma)
        {
            start = magma;
        }
        
        private void Update()
        {
            ///<summary>
            ///실행시 용암이 흘러내리는 효과 설정.
            ///</summary>
            ///

            if(start)
            {
                StartCoroutine(DoMountainColorLerp());

                for(int ii = 0; ii < listMagmaFlow.Count; ++ii)
                {
                    listMagmaFlow[ii].StartAni();
                }

                start = false;
            }
        }

        /// <summary>
        /// 용암이 흘러내리는 효과의 코루틴
        /// </summary>

        private IEnumerator DoMountainColorLerp()
        {
            float timeElapsed = 0f;
            
            while(timeElapsed < transitionMountainColorTime)
            {
                timeElapsed += Time.deltaTime;
                if(matVol_m_01 != null)
                {
                    matVol_m_01.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, timeElapsed / transitionMountainColorTime));
                }

                yield return null;
            }
        }
        
        /// <summary>
        /// 초기의 상태(용암이 흘러내지 않은) 되돌림.
        /// </summary>
        /// 
        public void ResetAni()
        {
            if(matVol_m_01 != null)
            {
                matVol_m_01.SetColor("_EmissionColor", Color.black);
            }

            for(int ii = 0; ii < listMagmaFlow.Count; ++ii)
            {
                listMagmaFlow[ii].ResetAni();
            }
        }
    }
}

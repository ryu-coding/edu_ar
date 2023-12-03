using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    /// <summary>
    /// Particle실행을 좀더 매끄럽게 하기위해 통제하는 Script
    /// </summary>

    public class Particle : MonoBehaviour
    {
        /// <summary>
        /// Volcano에서 발생해야할 모든 Particle과 소리, 애니메이션을 변수로 설정.
        /// </summary>

        public ParticleSystem Smoke_Light, Smoke_Dense, FireExplosionDirectionalALT, BlastEmbers;
        public GameObject magma, Ash;
        public GameObject AshNameTage;
        public AudioClip[] audioClips;
        public AudioSource AudioSource;
        int count;

        /// <summary>
        /// 마그마가 흘러 내리는 Animation을 실행하기 위한 설정값.
        /// </summary>
        
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
            AudioSource = GetComponent<AudioSource>(); // 화산 폭발 효과음 설정을 위해 Audiosource 설정.
        }
        

        public void StartParticle()
        {
            ///<summary>
            /// Scene이 시작되고 Object가 생성 되면 바로 작은 연기(Particle) 실행
            /// 그 외 활성화 되어 있는 Particle은 정지 함.
            ///</summary> 
            Smoke_Light.Stop();
            ResetAni();
            Smoke_Light.Play();
            Smoke_Dense.Stop();
            FireExplosionDirectionalALT.Stop();
            BlastEmbers.Stop();
            count = 0;

            ///<summary>
            ///10초 후에 작은 연기가 멈추고,
            ///동시에 화산 폭발이 이루어 지게 함.(메소드 호출)
            ///</summary>
            ///
            
            Invoke("StopSmallSmoke", 5f);
            Invoke("StartExplosion", 13f);

            ///<summary>
            ///용암의 초기 상태로,
            ///코루틴 호출 이후, 변경 되는 값.
            ///</summary>

            if (vol_m_01 != null)
            {
                for (int ii = 0; ii < vol_m_01.sharedMaterials.Length; ++ii)
                {
                    if (textureName_vol_m_01.Equals(vol_m_01.sharedMaterials[ii].name))
                    {
                        matVol_m_01 = vol_m_01.sharedMaterials[ii];

                        matVol_m_01.SetColor("_EmissionColor", Color.black);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 용암이 흘러내리는 효과의 코루틴
        /// </summary>

        private IEnumerator DoMountainColorLerp()
        {
            float timeElapsed = 0f;

            while (timeElapsed < transitionMountainColorTime)
            {
                timeElapsed += Time.deltaTime;
                if (matVol_m_01 != null)
                {
                    matVol_m_01.SetColor("_EmissionColor", Color.Lerp(startColor, endColor, timeElapsed / transitionMountainColorTime));
                }

                yield return null;
            }
        }

        /// <summary>
        /// 초기의 상태(용암이 흘러내지 않은) 되돌림.
        /// </summary>

        public void ResetAni()
        {
            if (matVol_m_01 != null)
            {
                matVol_m_01.SetColor("_EmissionColor", Color.black);
            }

            for (int ii = 0; ii < listMagmaFlow.Count; ++ii)
            {
                listMagmaFlow[ii].ResetAni();
            }
        }
        
        /// <summary>
        /// 화산 폭발시 나오는 소리 출력하는 메소드(코루틴)
        /// </summary>

        IEnumerator ExplosionAudioPlay()
        {
            AudioSource.clip = audioClips[0];
            AudioSource.Play();
            yield return new WaitForSeconds(0.5f);
        }

        /// <summary>
        /// 시작하는 흰 연기의 Particle실행을 멈춤
        /// </summary>
        void StopSmallSmoke()
        {
            Smoke_Light.Stop();  
        }

        /// <summary>
        /// 폭발 Particle실행을 시작하고, 
        /// 시작후 0.5초후에는 화산 암석이 날라가는 Particle,
        /// 2초 후에는 큰연기 생성, 
        /// 4초후에는 폭발을 멈추는 메소드를 실행
        /// </summary>

        void StartExplosion()
        {
            if(gameObject.activeSelf)
            {
                StartCoroutine("ExplosionAudioPlay");
                FireExplosionDirectionalALT.Play();
                StartCoroutine(DoMountainColorLerp());

                ///<summary>
                ///화산 폭발과 동시에 용암이 흘러 내리도록 함.
                ///</summary>
                for (int ii = 0; ii < listMagmaFlow.Count; ++ii)
                {
                    listMagmaFlow[ii].StartAni();
                }

                ///<summary>
                ///1회 복발시만 화산 암석도 같이 활성화
                /// 시간 별로 폭발 중지와 연기 중지를 반복.
                /// </summary>

                count++;
                Invoke("StartSmoke", 0.5f);
                Invoke("StopExplosion", 4f);
                if(count <=1)
                    Invoke("StartBlast", 0.5f);
                if(count > 1)
                    Invoke("StopBlast", 0.5f);
            }
        }

        /// <summary>
        /// 폭발Paritcle 멈추게 하는 메소드
        /// </summary>

        void StopExplosion()
        {
            FireExplosionDirectionalALT.Stop();
        }

        /// <summary>
        /// 큰연기가 실행,
        /// 폭발의 반복을 위해 다시폭발 Particle을 실행
        /// </summary>

        void StartSmoke()
        {
            Smoke_Dense.Play();
            Ash.SetActive(true);
            AshNameTage.SetActive(true);
            Invoke("StartExplosion", 13f);
        }


        /// <summary>
        /// 암석 Paritcle을 실행 및 정지를
        /// 연기 생성 여부에 따라 반복 실행.
        /// </summary>

        void StartBlast()
        {
            BlastEmbers.Play();
            Invoke("StopBlast",1f);
        }
        void StopBlast()
        { 
            BlastEmbers.Stop();
        }
        
        /// <summary>
        /// 반복을 위해 사용한 Invoke 모두 정지
        /// 용암도 초기 상태를 돌리기 위해 Coroutine 정지.
        /// </summary>
        /// 

        public void AllStop()
        {
            CancelInvoke();
            StopCoroutine(DoMountainColorLerp());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class ExperimentParticle : MonoBehaviour
    {
        public GameObject smoke1;
        public ParticleSystem smoke;
        public ParticleSystem candle;

        // 연기 파티클 플레이
        public void PlaySmokeParticle()
        {
            smoke1.SetActive(true);
            smoke.Play();
        }
        // 캔들 파티클 플레이
        public void PlayCandleParticle()
        {
            candle.Play();
        }
    }
}

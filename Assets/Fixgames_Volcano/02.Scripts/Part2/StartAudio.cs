using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    /// <summary>
    /// Scene이 시작되었을때 나오는 소리 Script
    /// </summary>

    public class StartAudio : MonoBehaviour
    {
        /// <summary>
        /// Popup창이 시작되면서, 나오는 소리를 설정하기 위해 Clip으로 3개를 넣어두고, 순차적으로 호출함.
        /// </summary>
        public AudioClip[] audioClips;
        public AudioSource AudioSource;
        int count = 0;
        // Use this for initialization
        void Start()
        {
            AudioSource = GetComponent<AudioSource>();
            StratPlay();
        }

        public AudioSource AudioPlayer()
        {
            return AudioSource;
        }

        /// <summary>
        /// Scene이 시작 되었을때,
        /// count를 확인 하여, 출력해야할 코루틴을 찾아서 출력한다.
        /// 3개의 오디오를  순차적으로 실행.
        /// </summary>

        public void StratPlay()
        {
            if (AudioSource.isPlaying)
            {
                AudioSource.Stop();
                if (count == 0 || count == 1 || count == 2)
                    count = 3;
            }
            AudioSource.clip = audioClips[count];
            AudioSource.Play();
            count++;
            if (count == 1)
                StartCoroutine("MainAudio");   
        }

        /// <summary>
        /// Popup창 실행시 저장하고 있는 3개의 Audio 순차적으로 실행
        /// </summary>
        
        IEnumerator MainAudio()
        {
            while (count < 3)
            {
                if (!AudioSource.isPlaying)
                {
                    AudioSource.clip = audioClips[count];
                    AudioSource.Play();
                    count++;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}

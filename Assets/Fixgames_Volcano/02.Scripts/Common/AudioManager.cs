using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class AudioManager : MonoBehaviour
    {
        // Audio Clip 변수
        public AudioClip[] audioClips;
        // Audio Source
        AudioSource AudioSource;
        // Audio Clip 순서
        public int count = 0;
        // Use this for initialization
        void Start()
        {
            AudioSource = GetComponent<AudioSource>();
        }
        public AudioSource AudioPlayer()
        {
            return AudioSource;
        }
        public void Play()
        {
            // 오디오가 플레이 중이면
            // stop하고 다음 나래이션을 play한다.
            if(AudioSource.isPlaying)
            {
                AudioSource.Stop();
            }
            AudioSource.clip = audioClips[count];
            AudioSource.Play();
            count++;
            // 첫번째 나래이션
            if(count == 1)
                StartCoroutine("MainAudio");
            // 10번째 나래이션
            else if(count == 10)
                StartCoroutine("TouchAudio");
        }
        public void Stop()
        {
            AudioSource.Stop();
        }

        IEnumerator MainAudio()
        {
            while(count < 3)
            {
                if(!AudioSource.isPlaying)
                {
                    AudioSource.clip = audioClips[count];
                    AudioSource.Play();
                    count++;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator TouchAudio()
        {
            yield return new WaitForSeconds(8.7f);
            AudioSource.clip = audioClips[count];
            AudioSource.Play();
            count++;
        }

        public void ScreenshotSound()
        {
            AudioSource.clip = audioClips[30];
            AudioSource.Play();
        }
    }
}

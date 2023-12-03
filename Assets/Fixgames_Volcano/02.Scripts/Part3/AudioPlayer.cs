using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class AudioPlayer : MonoBehaviour
    {
        [Header("Part3_2 Sound")]
        // Success Sound
        public AudioClip SuccessAudioClips;
        // Fail Sound
        public AudioClip FailAudioClips;
        // Audio Source
        AudioSource AudioSource;

        // Use this for initialization
        void Start()
        {
            AudioSource = GetComponent<AudioSource>();
        }
        // Success
        public void SuccessPlay()
        {
            AudioSource.clip = SuccessAudioClips;
            AudioSource.Play();
        }
        // Fail
        public void FailPlay()
        {
            AudioSource.clip = FailAudioClips;
            AudioSource.Play();
        }
    }
}

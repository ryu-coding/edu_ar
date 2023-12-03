using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    /// <summary>
    /// Part2의 마지막에 생성되는 Popup창에 맞는 소리를 출력하는 Script
    /// </summary>

    public class EndAudio : MonoBehaviour
    {
        public AudioSource AudioSource;
        

        void Awake()
        {
            AudioSource.GetComponent<AudioSource>();
        }

        // Use this for initialization
        void Start()
        {
            AudioSource.Play(); //Popup창이 생성됨과 동시에 실행함.
        }
    }
}

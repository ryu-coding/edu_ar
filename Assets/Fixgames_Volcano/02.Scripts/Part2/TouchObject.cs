using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fixgames.Volcano
{
    public class TouchObject : MonoBehaviour
    {
        
        /// <summary>
        /// 화산재, 용암 같은 텍스쳐 object를 클릭시 발생하는 Script.
        /// </summary>

        AudioSource audioSource;
        public AudioClip[] audioClips;
        AudioSource AudioSource;
        

        void Start()
        {
            AudioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray,out hit))
                {
                    ///<summary>
                    ///touch한 오브젝트가 null이 아니면 
                    ///해당하는 태그 검색해 해당하는 사운드도 출력
                    ///</summary>
                    if (hit.transform != null) 
                    {
                        if (hit.transform.gameObject.tag == "Ash")
                        {
                            
                            StartCoroutine(AudioPlay(0));
                        }
                            
                        if (hit.transform.gameObject.tag == "Lava")
                        {
                            StartCoroutine(AudioPlay(1));
                        }
                           
                        if (hit.transform.gameObject.tag == "Rock")
                        {
                            StartCoroutine(AudioPlay(2));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 해당 사운드를 출력하는 코루틴.
        /// </summary>

        IEnumerator AudioPlay(int n)
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.clip = audioClips[n];
                AudioSource.Play();
            }
            yield return new WaitForSeconds(0.5f); 
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Fixgames.Volcano
{
    public class Quiz1 : MonoBehaviour
    {
        public Text myText1;
        public GameObject controller, ReviewTest1, ReviewTest2;
        public bool onClick1, onClick2, onClick3, onClick4;
        // Use this for initialization
        private void Awake()
        {
        }
        void Start()
        {
            myText1.enabled = false;
            onClick1 = false;
            onClick2 = false;
            onClick3 = false;
            onClick4 = false;
        }
        public void SetClick1(bool on)
        {
            onClick1 = on;
        }

        public void SetClick2(bool on)
        {
            onClick2 = on;
        }

        public void SetClick3(bool on)
        {
            onClick3 = on;
        }

        public void SetClick4(bool on)
        {
            onClick4 = on;
        }
        public bool SetClick1()
        {
            return onClick1;
        }
        public bool SetClick2()
        {
            return onClick2;
        }
        public bool SetClick3()
        {
            return onClick3;
        }
        public bool SetClick4()
        {
            return onClick4;
        }


        // Update is called once per frame
        void Update()
        {
            // 확인
            onClick1 = controller.GetComponent<Quiz1>().SetClick1();
            onClick2 = controller.GetComponent<Quiz1>().SetClick2();
            onClick3 = controller.GetComponent<Quiz1>().SetClick3();
            onClick4 = controller.GetComponent<Quiz1>().SetClick4();
            // 모두 true이면 다음단계
            if (onClick1 == true && onClick2 == true && onClick3 == true && onClick4 == true)
            {
                StartCoroutine(Wait());
            }
        }
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(2.0f);
            ReviewTest1.SetActive(false);
            ReviewTest2.SetActive(true);
            GameObject.Find("AudioManager").GetComponent<AudioManager>().count = 28;
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
        }
        // 첫번째 문제 터치시
        public void OnClick1()
        {
            GameObject.Find("AudioManager").GetComponent<AudioPlayer>().SuccessPlay();
            controller.SendMessage("SetClick1", true, SendMessageOptions.DontRequireReceiver);
            myText1.enabled = true;

        }
        // 두번째 문제 터치시
        public void OnClick2()
        {
            GameObject.Find("AudioManager").GetComponent<AudioPlayer>().SuccessPlay();
            controller.SendMessage("SetClick2", true, SendMessageOptions.DontRequireReceiver);
            myText1.enabled = true;

        }

        // 세번째 문제 터치시
        public void OnClick4()
        {
            GameObject.Find("AudioManager").GetComponent<AudioPlayer>().SuccessPlay();
            controller.SendMessage("SetClick4", true, SendMessageOptions.DontRequireReceiver);
            myText1.enabled = true;

        }
        // 네번째 문제 터치시
        public void OnClick3()
        {
            GameObject.Find("AudioManager").GetComponent<AudioPlayer>().SuccessPlay();
            controller.SendMessage("SetClick3", true, SendMessageOptions.DontRequireReceiver);
            myText1.enabled = true;
           
        }
    }
}

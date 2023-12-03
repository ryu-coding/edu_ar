using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class ControllerNoAr : MonoBehaviour
    {
        [Header("Menu Toggle")] //메뉴 버튼
        public GameObject CaptureButton;
        public GameObject LookButton;
        public GameObject DetailsButton;

        [Header("Part1,2,3")]
        //Part 1 팝업
        public GameObject popup;
        //오브젝트를 생성할 anchor
        public GameObject anchor;
        //오브젝트를 생성했는지 저장할 bool
        private bool create;

        [Header("Part1")]
        //첫번째 팝업
        public GameObject Prefab1;
        //실험도구 프리팹
        public GameObject Prefab2;
        //DragManager 스크립트
        public GameObject DragMgr;
        //실험도구 오브젝트
        public GameObject ObjectP1;

        [Header("Part2")]
        //화산 오브젝트
        public GameObject VolcanoObject;
        public GameObject objectCheck;
        public GameObject volcnoText;
        //화산 이름 UI
        public GameObject jet;
        public GameObject[] jetTag;

        [Header("LoadingBar")]
        private bool showSearchingUI;
        private bool ShowExit = false;

        public void SetShowExit(bool ex)
        {
            ShowExit = ex;
        }

        public bool CreateObject()
        {
            return create;
        }

        public void SetActiveVolcano(bool b)
        {
            VolcanoObject.SetActive(b);
        }

        public void Update()
        {
            _UpdateApplicationLifecycle();
            if (!create)
            {
                create = true;
                //팝업창 생성
                Prefab1.SetActive(true);    // popup 생성
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();

                //실험도구 위치조정
                Prefab2.transform.localPosition = new Vector3(0, -0.05f, 0.2f);
                Prefab2.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                //오브젝트 생성
                ObjectP1 = Instantiate(Prefab2);
                //anchor 자식으로 생성
                ObjectP1.transform.parent = anchor.transform;
                ObjectP1.SetActive(false);

                showSearchingUI = false;
            }
        }

        //Part2로 넘어왔을 때 화산 오브젝트 생성 및 anchor 자식 오브젝트 제거
        public void ChangeObject()
        {
            Transform trChild;
            if (objectCheck.GetComponent<Close>().check == true)
            {
                VolcanoObject.GetComponent<CreateBtn>().Reset();
                VolcanoObject.SetActive(true);
                volcnoText.SetActive(true);
                VolcanoObject.transform.position = new Vector3(Prefab2.transform.position.x, Prefab2.transform.position.y - 0.0436f, Prefab2.transform.position.z);
                VolcanoObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                jet.SetActive(true);

                VolcanoObject.GetComponent<Particle>().StartParticle();
                Prefab2.SetActive(false);
            }
            for (int i = 0; i < anchor.transform.childCount; i++)
            {
                trChild = anchor.transform.GetChild(i);
                Destroy(trChild.gameObject);
            }
        }

        //Part1으로 넘어왔을때 anchor 자식 오브젝트 제거 및 실험도구 재생성
        public void ChangeObjectPart1()
        {
            Transform trChild;
            for (int i = 0; i < anchor.transform.childCount; i++)
            {
                trChild = anchor.transform.GetChild(i);
                Destroy(trChild.gameObject);
            }
            DragMgr.GetComponent<DragObject>().SetExperimentTurn(1);
            ObjectP1 = Instantiate(Prefab2);
            ObjectP1.transform.parent = anchor.transform;
            ObjectP1.SetActive(false);
            VolcanoObject.SetActive(false);
        }

        private void _UpdateApplicationLifecycle()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        public void _DoQuit()
        {
            Application.Quit();
        }

        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }

        //메뉴버튼 눌렀을때 자식 메뉴 활성화 비활성화
        public void MenuButton()
        {
            int num = GameObject.Find("StageManager").GetComponent<Stage>().getStageNum();
            if (num != 3)
            {
                CaptureButton.SetActive(!CaptureButton.activeSelf);
                LookButton.SetActive(!LookButton.activeSelf);
                DetailsButton.SetActive(!DetailsButton.activeSelf);
            }
            else
            {
                CaptureButton.SetActive(false);
                LookButton.SetActive(false);
                DetailsButton.SetActive(false);
            }
        }
    }
}

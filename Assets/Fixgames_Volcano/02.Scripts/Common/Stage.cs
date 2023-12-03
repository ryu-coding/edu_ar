using GoogleARCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Fixgames.Volcano
{
    public class Stage : MonoBehaviour
    {
        [Header("StageImage")]
        // Part1
        public GameObject First;
        // Part2
        public GameObject Second;
        // Part3
        public GameObject Third;
        // 기본 투명 Image
        public Sprite Defaultimage;
        // Part1 이미지
        public Sprite FirstSprite;
        // Part2 이미지
        public Sprite SecondSprite;
        // Part3 이미지
        public Sprite ThirdSprite;
        // 메뉴 
        public Image Menu;
        // 메뉴 비활성화 이미지
        public Sprite MenubuttonNo;
        // 메뉴 활성화 이미지
        public Sprite Menubutton;

        [Header("Part")]
        // Replay 열려있는지 확인
        public Replace rp;
        // 단계 구분
        public int StageNum = 1;
        // Part1
        public GameObject Part1;
        // Part2
        public GameObject Part2;
        // Part3
        public GameObject Part3;

        [Header("Part1")]
        // PlaneVisual
        public GameObject planeVisual;
        // Controller 스크립트
        private Controller controller;
        // ControllerNoAR 스크립트
        private ControllerNoAr controllerNoAr;
        // Part1 total Popup
        public GameObject Part1Popup;
        // Popup1
        public GameObject Popup1;
        // Popup2
        public GameObject popup2;
        // Popup2 이전 버튼
        public GameObject preButton;
        // Popup2 다음 버튼
        public GameObject NextButton;
        // Popup2 확인버튼
        public GameObject configButton;
        // Popup2 실험시작 Text
        public Text NextText;
        // 경고 이미지
        public GameObject DangerImage;
        // 실험 설명 Popup
        public GameObject experimentText;
        // 실험 설명 - Waring image
        public GameObject WaringImage;
        // 실험 결과창
        public GameObject result;
        // DragObject GameObject
        public GameObject DragObject;
        // DragObject 스크립트
        public DragObject dragobject;
        // PopupManager 스크립트
        public PopupManager popupmanager;
        // 버튼 오브젝트
        public GameObject MenuButton, QuitButton, ExplanationButton;
        //nameTag
        public GameObject nameTag;
        //nameTag 텍스트 Color
        public Color textColor;
        // 네임테그 오브젝트
        public GameObject[] nameTagList;
        // 네임태그 Text
        public Text[] nameText;


        [Header("Part2")]
        //Part2 Popup1
        public GameObject Prat2StartPopup;
        public GameObject Part2View;
        public GameObject Part2EndPopup;
        public GameObject checkobj;
        public GameObject jet;
        public GameObject resetPopup;
        // Part2 Sound
        public GameObject[] stage2Sound;
        public GameObject volcano;

    
        [Header("Review1")]
        //Part3-1
        public GameObject ReviewTest1;
        // Text
        public List<Text> listReview1Text;
        // Button1 Controller
        public GameObject QuizController;

        [Header("Review2")]
        //Part3-2
        public GameObject ReviewTest2;
        // 초기화 컨트롤러
        public GameObject prefabReview2Container;
        // 텍스트
        public List<Text> listReview2Text;
        // 재배치중 단계이동 제한

        public void Awake()
        {
            StageNum = 1;
        }
        public void Start()
        {
            controller = GameObject.Find("Controller").GetComponent<Controller>();
            controllerNoAr = GameObject.Find("Controller").GetComponent<ControllerNoAr>();
        }

        public void Back()
        {
            // 단계별 되돌리기
            if(StageNum == 2)
            {
                Stage1();
            }
            if(StageNum == 3)
            {
                Stage2();
            }
        }
        // StageNumber 반환
        public int getStageNum()
        {
            return StageNum;
        }

        //Part1
        public void Stage1()
        {
            if(GetCreate() && rp.play == false)
            {
                nameTag.SetActive(false);
                if (controller != null)
                    planeVisual.SetActive(true);
                jet.SetActive(false);
                volcano.GetComponent<Particle>().AllStop();
                // Part2 Sound 모두 멈춤
                if (StageNum == 2)
                {
                    if (stage2Sound[0].activeInHierarchy)
                        stage2Sound[0].GetComponent<StartAudio>().AudioSource.Stop();
                    if (stage2Sound[1].activeInHierarchy)
                        stage2Sound[1].GetComponent<EndAudio>().AudioSource.Stop();
                    if (stage2Sound[2].activeInHierarchy)
                        stage2Sound[2].GetComponent<Particle>().AudioSource.Stop();
                }
                //nameTag 활성화 및 Text 색 변경
                for (int i = 0; i < nameTagList.Length; i++)
                {
                    if (nameText[i] != null && nameTagList[i] != null)
                    {
                        nameTagList[i].SetActive(true);
                        nameText[i].color = textColor;
                    }
                }
                // 이전버튼 초기화
                preButton.SetActive(false);
                // 보안경, 장갑 경고 이미지 초기화
                DangerImage.SetActive(false);
                // StageNum 초기화 
                StageNum = 1;
                // tutorial 비활성화
                dragobject.tutorial = false;
                //DragObject 비활성화
                DragObject.SetActive(false);
                // 포일접기 비활성화
                dragobject.blendStart = false;
                // 나래이션 초기화
                GameObject.Find("AudioManager").GetComponent<AudioManager>().count = 0;
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
                // 기존 팝업 비활성화
                int iCount = Part1Popup.transform.childCount;
                for (int i = 0; i < iCount; i++)
                {
                    Transform trChild = Part1Popup.transform.GetChild(i);
                    trChild.gameObject.SetActive(false);
                }
                // 메뉴 활성화 이미지
                Menu.GetComponent<Image>().sprite = Menubutton;
                //Stage 이미지 초기화
                First.GetComponent<Image>().sprite = FirstSprite;
                Second.GetComponent<Image>().sprite = Defaultimage;
                Third.GetComponent<Image>().sprite = Defaultimage;
                // 실험 도구 오브젝트 생성
                if(controller != null)
                {
                    controller.ChangeObjectPart1();
                }
                else if(controllerNoAr != null)
                {
                    controllerNoAr.ChangeObjectPart1();
                }
                else
                {
                    // Do Nothiing
                }
                // 실험 도구 확대 사이즈 초기화
                dragobject.ResetScale();
                // 실험 순서 초기화
                dragobject.experimentTurn = 1;
                // Popup2 준비물 순서 초기화
                popupmanager.count = 1;
                // Popup2 다음버튼
                NextButton.SetActive(true);
                // popup2 확인버튼
                configButton.SetActive(false);
                // popup2 마지막 Text
                NextText.text = "";
              
                // Popup1 시작
                Popup1.SetActive(true);
                // 초기화
                Part1.SetActive(true);
                Part2.SetActive(false);
                Part3.SetActive(false);
                // 실험 설명 비활성화
                experimentText.SetActive(false);
                // 실험설명 경고이미지 비활성화
                WaringImage.SetActive(false);
                // 실험결과 비활성화
                result.SetActive(false);
            }
        }

        public void Stage2()
        {

            // Audio 초기화
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Stop();
            
            // DragObject 비활성화
            if (DragObject.activeSelf == true)
            {
                DragObject.SetActive(false);
                MenuButton.SetActive(true);
                QuitButton.SetActive(true);
                ExplanationButton.SetActive(true);
            }
            if(GetCreate() && rp.play == false)
            {
                volcano.SetActive(false);
                volcano.GetComponent<Particle>().AllStop();
                nameTag.SetActive(false);
                jet.SetActive(false);
                // 메뉴이미지 활성화
                Menu.GetComponent<Image>().sprite = Menubutton;
                // StageNum 초기화
                StageNum = 2;
                // StageImage 초기화
                First.GetComponent<Image>().sprite = Defaultimage;
                Second.GetComponent<Image>().sprite = SecondSprite;
                Third.GetComponent<Image>().sprite = Defaultimage;
                // Part 초기화
                Part1.SetActive(false);
                Part2.SetActive(true);
                Part3.SetActive(false);

                //Part2의 화면 초기화
                checkobj.GetComponent<Close>().SetCheck(false);
                Prat2StartPopup.SetActive(true);
                Part2View.SetActive(false);
                Part2EndPopup.SetActive(false);
                Part2View.GetComponent<VolcanoView>().ResetObject();
                resetPopup.SetActive(false);

            }

            ///<summary>
            /// 확대 축소 시 AR 과 noAR여부 확인 및 인자 전달.
            ///</summary>

            PinchZoom pinchZoom = GameObject.Find("Controller").GetComponent<PinchZoom>();
            if(pinchZoom != null)
            {
                pinchZoom.arMode = (controller != null);
                pinchZoom.SetStart(true);
            }
            
        }

        public void Stage3()
        {
            if (GetCreate() && rp.play == false)
            {
                nameTag.SetActive(false);
                jet.SetActive(false);
                volcano.GetComponent<Particle>().AllStop();
                if (controller != null)
                {
                    //메뉴버튼 Child 오브젝트 비활성화
                    controller.MenuButton();
                }
                else
                {
                    controllerNoAr.MenuButton();
                }
                //Stage 버튼 이미지
                First.GetComponent<Image>().sprite = Defaultimage;
                Second.GetComponent<Image>().sprite = Defaultimage;
                Third.GetComponent<Image>().sprite = ThirdSprite;
                //메뉴버튼 비활성화 이미지
                Menu.GetComponent<Image>().sprite = MenubuttonNo;
                // 이전 Sound 모두 멈춤
                if (StageNum == 2)
                {
                    if (stage2Sound[0].activeInHierarchy)
                        stage2Sound[0].GetComponent<StartAudio>().AudioSource.Stop();
                    if(stage2Sound[1].activeInHierarchy)
                        stage2Sound[1].GetComponent<EndAudio>().AudioSource.Stop();
                    if (stage2Sound[2].activeInHierarchy)
                        stage2Sound[2].GetComponent<Particle>().AudioSource.Stop();
                }
                // 화산모형 비활성화
                if (controller != null)
                {
                    controller.SetActiveVolcano(false);
                }
                else if (controllerNoAr != null)
                {
                    controllerNoAr.SetActiveVolcano(false);
                }
                else
                {
                    // Do Nothiing
                }

                // Sound 초기화
                GameObject.Find("AudioManager").GetComponent<AudioManager>().count = 23;
                GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();

                // Stage 초기화
                StageNum = 3;

                if (controller != null)
                {
                    //메뉴버튼 Child 오브젝트 비활성화
                    controller.MenuButton();
                }
                else
                {
                    controllerNoAr.MenuButton();
                }

                // 3-1 초기화
                Part1.SetActive(false);
                Part2.SetActive(false);
                Part3.SetActive(true);
                ReviewTest1.SetActive(true);
                ReviewTest2.SetActive(false);

                // Part3-1 초기화
                QuizController.GetComponent<Quiz1>().SetClick1(false);
                QuizController.GetComponent<Quiz1>().SetClick2(false);
                QuizController.GetComponent<Quiz1>().SetClick3(false);
                QuizController.GetComponent<Quiz1>().SetClick4(false);
                // Part3 Text 초기화
                for (int i = 0; i < listReview1Text.Count; i++)
                {
                    listReview1Text[i].enabled = false;
                }

                // 3-2 초기화
                for(int ii = 0; ii < listReview2Text.Count; ++ii)
                {
                    listReview2Text[ii].text = "";
                }

                // 3-2 컨테이너 초기화
                for (int jj = 0; jj < ReviewTest2.transform.childCount; ++jj)
                {
                    if(ReviewTest2.transform.GetChild(jj).gameObject.name.Equals(prefabReview2Container.name)
                        || ReviewTest2.transform.GetChild(jj).gameObject.name.Equals(prefabReview2Container.name + "(Clone)"))
                    {
                        RectTransform originalTransform = ReviewTest2.transform.GetChild(jj).gameObject.GetComponent<RectTransform>();

                        GameObject obj = GameObject.Instantiate(prefabReview2Container);
                        obj.transform.SetParent(ReviewTest2.transform);

                        RectTransform newTransform = obj.GetComponent<RectTransform>();
                        newTransform.anchorMin = originalTransform.anchorMin;
                        newTransform.anchorMax = originalTransform.anchorMax;
                        newTransform.anchoredPosition = originalTransform.anchoredPosition;
                        newTransform.sizeDelta = originalTransform.sizeDelta;

                        Destroy(ReviewTest2.transform.GetChild(jj).gameObject);
                        break;
                    }
                }
            }
        }

        // Part1에서 Object 생성됬는지 확인
        public bool GetCreate()
        {
            bool bCreate = false;
            if(controller != null)
            {
                bCreate = controller.CreateObject();
            }
            else if(controllerNoAr != null)
            {
                bCreate = controllerNoAr.CreateObject();
            }
            else
            {
                // Do Nothing
            }
            // 생성여부 반환
            return bCreate;
        }
        
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if(unityActivity != null)
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
    }
}
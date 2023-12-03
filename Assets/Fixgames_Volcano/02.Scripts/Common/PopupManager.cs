using UnityEngine;
using UnityEngine.UI;

namespace Fixgames.Volcano
{
    public class PopupManager : MonoBehaviour
    {
        [Header("Popup")]
        // Part1 Popup1
        public GameObject pop1;
        // Part1 Popup2
        public GameObject pop2;
        // Part1 Popup3
        public GameObject pop3;
        // 종료창
        public GameObject QuitPopup;
        // 설명하기 창
        public GameObject ExplanationPopup; 
        // 설명하기 버튼 눌렸을때 종료하기 반응 X
        bool ShowQuestion;
        // 종료하기 버튼 눌렸을때 설명하기 반응 X
        bool ShowExit;
        // NameTag
        public GameObject nameTags;

        [Header("Image")]
        // 준비물 왼쪽 이미지
        public Image Image1;
        // 준비물 오른쪽 이미지
        public Image Image2; 
        // 준비물 이미지 배열
        public Sprite[] sprites;

        [Header("GameObject")]
        // DragObject GameObject
        public GameObject DragandDrop;
        // 실험도구 Object
        public GameObject experimentObject;
        // Controller Object
        public GameObject controller; 
        // DragObject 스크립트
        public DragObject dr;

        [Header("Button")]
        // Popup2 시작하기 버튼
        public GameObject button;
        // Popup2 >버튼(다음)
        public GameObject NextButton;
        // Popup2 >버튼(이전)
        public GameObject preButton;

        [Header("Text")]
        // Popup2 왼쪽 준비물 Text
        public Text text1;
        // Popup2 오른쪽 준비물 Text
        public Text text2;
        // Popup2 마지막 Text
        public Text text3;
        // Popup2 준비물 Count
        public int count = 0;
        // 단계 구분
        int stageNum;
        // Part1 실험 순서
        int experimentTurn;


        private void Start()
        {
            // 단계 1단계이면
            stageNum = GameObject.Find("StageManager").GetComponent<Stage>().getStageNum();
            if(stageNum == 1)
            {
                // 준비물 첫번째 장부터 시작
                count = 1;
                Image1.GetComponent<Image>().sprite = sprites[(count * 2) - 2];
                Image2.GetComponent<Image>().sprite = sprites[(count * 2) - 1];
            }
            // 그 이외엔 0으로 초기화
            else
            { 
                count = 0;
            }
        }
        // Update is called once per frame
        void Update()
        {
            // count가 1보다 크고 5보다 작으면
            if (count >= 1 && count <= 5)
            {
                // 이미지 렌더링
                Image1.GetComponent<Image>().sprite = sprites[(count * 2) - 2];
                Image2.GetComponent<Image>().sprite = sprites[(count * 2) - 1];
                //준비물 텍스트
                if (count == 1)
                {
                    text1.text = "알루미늄 포일";
                    text2.text = "마시멜로 여러 개";
                }
                else if (count == 2)
                {
                    text1.text = "식용 색소 (빨간색)";
                    text2.text = "은박 접시";
                }
                else if (count == 3)
                {
                    text1.text = "삼발이";
                    text2.text = "알코올램프";
                }
                else if (count == 4)
                {
                    text1.text = "점화기";
                    text2.text = "보안경";
                    text3.text = "";
                }
                else if (count == 5)
                {

                    text1.text = "장갑 (두꺼운 것)";
                    text3.text = "실험을 시작해 볼까요?";
                    text2.text = "";
                }
            }
        }

        public void popup1()
        {
            // Popup1 시작 나레이션
            if (GameObject.Find("AudioManager").GetComponent<AudioManager>().count < 3)
                GameObject.Find("AudioManager").GetComponent<AudioManager>().count = 3;
            // 버튼 눌렸을때 현재 팝업 비활성화
            pop1.SetActive(false);
            // 다음 팝업 활성화
            pop2.SetActive(true);
            // 다음 나래이션 Play
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
        }

        // Popup2 다음 버튼
        public void next()
        {
            
            count++;
            if(count == 2)
            {
                preButton.SetActive(true);
            }
            // count가 5일때 다음 버튼이 비활성화 되고 시작하기 버튼 활성화된다
            if(count == 5)
            {
                button.SetActive(true);
                NextButton.SetActive(false);
            }
            // 5가 초과할수 없다
            else if(count == 6)
            {
                count--;
            }
            else
            {
                // Do Nothing
            }
        }

        public void pre()
        {
            // 1보다 작아질수 없다
            if(count == 1)
            {
                return;
            }
            if(count == 2)
            {
                preButton.SetActive(false);
            }
            // count가 4가 되면 시작하기버튼 비활성화
            // 다음 버튼 활성화
            else if(count == 5)
            {
                button.SetActive(false);
                NextButton.SetActive(true);
            }
            count--;

        }

        // 시작하기 버튼
        public void StartStage()
        {
            count++;
            // Popup3 활성화
            pop3.SetActive(true);
            // Popup3 나래이션 
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
            //Popup2 비활성화
            pop2.SetActive(false);
        }

        public void popup3()
        {
            nameTags.SetActive(true);
            // DragObject GameObject 활성화
            DragandDrop.SetActive(true);
            // 실험도구 GameObject 활성화
            // AR모드
            Controller controllerScript = controller.GetComponent<Controller>();
            if(controllerScript != null)
            {
                controllerScript.ObjectP1.SetActive(true);
            }
            // No_AR모드
            ControllerNoAr controllerNoArScript = controller.GetComponent<ControllerNoAr>();
            if(controllerNoArScript != null)
            {
                controllerNoArScript.ObjectP1.SetActive(true);
            }

            // 실험 시작 나래이션
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
            // 실험순서 1로 초기화
            GameObject.Find("DragManager").GetComponent<DragObject>().experimentTurn = 1;
            // Popup3 비활성화
            pop3.SetActive(false);
        }

        // 종료버튼
        public void Exit()
        {
            // 설명하기가 활성화 안되있을 때
            if (!ShowQuestion)
            {
                ShowExit = true;
                // 로딩바가 돌아가고있으면 로딩바 비활성화
                if (stageNum == 1)
                {
                    if (GameObject.Find("Controller").GetComponent<Controller>() != null)
                    {
                        controller.GetComponent<Controller>().SetShowExit(true);
                    }
                    else
                    {
                        controller.GetComponent<ControllerNoAr>().SetShowExit(true);
                    }
                }
                // 종료창 활성화
                QuitPopup.SetActive(true);
            }
        }

        public void CancelExit()
        {
            ShowExit = false;
            // 로딩바 다시 활성화
            if (stageNum == 1)
            {
                if (GameObject.Find("Controller").GetComponent<Controller>() != null)
                {
                    controller.GetComponent<Controller>().SetShowExit(false);
                }
                else
                {
                    controller.GetComponent<ControllerNoAr>().SetShowExit(false);
                }
            }
            // 종료창 비활성화
            QuitPopup.SetActive(false);
        }

        public void _DoExit()
        {
            Application.Quit();
        }

        public void Question()
        {
            // 종료창 비활성화시
            if(!ShowExit)
            {
                ShowQuestion = true;
                // Part1이면 로딩바 비활성화
                if (stageNum == 1)
                {
                    if (GameObject.Find("Controller").GetComponent<Controller>() != null)
                    {
                        controller.GetComponent<Controller>().SetShowExit(true);
                    }
                    else
                    {
                        controller.GetComponent<ControllerNoAr>().SetShowExit(true);
                    }
                }
                // 설명하기창 활성화
                ExplanationPopup.SetActive(true);
            }
        }

        public void ExitQuestion()
        {
            ShowQuestion = false;
            // 로딩바 다시 활성화
            if (stageNum == 1)
            {
                if (GameObject.Find("Controller").GetComponent<Controller>() != null)
                {
                    controller.GetComponent<Controller>().SetShowExit(false);
                }
                else
                {
                    controller.GetComponent<ControllerNoAr>().SetShowExit(false);
                }
            }
            // 설명하기창 비활성화
            ExplanationPopup.SetActive(false);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fixgames.Volcano
{
    public class DragObject : MonoBehaviour
    {
        [Header("ExperimentObjectDrag")]
        // 실험 순서 및 실험도구 이동 순서
        public int experimentTurn = 0;
        // 포일 + 마시멜로
        public GameObject MarshFoil;
        // 포일 + 마시멜로 + 색소 
        public GameObject MarshFoil_Coloring;
        // 은박접시 + 접힌 포일
        public GameObject silver_Plate;
        // 삼발이 + 알코올 램프
        public GameObject Alcohollamp_Three_Leg;
        // 최종 실험 단계 실험도구
        public GameObject TotalExperiment;
        // 포일 접기 시작 시점
        public bool blendStart = false;
        // Point Object 저장
        public GameObject Point1;
        public GameObject Point2;
        public GameObject Point3;
        // Point 재생성
        GameObject po, po1, po2;
        // 포일 접기 제스쳐
        bool fold1, fold2;
        private float BlendNum; // 포일 / 용암
        // 실험도구 드래그 중인지 확인
        bool isMouseDragging;
        // 실험도구 Trigger됫는지 확인
        bool deleteObject = false;
        // 튜토리얼
        public bool tutorial = true;
        // 터치한 실험도구
        GameObject getTarget;
        // 실험도구 이름
        GameObject TextName;
        // 실험도구 Point
        GameObject point;
        // 충돌된 다른 실험도구
        GameObject TriggerObject;
        // 실험도구 이동
        Vector3 offsetValue;
        Vector3 positionOfScreen;
        Vector3 _mouseOffset;
        Vector3 _mouseReference;
        // 실험도구 처음 위치
        Vector3 firstPosition;
        // 오브젝트 터치 여부 확인
        RaycastHit hitobject;
        // 연기 / 불 파티클
        private ExperimentParticle experimentParticle;
        // 버튼
        public GameObject MenuButton, QuitButton, ExplanationButton;
        //NameTag
        public GameObject[] nameTag;
        public Text[] nameText;

        [Header("Size & Rotation")]
        // 실험도구 크기 조정
        public float fscale = 1;
        // 확대, 축소
        float perspectiveZoomSpeed = 1f;
        // 실험도구 회전
        float _sensitivity = 0.1f;
        // 회전
        Vector3 _rotation = Vector3.zero;

        [Header("Sound")]
        // 나래이션 끝났는지 확인
        bool SoundEnd = true;
        // 나래이션
        AudioSource audioPlayer;

        [Header("Popup")]
        //마지막 실험 결과 창
        public GameObject ResultPrefab;
        //  실험 설명 Popup
        public GameObject popup;
        public Text text;
        // 목장갑 보안경 경고 이미지
        public GameObject WaringPrefab;
        public GameObject DangerImage;

        [Header("Controller")]
        // 컨트롤러 - AR지원
        public Controller controller;
        // 컨트롤러 - AR지원X
        public ControllerNoAr controllerNoAr;


        // 변수 Set/Get 메소드
        //충돌한 실험도구 받아오는 함수
        public void SetColliderObject(GameObject trigger)
        {
            TriggerObject = trigger;
        }
        // 마우스가 Drag중인지 확인하는 함수
        public bool MouseDragging()
        {
            return isMouseDragging;
        }
        // 터치한 실험도구를 반환하는 함수
        public GameObject Target()
        {
            return getTarget;
        }
        // 실험도구가 다른 실험도구와 Trigger됬는지 확인하는 함수
        public void SetDeleteObject(bool collider)
        {
            deleteObject = collider;
        }
        // 포일이 다 접히고 마시멜로가 다 흘러 나왔는지 확인하는 함수
        public void SetblendNum(float BlendNum)
        {
            this.BlendNum = BlendNum;
        }
        // 현재 실험의 순서를 반환하는 함수
        public int GetExperimentTurn()
        {
            return experimentTurn;
        }
        // 실험순서를 변경하는 함수
        public void SetExperimentTurn(int count)
        {
            experimentTurn = count;
        }
        // 포일접기 시작하는 시점을 알려주는 함수
        public bool BlendStart()
        {
            return blendStart;
        }

        private void Start()
        {
            ResetScale();
            audioPlayer = GameObject.Find("AudioManager").GetComponent<AudioManager>().AudioPlayer();
        }
        void Update()
        {
            // 실험 단계별 설명 TEXT
            PopupTextMeThod();
            // 튜토리얼 || 포인트
            if (tutorial == true)
                PointObject();
            // NameTag 색 변경
            NameTextColor();
            // 10~13번째 순서의 Object 사운드 및 애니메이션
            AnimationPlayer();
            // Size up & down
            if (Input.touchCount == 2 && experimentTurn != 3)
            {
                if (getTarget != null)
                {
                    if (getTarget.tag.Equals("4") || getTarget.tag.Equals("5"))
                    {
                        getTarget.transform.localScale -= new Vector3(0.35f, 0.02f, 0.35f);
                    }
                    else
                    {
                        getTarget.transform.localScale -= new Vector3(0.35f, 0.35f, 0.35f);
                    }
                    getTarget.transform.position = firstPosition;
                    getTarget = null;
                }

                isMouseDragging = false;
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitoudeDiff = prevTouchDeltaMag - touchDeltaMag;
                float tempscale = fscale + (deltaMagnitoudeDiff * perspectiveZoomSpeed / (-100));
                tempscale = Mathf.Clamp(tempscale, 0.1f, 6f);
                fscale = Mathf.Lerp(fscale, tempscale, 5 * Time.deltaTime);

                if (fscale > 1.15f)
                    fscale = 1.15f;

                if (controller != null)
                {
                    controller.ObjectP1.transform.localScale = new Vector3(fscale, fscale, fscale);
                }
                if (controllerNoAr != null)
                {
                    controllerNoAr.ObjectP1.transform.localScale = new Vector3(fscale, fscale, fscale);
                }
            }

            // Drag - 처음 눌렸을 때
            else if (Input.GetMouseButtonDown(0))
            {
                // UI 터치시 Unity상에 영향 X
#if !UNITY_EDITOR
                if(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
                if (!EventSystem.current.IsPointerOverGameObject())
#endif
                {
                    if (blendStart == true)
                    {
                        return;
                    }
                    getTarget = ReturnClickedObject(out hitobject);
                    if (getTarget != null)
                    {
                        // Drag X -> 클릭
                        if ((experimentTurn == 6 && getTarget.tag.Equals("6")))
                        {
                            Experiment();
                            return;
                        } // 포일 접기 Collider 이동 X
                        if (getTarget.tag.Equals("fold1") || getTarget.tag.Equals("fold2"))
                        {
                            return;
                        }
                        // 접시, 접시 + 포일 터치시 크기 
                        if (getTarget.tag.Equals("4") || getTarget.tag.Equals("5"))
                        {
                            getTarget.transform.localScale += new Vector3(0.35f, 0.02f, 0.35f);
                        }
                        // 나머지 실험도구 터치했을때 크기
                        else
                        {
                            getTarget.transform.localScale += new Vector3(0.35f, 0.35f, 0.35f);
                        }
                        // 원래 위치 저장
                        firstPosition = getTarget.transform.position;

                        isMouseDragging = true;
                        positionOfScreen = Camera.main.WorldToScreenPoint(getTarget.transform.position);
                        offsetValue = getTarget.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
                    }
                    else
                    {
                        _mouseReference = Input.mousePosition;
                    }
                }
            }

            // Drag 중일 때
            else if (isMouseDragging)
            {
                Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;
                currentPosition.y = getTarget.transform.position.y;
                getTarget.transform.position = currentPosition;
            }

            //NoAR 회전
            else if (controllerNoAr != null && _mouseReference != Vector3.zero)
            {
                _mouseOffset = (Input.mousePosition - _mouseReference);
                _rotation.x = -_mouseOffset.y * _sensitivity;
                _rotation.y = -_mouseOffset.x * _sensitivity;
                Camera.main.transform.RotateAround(controllerNoAr.ObjectP1.transform.position, Vector3.right, _rotation.x);
                controllerNoAr.ObjectP1.transform.Rotate(new Vector3(0, _rotation.y, 0));
                _mouseReference = Input.mousePosition;
            }

            // Drag 종료
            if (Input.GetMouseButtonUp(0))
            {
                isMouseDragging = false;
                _mouseReference = Vector3.zero;
                if (getTarget != null)
                {
                    // 터치 종료시 원래 크기로 되돌림
                    if (getTarget.tag.Equals("4") || getTarget.tag.Equals("5"))
                    {
                        getTarget.transform.localScale -= new Vector3(0.35f, 0.02f, 0.35f);
                    }
                    else
                    {
                        getTarget.transform.localScale -= new Vector3(0.35f, 0.35f, 0.35f);
                    }

                    // Drag 후 Trigger 안됬을 때 원래 자리로 되돌림
                    if (deleteObject == false)
                    {
                        getTarget.transform.position = firstPosition;
                    }
                    // Trigger 됬을 때
                    else
                    {
                        Experiment();
                    }
                    getTarget = null;
                }

            }

            // 3단계 TwoFingerGesture
            if (experimentTurn == 3)
            {
                TwoFingerGesture();
                // TwoFingerGesture 메소드에서 ture가 되면 포일을 접기 시작하고
                // 다 접어지면 다음단계로 이동
                if (BlendNum == 99)
                {
                    // 접힌 포일
                    GameObject obj = GameObject.FindWithTag("3");
                    // 태그 변경
                    obj.tag = "4";
                    // 접기 collider 삭제
                    Destroy(GameObject.FindWithTag("fold1"));
                    Destroy(GameObject.FindWithTag("fold2"));
                    // 포인트 삭제
                    Destroy(po1);
                    Destroy(po2);
                    blendStart = false;
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
                    experimentTurn++;
                    BlendNum = 0;
                }
            }
        }

        // NameTag 색 변경
        void NameTextColor()
        {
            if (experimentTurn >= 1 && experimentTurn <= 7)
            {
                if (GameObject.FindWithTag("name" + experimentTurn) != null)
                {
                    nameText[experimentTurn].color = new Color(236.0f, 0.0f, 200.0f);
                }
            }
        }

        // TwoFingerGesture
        void TwoFingerGesture()
        {
            if (Input.touchCount == 2)
            {
                Ray ray1 = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                Ray ray2 = Camera.main.ScreenPointToRay(Input.GetTouch(1).position);

                if (Physics.Raycast(ray1, out hitobject))
                {
                    if (hitobject.collider.tag == "fold1" || hitobject.collider.tag == "fold2")
                    {
                        fold1 = true;
                    }
                }
                if (Physics.Raycast(ray2, out hitobject) && fold1 == true)
                {
                    if (hitobject.collider.tag == "fold1" || hitobject.collider.tag == "fold2")
                    {
                        fold2 = true;
                    }
                }

                if (fold2 == true || fold1 == true)
                {
                    blendStart = true;
                }
            }
#if !UNITY_EDITOR
            else
            {
                fold1 = false;
                fold2 = false;
            }
#else
            blendStart = true;
#endif
        }


        // 단계별 object 실험
        void Experiment()
        {
            if (experimentTurn == int.Parse(getTarget.tag))
            {
                // 1~2단계 실험
                if ((experimentTurn == 1 || experimentTurn == 2) && TriggerObject.tag.Equals("3"))
                {
                    Destroy(getTarget.gameObject);
                    Destroy(GameObject.FindWithTag("name" + TriggerObject.tag));
                    if (experimentTurn == 1)
                    {
                        // 포일 + 마시멜로 생성
                        GameObject obj = Instantiate(MarshFoil);
                        obj.transform.localScale = new Vector3(fscale, fscale, fscale);
                        // nameTag 제거
                        nameTag[experimentTurn].SetActive(false);
                        // TriggerObejct nameTag 제거
                        nameTag[int.Parse(TriggerObject.tag)].SetActive(false);
                        // nameTag 삭제
                        Destroy(GameObject.FindWithTag("name" + int.Parse(TriggerObject.tag)));
                        // 새로운 실험도구를 Trigger 된 실험도구 위치에 배정
                        obj.transform.position = TriggerObject.transform.position;
                        obj.transform.rotation = TriggerObject.transform.rotation;
                        // 기존의 Trigger실험도구 제거
                        Destroy(TriggerObject);

                        if (controller != null)
                        {
                            obj.transform.parent = controller.ObjectP1.transform;
                        }
                        if (controllerNoAr != null)
                        {
                            obj.transform.parent = controllerNoAr.ObjectP1.transform;
                        }
                    }
                    else if (experimentTurn == 2)
                    {
                        // 포일 + 마시멜로 + 식염 색소 생성
                        GameObject obj = Instantiate(MarshFoil_Coloring);
                        obj.transform.localScale = new Vector3(fscale, fscale, fscale);
                        // nameTag 제거
                        nameTag[experimentTurn].SetActive(false);
                        // 새로운 실험도구를 Trigger 된 실험도구 위치에 배정
                        obj.transform.position = TriggerObject.transform.position;
                        obj.transform.rotation = TriggerObject.transform.rotation;
                        // 기존의 Trigger실험도구 제거
                        Destroy(TriggerObject);
                        if (controller != null)
                        {
                            obj.transform.parent = controller.ObjectP1.transform;
                        }
                        if (controllerNoAr != null)
                        {
                            obj.transform.parent = controllerNoAr.ObjectP1.transform;
                        }
                        // 다음 단계 나래이션
                        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
                    }
                    deleteObject = false;
                    TriggerObject = null;
                    experimentTurn++;
                }
                // 은박접시 + 접힌 호일
                else if ((experimentTurn == 4) && TriggerObject.tag.Equals("5"))
                {
                    Destroy(getTarget);
                    GameObject obj = Instantiate(silver_Plate);
                    obj.transform.position = TriggerObject.transform.position;
                    obj.transform.rotation = TriggerObject.transform.rotation;
                    obj.transform.localScale = new Vector3(fscale, fscale, fscale);
                    nameTag[int.Parse(TriggerObject.tag)].SetActive(false);
                    Destroy(TriggerObject);
                    if (controller != null)
                    {
                        obj.transform.parent = controller.ObjectP1.transform;
                    }
                    else if (controllerNoAr != null)
                    {
                        obj.transform.parent = controllerNoAr.ObjectP1.transform;
                    }
                    deleteObject = false;
                    TriggerObject = null;
                    // 다음 나래이션
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
                    experimentTurn++;
                }
                else if ((experimentTurn == 5) && TriggerObject.tag.Equals("8"))
                {
                    // 기존의 Trigger실험도구 제거
                    Destroy(GameObject.FindWithTag("name" + TriggerObject.tag));
                    Destroy(getTarget.transform.parent.gameObject);
                    // 최종 실험도구 생성
                    GameObject obj = Instantiate(TotalExperiment);
                    obj.transform.localScale = new Vector3(fscale, fscale, fscale);

                    // 새로운 실험도구를 Trigger 된 실험도구 위치에 배정   
                    obj.transform.position = TriggerObject.transform.position;
                    obj.transform.rotation = TriggerObject.transform.rotation;
                    // 기존의 Trigger실험도구 제거
                    Destroy(TriggerObject.transform.parent.gameObject);
                    // 파티클 생성
                    experimentParticle = obj.GetComponentInChildren<ExperimentParticle>();
                    nameTag[int.Parse( TriggerObject.tag )].SetActive(false);
                    if (controller != null)
                    {
                        obj.transform.parent = controller.ObjectP1.transform;
                    }
                    if (controllerNoAr != null)
                    {
                        obj.transform.parent = controllerNoAr.ObjectP1.transform;
                    }
                    // 다음 나래이션
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();

                    deleteObject = false;
                    TriggerObject = null;
                    experimentTurn++;
                }
                else if (experimentTurn == 6)
                {
                    if (getTarget.tag.Equals("" + experimentTurn))
                    {
                        // nameTag 제거
                        nameTag[experimentTurn].SetActive(false);
                        // 터지한 실험도구 제거
                        Destroy(getTarget.gameObject);
                        // 8단계면 위험표시 이미지 생성 후 다음 
                        DangerImage.SetActive(true);
                        StartCoroutine(WaitForIt());
                        deleteObject = false;
                        TriggerObject = null;
                    }
                }
                else if (experimentTurn == 7 && TriggerObject.tag.Equals("8"))
                {
                    // 터치한 실험도구 제거
                    Destroy(getTarget.gameObject);
                    // nameTag 제거
                    nameTag[experimentTurn].SetActive(false);
                    // 알코올램프에 불 파티클
                    Explode();
                    deleteObject = false;
                    TriggerObject = null;

                    // 다음 나래이션
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
                    experimentTurn++;
                }
                else
                {
                    getTarget.transform.position = firstPosition;
                }
            }
            else
            {
                getTarget.transform.position = firstPosition;
            }
        }

        // 8단계 이후 애니메이션 및 나래이션 play
        void AnimationPlayer()
        {
            if (experimentTurn >= 8)
            {
                // 1초 WaitTime
                if (experimentTurn == 8 && !audioPlayer.isPlaying && SoundEnd == true)
                {
                    StartCoroutine(WaitSound());
                }
                // 이전 나레이션이 끝난 후 1초 후 다음 나레이션 및 파티클 
                else if (experimentTurn == 8 && !audioPlayer.isPlaying && SoundEnd == false)
                {
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
                    experimentTurn++;
                    // 연기 파티클 생성
                    Smoke();
                }
                // 1초 WaitTime
                else if (experimentTurn == 9 && !audioPlayer.isPlaying && SoundEnd == false)
                {
                    StartCoroutine(WaitSound());
                }
                // 이전 나레이션이 끝난 후 1초 후 다음 나레이션
                else if (experimentTurn == 9 && !audioPlayer.isPlaying)
                {
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
                    experimentTurn++;
                }
                // 이전 나레이션이 끝난 후 1초 후 다음 나레이션 및 애니메이션
                else if (experimentTurn == 10 && !audioPlayer.isPlaying)
                {
                    // 용암이 다 흘러내리면 다음 단계
                    if (BlendNum >= 99f)
                    {
                        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
                        experimentTurn++;
                        BlendNum = 0;
                    }
                }
                // 실험 결과창 및 실제 모형 보기 단계로 이동
                else if (experimentTurn == 11 && !audioPlayer.isPlaying)
                {
                    // 결과창
                    ResultPrefab.SetActive(true);
                    MenuButton.SetActive(false);
                    QuitButton.SetActive(false);
                    ExplanationButton.SetActive(false);
                }
            }
        }

        // 연기 파티클
        void Smoke()
        {
            if (experimentParticle != null)
            {
                experimentParticle.PlaySmokeParticle();
            }
        }

        // 알코올램프 파티클
        void Explode()
        {
            if (experimentParticle != null)
            {
                experimentParticle.PlayCandleParticle();
            }
        }

        // 튜토리얼 Point
        void PointObject()
        {
            // 포일 접기 투핑거 제스쳐 포인트
            if (experimentTurn == 3)
            {
                if (po1 == null || po2 == null)
                {
                    // 생성
                    po1 = Instantiate(Point2);
                    po2 = Instantiate(Point3);
                    po1.SetActive(true);
                    po2.SetActive(true);
                    if (controller != null)
                    {
                        // 실험도구 Object 하위에 넣기
                        po1.transform.parent = controller.ObjectP1.transform;
                        po2.transform.parent = controller.ObjectP1.transform;

                        // 위치 조정
                        float targetx = GameObject.FindWithTag(experimentTurn + "").transform.position.x;
                        float targety = GameObject.FindWithTag(experimentTurn + "").transform.position.y;
                        float targetz = GameObject.FindWithTag(experimentTurn + "").transform.position.z;
                        po1.transform.position = new Vector3(targetx + 0.1f, targety + (0.15f * fscale), targetz);
                        po2.transform.position = new Vector3(targetx - 0.1f, targety + (0.15f * fscale), targetz);
                        // 크기 조정
                        po1.transform.localScale = new Vector3(0.025f * fscale, 0.025f * fscale, 0.025f * fscale);
                        po2.transform.localScale = new Vector3(0.025f * fscale, 0.025f * fscale, 0.025f * fscale);
                    }
                    if (controllerNoAr != null)
                    {
                        // 실험도구 Object 하위에 넣기
                        po1.transform.parent = controllerNoAr.ObjectP1.transform;
                        po2.transform.parent = controllerNoAr.ObjectP1.transform;
                        // 위치 조정
                        float targetx = GameObject.FindWithTag(experimentTurn + "").transform.position.x;
                        float targety = GameObject.FindWithTag(experimentTurn + "").transform.position.y;
                        float targetz = GameObject.FindWithTag(experimentTurn + "").transform.position.z;
                        po1.transform.position = new Vector3(targetx + 0.01f, targety + (0.03f * fscale * 7), targetz);
                        po2.transform.position = new Vector3(targetx - 0.01f, targety + (0.03f * fscale * 7), targetz);
                        // 크기 조정
                        po1.transform.localScale = new Vector3(0.025f * fscale * 7, 0.025f * fscale * 7, 0.025f * fscale * 7);
                        po2.transform.localScale = new Vector3(0.025f * fscale * 7, 0.025f * fscale * 7, 0.025f * fscale * 7);
                    }
                }
                // 드래그 중일때 제거
                if (isMouseDragging)
                {
                    Destroy(po1);
                    Destroy(po2);
                }
            }
            // 포일접기 이 외의 포인트
            else if (experimentTurn != 3 && experimentTurn <= 7)
            {
                if (GameObject.FindWithTag(experimentTurn + "") != null)
                {
                    if (po == null)
                    {
                        // 포인트 생성
                        po = Instantiate(Point1);
                        po.SetActive(true);
                        if (controller != null)
                        {
                            // 포인트를 실험도구 위치에 조정
                            po.transform.parent = controller.ObjectP1.transform;
                            // 위치 조정
                            float targetx = GameObject.FindWithTag(experimentTurn + "").transform.position.x;
                            float targety = GameObject.FindWithTag(experimentTurn + "").transform.position.y;
                            float targetz = GameObject.FindWithTag(experimentTurn + "").transform.position.z;
                            po.transform.position = new Vector3(targetx, targety + (0.15f * fscale), targetz);
                            // 크기 조정
                            po.transform.localScale = new Vector3(0.025f * fscale, 0.025f * fscale, 0.025f * fscale);
                        }
                        if (controllerNoAr != null)
                        {
                            // 포인트를 실험도구 위치에 조정
                            po.transform.parent = controllerNoAr.ObjectP1.transform;
                            // 위치 조정
                            float targetx = GameObject.FindWithTag(experimentTurn + "").transform.position.x;
                            float targety = GameObject.FindWithTag(experimentTurn + "").transform.position.y;
                            float targetz = GameObject.FindWithTag(experimentTurn + "").transform.position.z;
                            po.transform.position = new Vector3(targetx, targety + (0.03f * fscale * 7), targetz);
                            // 크기 조정
                            po.transform.localScale = new Vector3(0.025f * fscale * 7, 0.025f * fscale * 7, 0.025f * fscale * 7);
                        }
                    }
                    if (isMouseDragging)
                    {
                        Destroy(po);
                    }
                    else if (experimentTurn == 9 || experimentTurn == 10)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Destroy(po);
                        }
                    }
                }
            }
        }

        // 실험 단계별 설명 
        void PopupTextMeThod()
        {
            if (experimentTurn == 1 || experimentTurn == 2)
            {
                popup.SetActive(true);
                text.text = "첫째, 알루미늄 포일 위에 마시멜로를 놓고 식용 색소를 뿌립니다.";
            }
            else if (experimentTurn == 3)
            {
                text.text = "둘째, 알루미늄 포일로 마시멜로를 산 모양으로 감싼 뒤 윗부분을 열어 둡니다.\n" +
                    "(손가락 두 개를 이용해서 포일을 접어보세요.)";
            }
            else if (experimentTurn == 4)
            {
                text.text = "셋째, 마시멜로를 감싼 알루미늄 포일을 은박 접시 위에 올려놓습니다.";
            }
            else if (experimentTurn == 5)
            {
                text.text = "넷째, 은박 접시를 삼발이 위에 올려놓습니다.";
            }
            else if (experimentTurn == 6)
            {
                // 경고 이미지 활성화
                WaringPrefab.SetActive(true);
                text.text = "불을 붙이기 전에 실험복과 보안경, 면장갑을 착용하고 \n" +
                    "일정 거리를 유지하여 관찰합니다.";
            }
            else if (experimentTurn == 7)
            {
                //경고 이미지 비활성화
                WaringPrefab.SetActive(false);
                text.text = "다섯째, 알코올램프에 불을 붙인 뒤 나타나는 현상을 관찰해 봅니다.";
            }
            else if (experimentTurn == 8)
            {
                text.text = "알루미늄 포일이 들썩거립니다.";
            }
            else if (experimentTurn == 9)
            {
                text.text = "화산 모형 윗 부분에서 연기가 피어오릅니다.";
            }
            else if (experimentTurn == 10)
            {
                text.text = "화산 모형 윗 부분에서 액체 상태의 마시멜로가 흘러나옵니다.";
            }
            else if (experimentTurn == 11)
            {
                text.text = "흘러나온 마시멜로가 굳습니다.";
            }
        }

        // 실험도구를 터치햇는지 안햇는지 확인하고 터치한 실험도구 반환 
        GameObject ReturnClickedObject(out RaycastHit hit)
        {
            GameObject target = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
            {
                target = hit.collider.gameObject;
            }
            return target;
        }
        // 사운드 1초 wait
        IEnumerator WaitSound()
        {
            yield return new WaitForSeconds(1.0f);
            if (SoundEnd == true)
                SoundEnd = false;
            else
                SoundEnd = true;
        }
        // 보안경, 장갑 이미지 띄우고 7초후 비활성화
        IEnumerator WaitForIt()
        {
            yield return new WaitForSeconds(7.0f);
            DangerImage.SetActive(false);
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
            experimentTurn++;
            Destroy(po);
        }

        //Scale 초기화 함수
        public void ResetScale()
        {
            if (controller != null)
            {
                fscale = 1f;
            }
            else
            {
                fscale = 0.15f;
            }
        }
    }
}
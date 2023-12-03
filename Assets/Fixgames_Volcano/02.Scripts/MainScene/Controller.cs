using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCore.Examples.Common;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

namespace Fixgames.Volcano
{
    public class Controller : MonoBehaviour
    {
        [Header("Menu Toggle")]
        public GameObject CaptureButton;
        public GameObject LookButton;
        public GameObject DetailsButton;

        [Header("Plane Search")]
        public GameObject DetectedPlanePrefab;
        // ARCore 카메라 변수
        public Camera FirstPersonCamera;
        // 트랙킹된 평면
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();
        // 
        private bool m_IsQuitting = false;
        // Object가 생성됬는지 확인
        private bool create;

        [Header("Part1,2,3")]
        public GameObject popup;

        [Header("Part1")]
        // 실험 단계 시작하는 Popup
        public GameObject Prefab1;
        // 실험단계 Object
        public GameObject Prefab2;
        // DragObject GameObject
        public GameObject DragMgr;
        // 실험 도구 초기화 
        public GameObject ObjectP1;
        private const float k_ModelRotation = 180.0f;

        [Header("Part2")]
        // 실제 모형 Object
        public GameObject planeVisual;
        public GameObject VolcanoObject;
        public GameObject objectCheck;
        public GameObject volcnoText;
        public GameObject jet;
        public GameObject[] jetTag;

        [Header("LoadingBar")]
        public GameObject m_ObjectCreateCircle;
        public GameObject SearchingForPlaneUI;
        private bool ShowExit = false;
        private bool showSearchingUI;

        // Set ShowExit
        public void SetShowExit(bool ex)
        {
            ShowExit = ex;
        }
        // Get Create
        public bool CreateObject()
        {
            return create;
        }
        // 화산 도형 활성화/비활성화
        public void SetActiveVolcano(bool b)
        {
            VolcanoObject.SetActive(b);
        }

        public void Update()
        {
            _UpdateApplicationLifecycle();
            // 종료, 설명하기 버튼 눌렸을때 로딩바 안보이게함
            if (ShowExit)
            {
                SearchingForPlaneUI.SetActive(false);
                return;
            }

            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            if(!create)
                showSearchingUI = true;

            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                // 평면이 트랙킹 됬을 땐
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;
                    break;
                }
            }
            SearchingForPlaneUI.SetActive(showSearchingUI);
            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
                {
                    m_ObjectCreateCircle.SetActive(true);
                    m_ObjectCreateCircle.transform.position = hit.Pose.position;
                }
                else
                {
                    m_ObjectCreateCircle.SetActive(false);
                }
                return;
            }
            else
            {
                if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
                {
                    // Use hit pose and camera pose to check if hittest is from the
                    // back of the plane, if it is, no need to create the anchor.
                    if ((hit.Trackable is DetectedPlane) && Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) < 0)
                    {
                        Debug.Log("Hit at back of the current DetectedPlane");
                    }
                    else
                    {
                        if (!create)
                        {
                            //생성
                            create = true;
                            //팝업창 생성
                            Prefab1.SetActive(true);    // popup 생성
                            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play();
                            //실험도구 위치조정
                            Prefab2.transform.position = new Vector3(hit.Pose.position.x, hit.Pose.position.y + 0.0005f, hit.Pose.position.z);
                            Prefab2.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                            // 해당위치에 생성
                            ObjectP1 = Instantiate(Prefab2);
                            ObjectP1.SetActive(false);

                            //Loading 없앰
                            showSearchingUI = false;
                            GameObject.FindObjectOfType<DetectedPlaneVisualizer>().isObjectCreate = true;
                            m_ObjectCreateCircle.GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                    if (create)
                    {
                        showSearchingUI = false;
                    }
                }
            }
        }

        // 실제 화산 분출 모형 Object 생성
        public void ChangeObject()
        {
            if (objectCheck.GetComponent<Close>().check == true)
            {
                VolcanoObject.GetComponent<CreateBtn>().Reset();
                planeVisual.SetActive(false);
                VolcanoObject.SetActive(true);
                volcnoText.SetActive(true);
                VolcanoObject.transform.position = Prefab2.transform.position;
                VolcanoObject.transform.rotation = Quaternion.identity;
                Prefab2.SetActive(false);
                jet.SetActive(true);
                VolcanoObject.GetComponent<Particle>().StartParticle();
            }
            else
            {
                VolcanoObject.SetActive(false);
                volcnoText.SetActive(false);
                VolcanoObject.transform.position = Prefab2.transform.position;
                VolcanoObject.transform.rotation = Quaternion.identity;
                Prefab2.SetActive(false);
                jet.SetActive(false);
            }
            Destroy(GameObject.Find("Experiment(Clone)"));
        }
        // 실험하기 Object 생성하기
        public void ChangeObjectPart1()
        {
            Destroy(GameObject.Find("Experiment(Clone)"));
            DragMgr.GetComponent<DragObject>().SetExperimentTurn(1);
            ObjectP1 = Instantiate(Prefab2);
            ObjectP1.SetActive(false);
            ObjectP1.transform.localScale = new Vector3(1, 1, 1);
            VolcanoObject.SetActive(false);
        }

        private void _UpdateApplicationLifecycle()
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if(Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if(m_IsQuitting)
            {
                return;
            }

            if(Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if(Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
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
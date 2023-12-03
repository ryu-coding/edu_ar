using UnityEngine;
using System.Collections;

namespace Fixgames.Volcano
{
    // Object의 크기를 조절 하는 Script.

    public class PinchZoom : MonoBehaviour
    {
        /// <summary>
        /// Volcano의 크기를 조절 할수 있도록 함.
        /// </summary>

        public Camera MainCamera;
        public GameObject obj;
        float Scale = 1, ZoomSpeed = 2f;
        public bool start;

        public bool arMode = true;
        
        /// <summary>
        /// 실행시 일시적으로 Particle.cs의 start를 false로 마그마 분출.
        /// 마그마 분출 애니메이션을 잠시 멈춤.(마그마 분출이 Volcano 크기에 따라 잘 안보이는경우를 대비)
        /// </summary>

        void Start()
        {
            start = false;

            ResetScale();
        }

        void Update()
        {
            ///<summary>
            /// touch인식이 두개가 되면 두 touch간에 간격차를 이용하여,
            /// 확대 및 축소를 실행시킴.(Rotate도 일시 정지)
            /// touch인식이하나 이상 사라지면, 사용했던 touch반납 및 Rotate활성화.
            ///</summary>
            if (Input.touchCount == 2 && start)
            {
                obj.GetComponent<Rotate>()._isRotating = false; // 확대 시, 일시적으로 회전 정지.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitoudeDiff = prevTouchDeltaMag - touchDeltaMag;
                float tempscale = Scale + (deltaMagnitoudeDiff * ZoomSpeed / (-100));
                
                ///<summary>
                ///ar과 no_ar간의 크기 차이가 형성 되기 때문에.
                ///ar여부에 따라 크기 제한 설정.
                ///</summary>
                ///

                if(arMode)
                {
                    tempscale = Mathf.Clamp(tempscale, 0.5f, 4f);
                }
                else
                {
                    tempscale = Mathf.Clamp(tempscale, 0.1f, 0.9f);
                }

                Scale = Mathf.Lerp(Scale, tempscale, 5 * Time.deltaTime);
                obj.transform.localScale = new Vector3(Scale, Scale, Scale);

                ///<summary>
                ///사용한 터치 반납.
                ///회전 활성화.
                ///</summary>

                if (touchZero.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Ended)
                {
                    obj.GetComponent<Rotate>()._isRotating = true;
                }
            } 
            
        }

        /// <summary>
        /// 외부에서 실행 여부를 지정 할수 있도록 하는 메소드.
        /// </summary>
        public void SetStart(bool setting)
        {
            start = setting;

            ResetScale();
        }

        /// <summary>
        /// 초기의 크기 값 설정.
        /// </summary>
        
        public void ResetScale()
        {
            Scale = obj.transform.localScale.x;
        }
    }
}
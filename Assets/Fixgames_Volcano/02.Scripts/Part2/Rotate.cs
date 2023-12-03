using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fixgames.Volcano
{
    //Object 회전하는 Script

    public class Rotate : MonoBehaviour
    {
        /// <summary>
        /// touch가 인식되고 touch 방향에 따라, 회전하기 위한 변수 설정.
        /// _isRoating은  재배치하기 같은 기능을 사용할 때, 회전을 하지 않기 하기위해 외부 접근이가능한 public 변수로 설정
        /// </summary>
        private float _sensitivity;
        private Vector3 _mouseReference;
        private Vector3 _mouseOffset;
        private Vector3 _rotation;
        public bool _isRotating,Replacing;
        public bool rotateAllDirection;
        
        void Start()
        {
            ///<summary>
            ///회전 초기값 설정.(회전 속도,초기 방향)
            ///</summary>
            _sensitivity = 0.2f;
            _rotation = Vector3.zero;
            Replacing = false;
        }
        
        void Update()
        {
            ///<summary>
            ///회전 활성화 및 터치 인식, 재배치 중이 아닐경우에 실행
            ///Touch한 후, Drag 방향으로 Objcet 이동.
            ///</summary>
            ///

            if(Input.GetMouseButtonDown(0))
            {
#if !UNITY_EDITOR
                if(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
                if (!EventSystem.current.IsPointerOverGameObject())
#endif
                {
                    _isRotating = true;
                    _mouseReference = Input.mousePosition;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isRotating = false;
            }
            
            if (_isRotating && Input.GetMouseButton(0) && Replacing == false)
            {
                    _mouseOffset = (Input.mousePosition - _mouseReference);
                _rotation.x = -_mouseOffset.y * _sensitivity;
                _rotation.y = -_mouseOffset.x * _sensitivity;
                if (rotateAllDirection)
                {
                    Camera.main.transform.RotateAround(this.transform.position, Vector3.right, _rotation.x);
                }
                this.transform.Rotate(new Vector3(0, _rotation.y, 0));
                _mouseReference = Input.mousePosition;
            }

        }


        /// <summary>
        /// 외부에서 회전 여부를 설정 시, 실행 할 수 있도혹 설정.
        /// </summary>

        public void SetRotate(bool set)
        {
            Replacing = set;
        }
    }
}

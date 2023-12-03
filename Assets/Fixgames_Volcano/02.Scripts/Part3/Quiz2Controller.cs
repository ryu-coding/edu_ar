using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class Quiz2Controller : MonoBehaviour
    {
        int successCount = 0;
        public GameObject EndPopup;
        // Use this for initialization
        void Start()
        {

        }
        public void Success()
        {
            successCount++;
        }
        // Update is called once per frame
        void Update()
        {
            if(successCount == 6)
            {
                EndPopup.SetActive(true);
                successCount = 0;
            }
        }
    }
}

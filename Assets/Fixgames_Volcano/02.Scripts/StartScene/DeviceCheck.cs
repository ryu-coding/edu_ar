using GoogleARCore;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fixgames.Volcano
{
    public class DeviceCheck : MonoBehaviour
    {
        private bool supportArCore = false;     // AR CORE 지원 여부
        private bool checkComplete = false;     // 지원 여부 확인 완료

        void Start()
        {
            StartCoroutine(CheckCompatibility());
        }

        void Update()
        {
            if(checkComplete)
            {
                if (supportArCore)
                {
                    SceneManager.LoadScene("main");
                }
                else
                {
                    SceneManager.LoadScene("main_no_ar");
                }
            }
        }

        private IEnumerator CheckCompatibility()
        {
            AsyncTask<ApkAvailabilityStatus> checkTask = null;

            try
            {
                checkTask = Session.CheckApkAvailability();
            }
            catch(EntryPointNotFoundException e)
            {
                supportArCore = false;
                checkComplete = true;
                Debug.Log(e);
            }            

            if(checkTask != null)
            {
                CustomYieldInstruction customYield = checkTask.WaitForCompletion();
                yield return customYield;
                ApkAvailabilityStatus result = checkTask.Result;
                switch(result)
                {
                    case ApkAvailabilityStatus.SupportedApkTooOld:
                    case ApkAvailabilityStatus.SupportedInstalled:
                        supportArCore = true;
                        break;
                    case ApkAvailabilityStatus.SupportedNotInstalled:
                        _ShowAndroidToastMessage("Supported, not installed, requesting installation");
                        Session.RequestApkInstallation(false);
                        supportArCore = true;
                        break;
                    case ApkAvailabilityStatus.UnknownChecking:
                    case ApkAvailabilityStatus.UnknownError:
                    case ApkAvailabilityStatus.UnknownTimedOut:
                    case ApkAvailabilityStatus.UnsupportedDeviceNotCapable:
                        supportArCore = false;
                        break;
                }

                checkComplete = true;
            }
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
    }
}

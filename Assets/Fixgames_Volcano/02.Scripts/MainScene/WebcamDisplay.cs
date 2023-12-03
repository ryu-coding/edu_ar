using System.Collections;
using UnityEngine;

namespace Fixgames.Volcano
{
    public class WebcamDisplay : MonoBehaviour
    {
        // 웹캠 텍스쳐
        public Renderer webCamRenderer;
        private WebCamDevice[] devices;
        private WebCamTexture wct;

#if UNITY_ANDROID
        public void Start()
#else
        public IEnumerator Start()
#endif
        {
            bool permissionGranted = false;

#if UNITY_ANDROID
            AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.CAMERA");
            if(result == AndroidRuntimePermissions.Permission.Granted)
            {
                permissionGranted = true;
            }
            else
            {
                _ShowAndroidToastMessage("No Permission Error");
            }
#else
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if(Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                permissionGranted = true;
            }
            else
            {
                Debug.Log("No Permission Error");
            }
#endif

            if(permissionGranted)
            {
                devices = WebCamTexture.devices;
                if(devices.Length > 0)
                {
                    wct = new WebCamTexture(devices[0].name, 1280, 800, 30);
                    webCamRenderer.material.mainTexture = wct;
                    wct.Play();
                }
            }
        }

        public void LateUpdate()
        {
            if(wct != null && wct.isPlaying)
            {
                if(wct.videoRotationAngle == 180f)
                {
                    webCamRenderer.transform.eulerAngles = new Vector3(180f, 180f, 0f);
                }
                else
                {
                    webCamRenderer.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                }
            }
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

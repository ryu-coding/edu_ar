using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;

namespace Fixgames.Volcano
{
    /// <summary>
    /// 무료 배포 가능한 plugin을 사용 하여 캡쳐 및 갤러리에 저장·갱신 하는 Script, 
    /// 출처 : https://github.com/yasirkula/UnityNativeGallery
    /// </summary>

    public class Capture : MonoBehaviour
    {
        public void OnClick()
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().ScreenshotSound();
            StartCoroutine(TakeScreenshotAndSave()); // 캡처하기 버튼 클릭시 준비되어있던 코루틴 호출
        }
        private IEnumerator TakeScreenshotAndSave()
        {
            yield return new WaitForEndOfFrame();

            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ss.Apply();     //자신이 보고 있는 화면(구동하는 기기의 화면)을 그대로 Texture로 변환하여 저장을 한다.
            
           Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, "GalleryTest", "My img {0}.png")); 
            
            Destroy(ss); // 저장에 사용한 Textre를 다음에 다시 사용하기 위해 기존에 저장된  Texture를 삭제한다.
        }
    }
}

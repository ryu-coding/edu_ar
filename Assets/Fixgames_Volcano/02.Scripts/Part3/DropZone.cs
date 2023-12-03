using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fixgames.Volcano
{
    public class DropZone : MonoBehaviour, IDropHandler
    {
        private Text mText;
        public void Start()
        {
            mText = gameObject.GetComponent<Text>();
        }

        // 커서가 UI객체의 Rect 영역에서 Object를 놓을때
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.name.Equals(gameObject.name))
            {
                Drag d = eventData.pointerDrag.GetComponent<Drag>();
                if (d != null)
                {
                    if (mText != null)
                    {
                        mText.text = gameObject.name;
                        GameObject controller = GameObject.Find("ReviewTest2Controller");
                        controller.GetComponent<Quiz2Controller>().Success();
                        GameObject.Find("AudioManager").GetComponent<AudioPlayer>().SuccessPlay();
                    }
                    GameObject.Destroy(d.gameObject);
                }
            }
            else
                GameObject.Find("AudioManager").GetComponent<AudioPlayer>().FailPlay();
        }
    }
}

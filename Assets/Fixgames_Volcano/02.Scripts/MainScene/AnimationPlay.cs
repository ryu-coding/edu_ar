using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fixgames.Volcano
{
    public class AnimationPlay : MonoBehaviour
    {
        private Animator Anim;
        int experimentTurn;
        // Use this for initialization
        void Start()
        {
            Anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameObject.Find("DragManager") != null)
                experimentTurn = GameObject.Find("DragManager").GetComponent<DragObject>().GetExperimentTurn();
            if (experimentTurn == 8)
            {
                Anim.SetBool("SetAnimation", false);
            }
            else if (experimentTurn == 11)
            {
                Anim.SetBool("SetAnimation", true);
            }
        }
    }
}
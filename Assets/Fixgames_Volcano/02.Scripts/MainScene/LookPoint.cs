using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fixgames.Volcano
{
    public class LookPoint : MonoBehaviour
    {
        Vector3 po;
        void Update()
        {
            po = Camera.main.transform.position;
            po.y = transform.position.y;
            po.z = -transform.position.z;
            transform.LookAt(Camera.main.transform.position);
        }
    }
}
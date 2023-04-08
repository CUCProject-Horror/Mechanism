using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VirtualEscapes.Common.Jitter;

namespace VirtualEscapes.Common.JitterExample
{
    public class ObjRotate : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, 0, GetComponent<JitterCustom>().jitterValue * Time.deltaTime);
        }
    }
}

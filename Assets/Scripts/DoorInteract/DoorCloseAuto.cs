using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class DoorCloseAuto : MonoBehaviour
{
        public DoorOpen dr;
        public void CloseDoor()
        {
            if(dr.canClose)
            dr.isClosing = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            CloseDoor();
        }
        void Update()
        {
            if (dr.isClosing && dr.timer <= dr.maxAngle / Mathf.Abs(dr.closeForce))
            {
                dr.gameObject.transform.Rotate(0, 0, dr.closeForce * Time.deltaTime * dr.pushForce / Mathf.Abs(dr.pushForce));
                dr.timer += Time.deltaTime;
            }
            else if (dr.isClosing)
            { 
                dr.isClosing = false; dr.timer = 0;
                dr.canClose = false;
            }
        }
}
}

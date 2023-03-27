using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    public class DoorCloseAuto : MonoBehaviour
{
        public DoorOpen dr;
        public GameObject closeDistance;
        public UnityEvent onClosing;
        public void CloseDoor()
        {
            if (dr.canClose && dr.isOpening != true)
            {
                if (dr.isInfinity == false)
                {
                    Destroy(dr.innerUI.transform.parent.gameObject);
                    Destroy(dr.outterUI.transform.parent.gameObject);
                    dr.hasDestroyedDoor = true;
                    Destroy(this.gameObject);
                }
                dr.isClosing = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && !dr.isClosing)
            {
                onClosing.Invoke();
                CloseDoor();
            }        
        }
}
}

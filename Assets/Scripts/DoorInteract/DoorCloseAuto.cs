using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class DoorCloseAuto : MonoBehaviour
{
        public DoorOpen dr;
        public GameObject closeDistance;
        public void CloseDoor()
        {
            if (dr.canClose && dr.isOpening != true)
            {
                if (dr.isInfinity == false)
                {
                    Destroy(dr.innerUI.transform.parent.gameObject);
                    Destroy(dr.outterUI.transform.parent.gameObject);
                    dr.hasDestroyedDoor = true;
                }
                dr.isClosing = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player" && !dr.isClosing)
            CloseDoor();
        }
}
}

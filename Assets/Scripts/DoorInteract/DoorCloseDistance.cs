using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
    public class DoorCloseDistance : MonoBehaviour
    {
        public DoorOpen dr;
        public void CloseDoor()
        {
            if (dr.canClose && dr.isOpening != true)
                dr.isClosing = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
                CloseDoor();
        }
    }
}

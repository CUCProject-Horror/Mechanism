using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Key : MonoBehaviour
    {
        public GameObject targetDoor;
        public GameObject targetOther;

        public void GetKey()
        {
            targetDoor.GetComponent<DoorOpen>().isLocked = false;
        }

        public void GetOtherKey()
        {
            targetOther.GetComponent<Glitch>().canGlitch = true;
        }
    }
}

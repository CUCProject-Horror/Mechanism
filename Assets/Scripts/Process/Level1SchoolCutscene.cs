using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Level1SchoolCutscene : MonoBehaviour
    {
        public GameObject targetInteractable;
        public GameObject cutsceneTrigger;
        public DoorOpen door;

        public void Interact()
        {
            cutsceneTrigger.SetActive(true);
            Destroy(targetInteractable);
        }

        public void DestroyTrigger()
        {
            Destroy(this.gameObject);
        }

        public void DoorUnlock()
        {
            door.OpenTheDoor();
            targetInteractable.SetActive(true);
        }
    }
}

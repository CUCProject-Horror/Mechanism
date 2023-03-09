using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Game {
    //[RequireComponent(typeof(PlayerInput))]
    public class DoorOpen : MonoBehaviour
    {

        public GameObject innerUI;
        public GameObject outterUI;
        public bool isLocked;
        public int timer = 0;
        public float force;
        public float pushForce;
        public float maxAngle;
        public float closeForce;
        public bool isInfinity;
        public float handleAnimTime;

        bool isBlocked;
        public bool canPush = false;

        public UnityEvent onDoorLock;
        public UnityEvent onDoorOpen;
        [HideInInspector] public bool canClose = false;
        [HideInInspector] public bool isOpening = false;
        [HideInInspector] public bool isClosing = false;
        [HideInInspector] public bool hasDestroyedDoor = false;
        //public bool hasPassedDoor = false;

        InputManager inputManager;

        private void Start()
        {
            isBlocked = false;
        }

        public void InteractDoorOutter()
        {
            canPush = true;
            force = pushForce;
        }

        public void DeactivateDoorOutter()
        {
            canPush = false;
            force = 0;
        }

        public void InteractDoorInner()
        {
            canPush = true;
            force = -pushForce;
        }

        public void DeactivateDoorInner()
        {
            canPush = false;
            force = 0;
        }

        void OpenTheDoor()
        {
            innerUI.SetActive(false);
            outterUI.SetActive(false);
            isOpening = true;
            canClose = true;
        }

        public void Drag(Component source, Vector3 drag)
        {
            if (!canPush)
                return;
            else
            {
                if(force > 0)
                {
                    if (drag.y > 6)
                    {
                        if (isLocked)
                        {
                            Debug.Log("The Door Is Locked!");
                            onDoorLock.Invoke();
                            canPush = false;
                        }
                        else
                        {
                            onDoorOpen.Invoke();
                            canPush = false;
                            Invoke("OpenTheDoor", handleAnimTime);
                        }
                        
                    }
                }
                else if (force < 0)
                {
                    if (drag.y < -6)
                    {
                        if (isLocked)
                        {
                            onDoorLock.Invoke();
                            Debug.Log("The Door Is Locked!");
                            canPush = false;
                        }
                        else
                        {
                            onDoorOpen.Invoke();
                            canPush = false;
                            Invoke("OpenTheDoor", handleAnimTime);
                        }
                    }
                }
            }
        }

        public void OnReleaseInteract()
        {
            canPush = false;
            //Debug.Log("Release!");
        }

        private void FixedUpdate()
        {

            if (isOpening && timer < (int)(maxAngle / Mathf.Abs(pushForce)/ Time.deltaTime) && !isBlocked)
            {
                transform.Rotate(0, 0, pushForce * Time.deltaTime);
                timer ++ ;
            }
            else if (isOpening && timer < maxAngle / Mathf.Abs(pushForce)/ Time.deltaTime && isBlocked)
            { 
                return;
            }
            else if (isOpening)
            {
                timer = 0;
                isOpening = false;
            }

            if (isClosing && timer < (int)(maxAngle / Mathf.Abs(closeForce)/ Time.deltaTime))
            {
                transform.Rotate(0, 0, closeForce * Time.deltaTime * -pushForce / Mathf.Abs(pushForce));
                timer ++;
            }
            else if (isClosing && hasDestroyedDoor)
            {
                isClosing = false; timer = 0;
                canClose = false;
            }
            else if (isClosing)
            {
                timer = 0;
                innerUI.SetActive(true);
                outterUI.SetActive(true);
                isClosing = false; 
                canClose = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                isBlocked = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                isBlocked = false;
            }
        }

    }

}

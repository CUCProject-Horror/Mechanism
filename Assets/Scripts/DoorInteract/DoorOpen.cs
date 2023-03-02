using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
    //[RequireComponent(typeof(PlayerInput))]
    public class DoorOpen : MonoBehaviour
    {

        public GameObject innerUI;
        public GameObject outterUI;
        public Animator handle;
        public bool isLocked;
        public float timer = 0;
        float force;
        public float pushForce;
        public float maxAngle;
        public float closeForce;
        public bool isInfinity;
        public float handleAnimTime;

        bool isBlocked;
        bool canPush = false;
        [HideInInspector] public bool canClose = false;
        [HideInInspector] public bool isOpening = false;
        [HideInInspector] public bool isClosing = false;
        [HideInInspector] public bool hasDestroyedDoor = false;
        //public bool hasPassedDoor = false;

        private void Start()
        {
            isBlocked = false;
            AnimatorStateInfo info = handle.GetCurrentAnimatorStateInfo(0);
        }

        public void InteractDoorOutter()
        {
            canPush = true;
            force = pushForce;
        }

        public void InteractDoorInner()
        {
            canPush = true;
            force = -pushForce;
        }

        void OpenTheDoor()
        {
            innerUI.SetActive(false);
            outterUI.SetActive(false);
            isOpening = true; 
            canClose = true;
        }

        public void OnOpendoor(InputValue value)
        {
            if (!canPush)
                return;
            else
            {
                if(force > 0)
                {
                    if (value.Get<Vector2>().y > 0)
                    {
                        if (isLocked)
                        {
                            Debug.Log("The Door Is Locked!");
                            handle.SetTrigger("isLocked");
                            canPush = false;
                        }
                        else
                        {
                            handle.SetTrigger("isOpening");
                            canPush = false;
                            Invoke("OpenTheDoor", handleAnimTime);
                        }
                        
                    }
                }
                else if (force < 0)
                {
                    if (value.Get<Vector2>().y < 0)
                    {
                        if (isLocked)
                        {
                            Debug.Log("The Door Is Locked!");
                            handle.SetTrigger("isLocked");
                            canPush = false;
                        }
                        else
                        {
                            handle.SetTrigger("isOpening");
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

        private void Update()
        {
            if (isOpening && timer <= maxAngle / Mathf.Abs(pushForce) && !isBlocked)
            {
                transform.Rotate(0, 0, pushForce * Time.deltaTime);
                timer += Time.deltaTime;
            }
            else if (isOpening && timer <= maxAngle / Mathf.Abs(pushForce) && isBlocked)
            { 
                return;
            }
            else if (isOpening)
            {
                isOpening = false; timer = 0;
                

            }

            if (isClosing && timer <= maxAngle / Mathf.Abs(closeForce))
            {
                gameObject.transform.Rotate(0, 0, closeForce * Time.deltaTime * -pushForce / Mathf.Abs(pushForce));
                timer += Time.deltaTime;
            }
            else if (isClosing && hasDestroyedDoor)
            {
                isClosing = false; timer = 0;
                canClose = false;
            }
            else if (isClosing)
            {
                innerUI.SetActive(true);
                outterUI.SetActive(true);
                isClosing = false; timer = 0;
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
        { isBlocked = false; }

    }

}

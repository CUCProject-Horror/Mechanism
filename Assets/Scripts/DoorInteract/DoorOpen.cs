using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
    //[RequireComponent(typeof(PlayerInput))]
    public class DoorOpen : MonoBehaviour
    {
        public bool isLocked;
        public bool canPush = false;
        public bool canClose = false;
        public bool isOpening = false;
        public bool isClosing = false;
        public float timer = 0;
        public float pushForce; 
        public float maxAngle; 
        public float closeForce;

        public bool isBlocked;

        private void Start()
        {
            isBlocked = false;
        }

        public void InteractDoor()
        {
            canPush = true;
        }

        public void OnOpendoor(InputValue value)
        {
            if (!canPush)
                return;
            else if (isLocked)
            { Debug.Log("The Door Is Locked!"); }
            else
            {
                if(value.Get<Vector2>().x >= 0.5f)
                {
                    isOpening = true;
                    canPush = false;
                    canClose = true;
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
                transform.Rotate(0, 0, -pushForce * Time.deltaTime);
                timer += Time.deltaTime;
            }
            else if (isOpening && timer <= maxAngle / Mathf.Abs(pushForce) && isBlocked)
            { return; }
            else if (isOpening)
            {
                isOpening = false; timer = 0;
                //销毁UI
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

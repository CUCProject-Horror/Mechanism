using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game{
    public class DoorOpenDouble : MonoBehaviour
{
    public DoorOpen dr;
    public bool isBlocked;
    int timer;
    bool isOpening = false;
    bool notOpen = true;
    bool isClosing = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            if (dr.isOpening && notOpen) 
            { 
                isOpening = true;
                notOpen = false;
            }

            if (isOpening && timer < (int)(dr.maxAngle / Mathf.Abs(dr.pushForce) / Time.deltaTime) && !isBlocked && !notOpen)
            {
                transform.Rotate(0, 0, -dr.pushForce * Time.deltaTime);
                timer ++;
            }
            else if (isOpening && timer <= dr.maxAngle / Mathf.Abs(dr.pushForce) / Time.deltaTime && isBlocked && !notOpen)
            {
                return;
            }
            else if(isOpening)
            {
                timer = 0;
                isOpening = false;
            }

            if (dr.isOpening == false && !isOpening) 
            { notOpen = true; }

            if (dr.isClosing && timer < (int)(dr.maxAngle / Mathf.Abs(dr.closeForce) / Time.deltaTime))
            {
                isClosing = true;
                transform.Rotate(0, 0, dr.closeForce * Time.deltaTime * dr.pushForce / Mathf.Abs(dr.pushForce));
                timer++;
            }
            else if(isClosing)
            {
                timer = 0;
                isClosing = false;
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

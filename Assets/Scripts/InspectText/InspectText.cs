using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class InspectText : MonoBehaviour
    {
        public float maxFocusTime;

        float timer = 0;
        bool isFocusing = false;
        bool canFocus;
        bool canInspect = false;
        bool hasInspected = false;

        public UnityEvent showSubtitle;
        public UnityEvent stopSubtitle;

        void FixedUpdate()
        {
            if(isFocusing && canFocus)
            {
                timer += Time.deltaTime;
            }

            if(timer >= maxFocusTime)
            {
                canInspect = true;
                timer = 0;
            }

            EnterInspect();
            LeaveInspect();
        }

        public void OnFocus()
        {
            isFocusing = true;
        }

        public void OnBlur()
        {
            timer = 0;
            isFocusing = false;
            canInspect = false;
        }

        public void EnterInspect()
        {
            if(canInspect && canFocus && !hasInspected)
            {
                Debug.Log("Inspect!");
                hasInspected = true;
                showSubtitle.Invoke();
                //BeginInspect
            }
        }

        public void LeaveInspect()
        {
            if((!canInspect || !canFocus) && hasInspected)
            {
                Debug.Log("Leave Inspect!");
                hasInspected = false;
                stopSubtitle.Invoke();
                //Destroy Subtitle
            }
        }

        private void OnTriggerStay(Collider other)
        {
            canFocus = true;
        }

        private void OnTriggerExit(Collider other)
        {
            canFocus = false;
            canInspect = false;
            timer = 0;
        }
    }
}
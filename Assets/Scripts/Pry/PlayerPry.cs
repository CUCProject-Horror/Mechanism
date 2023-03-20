
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using Cinemachine;

namespace Game
{
    public class PlayerPry : MonoBehaviour
    {
        public Camera mainCam;
        public Camera pryCam;
        public SpriteRenderer indcator;
        public GameObject pryCanvas;
        public GameObject blinkImage;
        public Sprite pryTex;
        public Sprite blinkStartTex;
        public Animator pryAnim;

        public CinemachineVirtualCamera[] cameras;

        public CinemachineVirtualCamera playerCam;
        public CinemachineVirtualCamera pryCamera;

        public CinemachineVirtualCamera startCam;
        private CinemachineVirtualCamera currentCam;
        bool isPrying;

        public UnityEvent OnEnterPry;
        public UnityEvent OnLeavePry;

        void Start()
        {
            mainCam = Camera.main;
            //pryAnim = GetComponent<Animator>();
            currentCam = startCam;

            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i] == currentCam)
                {
                    cameras[i].Priority = 20;
                }
                else
                {
                    cameras[i].Priority = 10;
                }
            }

        }


        public void Activate()
        {
            if (!isPrying)
            {
                //OnEnterPry.Invoke();

                //pryCam.enabled = true;
                mainCam.enabled = false;
                
                pryCam.depth = 1;
                isPrying = true;
                indcator.enabled = false;
                Invoke("PryAnimator", 1.5f);
                Invoke("PryMethod", 2f);
                SwitchCamera(pryCamera);
            }
            else
                return;
        }

        public void Deactivate()
        {
            indcator.enabled = true;
            mainCam.enabled = true;
            //pryCam.enabled = false;
            pryCam.depth = -1;
            SwitchCamera(playerCam);
            EndPryAnimator();
            Invoke("EndPryMethod", 0.5f);
        }

        public void SwitchCamera(CinemachineVirtualCamera newCam)
        {
            currentCam = newCam;

            currentCam.Priority = 20;

            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i] != currentCam)
                {
                    cameras[i].Priority = 10;
                }
            }
        }

        public void PryMethod()
        {
            if (isPrying)
            {
                pryCanvas.SetActive(true);
                pryCanvas.GetComponent<Image>().sprite = pryTex;
            }
        }

        public void EndPryMethod()
        {
            OnLeavePry.Invoke();
            isPrying = false;
            pryCanvas.SetActive(false);
            pryCanvas.GetComponent<Image>().sprite = null;
        }

        public void PryAnimator()
        {
            if(isPrying)
            pryAnim.SetBool("Pry", true);
        }

        public void EndPryAnimator()
        {
            pryAnim.SetBool("Pry", false);
        }

    }
}
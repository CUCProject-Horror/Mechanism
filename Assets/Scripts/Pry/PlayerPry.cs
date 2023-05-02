
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
        public GameObject pryCamRenderTex;
        public SpriteRenderer indcator;
        public GameObject pryObject;
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
        public UnityEvent OnStartPry;
        [HideInInspector]public Vector2 imgMoveAmount;

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
                OnEnterPry.Invoke();
                StartCoroutine(ChangeCinemachineState(true)); 
                isPrying = true;
                Invoke("PryAnimator", 1.5f);
                Invoke("PryMethod", 2f);
            }
            else
                return;
        }

        public void Deactivate()
        {
            StartCoroutine(ChangeCinemachineState(false));
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
                OnStartPry.Invoke();
                pryObject.SetActive(true);
                pryCamRenderTex.SetActive(false);
                //pryObject.transform.parent.transform.position = Vector3.zero;
                pryObject.GetComponent<Image>().sprite = pryTex;
            }
        }

        public void EndPryMethod()
        {
            OnLeavePry.Invoke();
            isPrying = false;
            pryObject.SetActive(false);
            pryObject.GetComponent<Image>().sprite = null;
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

        public IEnumerator ChangeCinemachineState(bool camState)
        {
            mainCam.enabled = !camState;
            pryCam.enabled = camState;
            indcator.enabled = !camState;
            yield return new WaitForSeconds(0.1f);
            pryCamRenderTex.SetActive(camState);

            if (camState)
            { SwitchCamera(pryCamera); }
            else if (!camState)
            { SwitchCamera(playerCam); }
        }


        public void MovePryObject()
        {
            pryObject.transform.parent.Translate(imgMoveAmount.x, imgMoveAmount.y, 0);
        }

        public void FixedUpdate()
        {
            MovePryObject();
        }

    }
}
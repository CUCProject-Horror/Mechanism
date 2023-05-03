
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
        #region Serialized Fields
        public Camera mainCam;
        public Camera pryCam;
        public GameObject pryCamRenderTex;
        public SpriteRenderer indcator;
        public GameObject pryObject;
        RectTransform pryObjTransform;
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
        public UnityEvent OnEndPry;
        public UnityEvent OnStartPry;
        [HideInInspector]public Vector2 imgMoveAmount;
        #endregion

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
            StartCoroutine(EndPry());
        }

        #region Internal Functions
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
                pryObjTransform.anchoredPosition3D = Vector3.zero;
                //每次开始pry时重置坐标的办法？
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

        public IEnumerator EndPry()
        {
            pryAnim.SetBool("Pry", false);
            yield return new WaitForSeconds(0.5f);
            EndPryMethod();
            yield return new WaitForSeconds(0.5f);
            OnEndPry.Invoke();
            yield return null;
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
        #endregion

        #region Life Cycle
        void Start()
        {
            mainCam = Camera.main;
            //pryAnim = GetComponent<Animator>();
            currentCam = startCam;
            pryObjTransform = pryObject.transform.parent.gameObject.GetComponent<RectTransform>();

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

        public void Update()
        {
            
            //if ((pryObjTransform.anchoredPosition3D.x + imgMoveAmount.x >= 1750) || (pryObjTransform.anchoredPosition3D.x + imgMoveAmount.x <= -1750))
            //{
            //    pryObject.transform.parent.Translate(0, imgMoveAmount.y, 0);
            //}
            //else
            //这样判断为什么会有误差呢？


            MovePryObject();

            if (pryObjTransform.anchoredPosition3D.x > 1750)
            {
                pryObjTransform.anchoredPosition3D = new Vector3(1750, pryObjTransform.anchoredPosition3D.y, pryObjTransform.anchoredPosition3D.z);
            }
            if (pryObjTransform.anchoredPosition3D.x < -1750)
            {
                pryObjTransform.anchoredPosition3D = new Vector3(-1750, pryObjTransform.anchoredPosition3D.y, pryObjTransform.anchoredPosition3D.z);
            }
            if (pryObjTransform.anchoredPosition3D.y > 960)
            {
                pryObjTransform.anchoredPosition3D = new Vector3(pryObjTransform.anchoredPosition3D.x, 960, pryObjTransform.anchoredPosition3D.z);
            }
            if (pryObjTransform.anchoredPosition3D.y < -960)
            {
                pryObjTransform.anchoredPosition3D = new Vector3(pryObjTransform.anchoredPosition3D.x, -960, pryObjTransform.anchoredPosition3D.z);
            }

            //添加限制条件（坐标）
        }
        #endregion
    }
}
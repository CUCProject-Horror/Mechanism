
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

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

    void Update()
    {

    }

    public void StartPry()
    {
        if (!isPrying)
        {
            pryCam.enabled = true;
            isPrying = true;
            indcator.enabled = false;
            Invoke("PryAnimator", 1.5f);
            Invoke("PryMethod", 2f);
        }
        else
            return;
    }

    public void OnEndPry()
    {
        EndPry();
    }

    public void EndPry()
    {
        indcator.enabled = true;
        pryCam.enabled = false;
        SwitchCamera(playerCam);
        EndPryAnimator();
        GetComponent<PlayerInput>().enabled = false;
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
        if(isPrying)
        {
            pryCanvas.SetActive(true);
            pryCanvas.GetComponent<Image>().sprite = pryTex;
            GetComponent<PlayerInput>().enabled = true;
        }    
    }

    public void EndPryMethod()
    {
        isPrying = false;
        pryCanvas.SetActive(false);
        pryCanvas.GetComponent<Image>().sprite = null;
    }

    public void PryAnimator()
    {
        pryAnim.SetTrigger("Pry");
    }

    public void EndPryAnimator()
    {
        pryAnim.SetTrigger("EndPry");
    }

}

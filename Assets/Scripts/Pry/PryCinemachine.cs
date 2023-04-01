using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PryCinemachine : MonoBehaviour
{
    public CinemachineFreeLook[] cameras;

    public CinemachineFreeLook playerCam;
    public CinemachineFreeLook pryCam;

    public CinemachineFreeLook startCam;
    private CinemachineFreeLook currentCam;

    private void Start()
    {
        currentCam = startCam;
        
        for(int i = 0; i < cameras.Length; i++)
        {
            if(cameras[i] == currentCam)
            {
                cameras[i].Priority = 20;
            }
            else
            {
                cameras[i].Priority = 10;
            }
        }
    }

    public void SwitchCamera(CinemachineFreeLook newCam)
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

}

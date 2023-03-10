
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPry : MonoBehaviour
{
    public Camera mainCam;
    
    public Camera pryCam;
    public Transform pryTransform;
    public GameObject pryCanvas;
    public Texture pryTex;
    public Animator pryAnim;

    public float moveSpeed;

    Vector3 playerCamPos;
    bool isMoving;
    float timer;

    void Start()
    {
        //mainCam = Camera.main;
        pryTransform = this.gameObject.transform;
        pryAnim = GetComponent<Animator>();
        
    }

    void Update()
    {
        CamMove();
    }

    public void StartPry()
    {
        mainCam.gameObject.SetActive(true);
        Debug.Log("!!!");
        playerCamPos = mainCam.transform.position;

        isMoving = true;

        //pryAnim.SetTrigger("Pry");
        //pryCanvas.SetActive(true);
        //pryCanvas.GetComponent<RawImage>().texture = pryTex;

    }

    public void EndPry()
    {
        mainCam.gameObject.SetActive(false);
        isMoving = false;
        mainCam.transform.position = playerCamPos;

        //pryAnim.SetTrigger("Pry");
        //pryCanvas.SetActive(false);
        //pryCanvas.GetComponent<RawImage>().texture = null;
    }

    public void CamMove()
    {
        float dis = (pryTransform.position - playerCamPos).magnitude;
        Vector3 dir = (pryTransform.position - playerCamPos).normalized;

        if (isMoving && timer <= dis / moveSpeed)
        {
            mainCam.transform.LookAt(pryTransform);
            mainCam.transform.position += dir * Time.deltaTime * moveSpeed;
            timer += Time.deltaTime;
        }
        else if(isMoving)
        {
            isMoving = false;
            timer = 0;
        } 
    }
}

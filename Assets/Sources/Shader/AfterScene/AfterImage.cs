using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterImage : MonoBehaviour
{
    //private float time_total;    
    public float time_Interval;
    [SerializeField]private int _downRes = 1;
    public GameObject AfterRawImage;
    public Transform RawImageCanvas;
    private RenderTexture[] rt;
    public int RenderTextureCount;
    public Camera rCamera;
    private bool start_afterImageEffect = true;

    bool test = true;

    /*private void Start()
    {
        time_total = AfterRawImage.Length * time_Interval; 
        foreach(GameObject i in AfterRawImage)
        {
            i.SetActive(false);            
        }        
    }*/

    private void LateUpdate()
    {
        if (test)
        {
            //start_afterImageEffect = !start_afterImageEffect;
            if(start_afterImageEffect)
            {
                rt = new RenderTexture[RenderTextureCount];
                for (int i = 0; i < rt.Length; i++)
                {
                    rt[i] = new RenderTexture(rCamera.pixelWidth >> _downRes, rCamera.pixelHeight >> _downRes, 16);
                }
                StartCoroutine(StartAfterImageEffect());
            }
            else
            {
                rt = null;
                rCamera.targetTexture = null;
                StopCoroutine(StartAfterImageEffect());
            }
            test = false;
       }
    }

    

    public IEnumerator StartAfterImageEffect()
    {        
        int iterator = 0;
        while (start_afterImageEffect)
        {            
            yield return new WaitForSeconds(time_Interval);
            if(rt != null)
            {
                rCamera.targetTexture = rt[iterator % rt.Length];
            }              
            GameObject temp = Instantiate(AfterRawImage, new Vector3(0, 0, 0), Quaternion.identity);
            temp.transform.SetParent(RawImageCanvas);
            //temp.GetComponent<AfterImageFade>().Init();
            if(rt != null)
            {
                temp.GetComponent<RawImage>().texture = rt[iterator % rt.Length];
            }              
            iterator++;            
        }        
        yield return null;       
    }

    /*public IEnumerator StartAfterImageEffect()
    {
        float timer = time_total;
        int iterator = 0;
        while (timer > 0) 
        {             
            yield return new WaitForSeconds(time_Interval);
            rCamera.targetTexture = rt[iterator % rt.Length];
            AfterRawImage[Mathf.Min(iterator, AfterRawImage.Length - 1)].SetActive(true);
            AfterRawImage[Mathf.Min(iterator, AfterRawImage.Length - 1)].GetComponent<RawImage>().texture = rt[iterator % rt.Length];                
            iterator++;
            timer -= time_Interval;
        }
        rCamera.targetTexture = null;
        yield return new WaitForSeconds(time_total);
        foreach(GameObject a in AfterRawImage)
        {
            a.SetActive(false);
        }
        rt = null;
    }*/
}

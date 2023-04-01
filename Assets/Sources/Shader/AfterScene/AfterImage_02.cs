using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterImage_02 : MonoBehaviour
{
    private bool StartAfterImage = false;
    private RenderTexture[] rt;
    [SerializeField] private int RawImgCount;
    [SerializeField] private Camera rCamera;
    [SerializeField] private RawImage[] rawImages;
    [SerializeField] private float time_Interval;
    [SerializeField] private GameObject AI_Canvas;
    [HideInInspector]public bool isPlaying;
    public RenderTexture rCamTex;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in rawImages)
        {
            item.color = new Color(1, 1, 1, 0);
            item.texture = null;
            AI_Canvas.SetActive(false);
        }                
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isPlaying)
        {
            StartAfterImage = !StartAfterImage;
            if (StartAfterImage)
            {

                AI_Canvas.SetActive(true);
                rt = new RenderTexture[RawImgCount];
                for (int i = 0; i < RawImgCount; i++)
                {
                    rt[i] = new RenderTexture(rCamera.pixelWidth, rCamera.pixelHeight, 16);
                }
                StartCoroutine(AfterImageEffect());
            }
            else
            {
                AI_Canvas.SetActive(false);
                rt = null;
                rCamera.targetTexture = rCamTex;
                StopCoroutine(AfterImageEffect());
                foreach (var item in rawImages)
                {
                    item.color = new Color(1, 1, 1, 0);
                    item.texture = null;
                }
            }
            isPlaying = false;
        }
    }

    private IEnumerator AfterImageEffect()
    {
        int iterator = 0;
        while(StartAfterImage)
        {           
            yield return new WaitForSeconds(time_Interval);
            if(rt != null && rt[iterator % RawImgCount] != null)
            {
                rCamera.targetTexture = rt[iterator % RawImgCount];
                rawImages[iterator % rawImages.Length].texture = rt[iterator % RawImgCount];
                rawImages[iterator % rawImages.Length].GetComponent<AfterImage_Fade_02>().Init();
            }
            iterator++;
        }
        yield return null;
    }
}

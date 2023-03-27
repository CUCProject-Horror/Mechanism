using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class AfterImage : MonoBehaviour
    {
        //private float time_total;    
        public float time_Interval;
        [SerializeField] private int _downRes = 1;
        public GameObject AfterRawImage;
        public Transform RawImageCanvas;
        private RenderTexture[] rt;
        public int RenderTextureCount;
        public Camera rCamera;
        public RenderTexture rCamTex;
        private bool start_afterImageEffect;

        public bool isPlaying;

        private void Start()
        {
            start_afterImageEffect = false;
        }

        private void LateUpdate()
        {
            if (isPlaying)
            {
                start_afterImageEffect = !start_afterImageEffect;
                if (start_afterImageEffect)
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
                    rCamera.targetTexture = rCamTex;
                    StopCoroutine(StartAfterImageEffect());

                }
                isPlaying = false;
            }
        }



        public IEnumerator StartAfterImageEffect()
        {
            int iterator = 0;
            while (start_afterImageEffect)
            {
                yield return new WaitForSeconds(time_Interval);
                if (rt != null)
                {
                    rCamera.targetTexture = rt[iterator % rt.Length];
                    GameObject temp = Instantiate(AfterRawImage, new Vector3(0, 0, 0), Quaternion.identity);
                    temp.transform.parent = RawImageCanvas;
                    temp.GetComponent<RawImage>().texture = rt[iterator % rt.Length];
                }
                iterator++;
            }
            yield return null;
        }
    }
}

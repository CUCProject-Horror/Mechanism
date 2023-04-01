using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterImage_Fade_02 : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)] private float InitAlpha;
    [SerializeField] private float Fadetime;

    private float time_left;
    public void Init()
    {
        if(this.GetComponent<RawImage>().texture != null)
        {
            this.GetComponent<RawImage>().color = new Color(1, 1, 1, InitAlpha);
            time_left = InitAlpha;
        }        
    }

    private void LateUpdate()
    {
        if(this.GetComponent<RawImage>().texture != null)
        {
            Color NowColor = this.GetComponent<RawImage>().color;
            time_left -= Time.deltaTime * InitAlpha / Fadetime;
            NowColor.a = Mathf.Max(0, time_left);
            this.GetComponent<RawImage>().color = NowColor;
        }
        
    }
}

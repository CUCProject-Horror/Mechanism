using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterImageFade : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)] private float InitAlpha;

    [SerializeField] private float Fadetime;

    private RawImage RI;
    private float time_left;
     
    public void Start()
    {
        RI = this.GetComponent<RawImage>();
        Color OriginColor = RI.color;
        OriginColor.a = InitAlpha;
        RI.color = OriginColor;
        time_left = InitAlpha;         
    }

    private void LateUpdate()
    {
        StartCoroutine(TimetoDestroy());
        Color NowColor = RI.color;
        time_left -= Time.deltaTime * InitAlpha / Fadetime * 0.05f;
        NowColor.a = Mathf.Max(0, time_left);
        RI.color = NowColor;
        //Debug.Log(this.GetComponent<RawImage>().color.a);
    }

    private IEnumerator TimetoDestroy()
    {
        yield return new WaitForSeconds(Fadetime);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageAnim : MonoBehaviour
{
    private RawImage m_rawImage;

    //增量
    private float m_offsetx;
    private float m_offsety;


    void Start()
    {
        m_rawImage = GetComponent<RawImage>();
        //计算增量
        m_offsetx = 1 / 4f;
        m_offsety = 1 / 2f;
        StartCoroutine(payAni());
    }


    IEnumerator payAni()
    {
        float x = 0;
        float y = 0;
        while (true)
        {
            y += m_offsety;
            while (x < 1)
            {
                x += m_offsetx;
                m_rawImage.uvRect = new Rect(x, y, m_rawImage.uvRect.width, m_rawImage.uvRect.height);
                yield return new WaitForSeconds(0.2f);
            }
            x = 0;
        }
    }
}

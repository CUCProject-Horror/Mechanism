using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkPlane : MonoBehaviour
{
    Image dark;

    private void Start()
    {
        dark = GetComponent<Image>();
    }

    private void Update()
    {
        if (dark.color.a >= 0)
        {
            dark.color = dark.color - new Color(0, 0, 0, 1) * Time.deltaTime;
        }
    }
}

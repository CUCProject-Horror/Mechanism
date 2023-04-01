using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAngleFix : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
    }
}

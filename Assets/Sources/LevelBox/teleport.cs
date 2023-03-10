using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public Transform teleportPoint;
    public CharacterController cr;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            cr.enabled = false;
            a.SetActive(false);
            b.SetActive(false);
            cr.transform.position = teleportPoint.position;
            cr.gameObject.transform.Rotate(0, 180, 0);
            cr.enabled = true;
        }
    }
}

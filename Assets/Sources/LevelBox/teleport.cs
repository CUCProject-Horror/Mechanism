using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class teleport : MonoBehaviour
    {
        //public GameObject a;
        //public GameObject b;
        //public GameObject c;
        //public GameObject d;
        //public GameObject e;
        public Transform teleportPoint;
        public CharacterController cr;
        public UnityEvent onTeleporting;
        public void PlayerTeleport()
        {
            onTeleporting.Invoke();
            cr.enabled = false;
            cr.transform.position = teleportPoint.position;
            cr.transform.rotation = teleportPoint.rotation;
            cr.enabled = true;
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.gameObject.tag == "Player")
        //    {
        //        //a.SetActive(false);
        //        //b.SetActive(false);
        //        //c.SetActive(true);
        //        //d.SetActive(true);
        //        //e.SetActive(false);
        //        PlayerTeleport();
        //    }
        //}
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AfterImageController : MonoBehaviour
    {
        public GameObject af;
        public Camera afCam;

        public float afTime;

        public void StartAfterImage()
        {
            StartCoroutine(ChangeAfState());
        }

        public IEnumerator ChangeAfState()
        {
            af.GetComponent<AfterImage_02>().isPlaying = true;
            afCam.enabled = true;
            //afCam.depth = 1;
            yield return new WaitForSeconds(afTime);
            af.GetComponent<AfterImage_02>().isPlaying = true;
            //afCam.depth = -1;
            afCam.enabled = false;
        }
    }
}
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
            af.GetComponent<AfterImage>().isPlaying = true;
            afCam.enabled = true;
            yield return new WaitForSeconds(afTime);
            af.GetComponent<AfterImage>().isPlaying = true;
            afCam.enabled = false;
        }
    }
}
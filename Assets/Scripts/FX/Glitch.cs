using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Glitch : MonoBehaviour
    {
        [HideInInspector] public bool canGlitch = false;
        public GameObject glitchInteractor;

        public void ChangeGlitchState()
        {
            canGlitch = true;
        }
        public void GlitchTransfer()
        {
             if (canGlitch)
            {
                Debug.Log("Glitch!");
                GetComponent<GlitchSwitch>().Switch = true;
            }
        }

        private void Update()
        {
            if (!canGlitch)
            {
                glitchInteractor.SetActive(false);
            }
            else if (canGlitch)
            {
                glitchInteractor.SetActive(true);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;

namespace Game
{
    public class VidPlayer : MonoBehaviour
    {
        public GameObject ScreenToPlay;
        public GameObject ScreenDark;
        public VideoClip clipToPlay;
        [System.NonSerialized] public VideoPlayer vp;
        // Start is called before the first frame update
        void Start()
        {
            vp = ScreenToPlay.GetComponent<VideoPlayer>();
        }

        public void PlayVid()
        {
            ScreenToPlay.SetActive(true);
            ScreenDark.SetActive(true);
            vp.clip = clipToPlay;
            vp.enabled = true;
            ScreenToPlay.GetComponent<PlayerInput>().enabled = true;
        }

        
    }
}

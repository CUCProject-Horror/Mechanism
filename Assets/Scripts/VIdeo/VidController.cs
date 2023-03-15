using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.InputSystem;

namespace Game
{
    public class VidController : MonoBehaviour
    {
        public GameObject ScreenToPlay;
        public GameObject ScreenDark;
        VideoPlayer vp;
        public int playSpeed;

        bool isPause;
        public long vidFrame;

        private void Awake()
        {
            vp = ScreenToPlay.GetComponent<VideoPlayer>();
            playSpeed = 1;
            isPause = false;
        }
        public void OnQuit()
        {
            ScreenToPlay.SetActive(false);
            ScreenDark.SetActive(false);
            isPause = false;
            playSpeed = 1;
            vp.clip = null;
            vp.enabled = false;
            ScreenToPlay.GetComponent<PlayerInput>().enabled = false;
        }

        public void OnPause()
        {
            isPause = !isPause;
        }

        public void OnBackward()
        {

            if(playSpeed > 0)
            playSpeed--;
        }

        public void OnForward()
        {
            if (playSpeed < 2)
            playSpeed++;
        }

        void FixedUpdate()
        {
            vidFrame = vp.frame;

            if (isPause)
            {
                vp.Pause();
                playSpeed = 1;
            }
            else if(!isPause)
            {
                vp.Play();   
            }

            if (playSpeed == 1)
            {
                vp.playbackSpeed = 1;
            }
            else if (playSpeed == 2)
            {
                vp.playbackSpeed = 2;
            }
            else if(playSpeed == 0)
            {
                //反正会倒放
                vp.frame = vp.frame - 1 * (long)vp.frameRate;
            }
        }

        public void PlayVidInBag(VideoClip clipToPlay)
        {
            ScreenToPlay.SetActive(true);
            ScreenDark.SetActive(true);
            vp.clip = clipToPlay;
            vp.enabled = true;
            ScreenToPlay.GetComponent<PlayerInput>().enabled = true;
        }
    }
}
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

namespace Game {
    public class VidController : MonoBehaviour
    {
        public GameObject ScreenToPlay;
        public GameObject ScreenDark;
        VideoPlayer vp;
        public int playSpeed;

        public bool isPause;
        public long vidFrame;
        public bool isInventory;

        public UnityEvent endTVState;
        public UnityEvent endTVStateInventory;

        Animator tvScreenAnim;

        private void Awake()
        {
            vp = ScreenToPlay.GetComponent<VideoPlayer>();
            tvScreenAnim = GetComponent<Animator>();

            tvScreenAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
            playSpeed = 1;
            isPause = false;
        }
        public void Quit()
        {
            if (isInventory)
            { 
                endTVStateInventory.Invoke();
            }
            else
            { endTVState.Invoke(); }
            ScreenToPlay.SetActive(false);
            ScreenDark.SetActive(false);
            isPause = false;
            playSpeed = 1;
            vp.clip = null;
            vp.enabled = false;
            isInventory = false;
        }

        public void Pause()
        {
            isPause = !isPause;
        }

        public void Backward()
        {

            if(playSpeed > 0)
            playSpeed--;
        }

        public void Forward()
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
                vp.frame = vp.frame - 1 * (long)vp.frameRate;
            }
        }

        public void PlayVidInBag(VideoClip clipToPlay)
        {
            ScreenToPlay.SetActive(true);
            ScreenDark.SetActive(true);
            vp.clip = clipToPlay;
            vp.enabled = true;
        }
    }
}

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
            vp.playbackSpeed = 1;
            vp.clip = null;
            vp.enabled = false;
            isInventory = false;
        }

        public void Pause()
        {
            isPause = !isPause;
            if (isPause)
            {
                vp.Pause();
                vp.playbackSpeed = 1;
            }
            else if (!isPause)
            {
                vp.Play();
            }
        }

        public void Backward()
        {

            if (vp.playbackSpeed > -1)
                vp.playbackSpeed--;
        }

        public void Forward()
        {
            if (vp.playbackSpeed < 2)
                vp.playbackSpeed++;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{
    public GameObject ScreenToPlay;
    public GameObject ScreenDark;
    [System.NonSerialized] public VideoPlayer vp;
    // Start is called before the first frame update
    void Start()
    {
        vp = this.gameObject.GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    ScreenToPlay.SetActive(false);
        //    ScreenDark.SetActive(false);
        //    vp.enabled = false;
        //}
    } 
    public void PlayVid()
    {
        ScreenToPlay.SetActive(true);
        ScreenDark.SetActive(true);
        vp.enabled = true;
    }
}

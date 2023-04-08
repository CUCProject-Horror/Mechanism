using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixerTest : MonoBehaviour
{
    public AudioSource thisSource;

    // Start is called before the first frame update
    void Start()
    {
        thisSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //thisSource.outputAudioMixerGroup
    }
}

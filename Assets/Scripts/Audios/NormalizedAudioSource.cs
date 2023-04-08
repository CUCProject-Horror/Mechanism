using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    public class NormalizedAudioSource : MonoBehaviour
    {
        bool isPlayingAudio = false;

        public void Update()
        {
            if(this.GetComponent<AudioSource>().isPlaying)
            {
                isPlayingAudio = true;
            }

            if(!this.GetComponent<AudioSource>().isPlaying && isPlayingAudio)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

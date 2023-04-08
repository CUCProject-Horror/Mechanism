using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/AudioClipArray")]
    public class AudioClipArray : ScriptableObject
    {
        public AudioClip[] clipArray;
    }
}
using UnityEngine;
using UnityEngine.Video;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/GameAgent")]
    public class GameAgent : ScriptableObject
    {
        GameManager game => GameManager.instance;
        public void DebugLog(string message) => Debug.Log(message);
        public void PlayVideo(VideoClip clip) {
            game.vidPlayer.vp.clip = clip;
            game.vidPlayer.PlayVid();
        }
    }
}
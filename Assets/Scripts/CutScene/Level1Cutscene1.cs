using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Level1Cutscene1 : MonoBehaviour
    {
        public float titleWaitTime;
        public float titleShowTime;
        public float guassianBlurTime;
        public float soundFXWaitTime;

        public UnityEvent OnGaussianEnd;

        public IEnumerator CutsceneWalkThrough()
        {
            Debug.Log("演出开始，屏幕从全黑渐隐，高斯模糊开始，Invalid角色控制器");
            yield return new WaitForSeconds(titleWaitTime);
            Debug.Log("游戏标题出现！");
            yield return new WaitForSeconds(titleShowTime);
            Debug.Log("游戏标题消失，高斯模糊渐隐，且灯光远去");
            yield return new WaitForSeconds(guassianBlurTime);
            Debug.Log("高斯模糊结束!交还角色控制器，大门出现");
            OnGaussianEnd.Invoke();
            yield return new WaitForSeconds(soundFXWaitTime);
            Debug.Log("标志性音效浮现！");

        }

        #region Life Cycle
        private void Start()
        {
            StartCoroutine(CutsceneWalkThrough());
        }
        #endregion
    }
}

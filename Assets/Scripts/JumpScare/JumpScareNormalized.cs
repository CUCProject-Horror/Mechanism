using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Game
{
    public class JumpScareNormalized : MonoBehaviour
    {
        public UnityEvent OnStartScare;
        public UnityEvent OnScare;
        public UnityEvent OnEndScare;

        public float ScareTime;
        public float screenTwistTime;

        public void StartScare()
        {
            StartCoroutine(JumpScare1_1());
        }

        public void EndScare()
        {
            OnEndScare.Invoke();
        }

        public IEnumerator JumpScare1_1()
        {
            OnStartScare.Invoke();
            yield return new WaitForSeconds(ScareTime);

            yield return new WaitForSeconds(screenTwistTime);
        }
    }
}

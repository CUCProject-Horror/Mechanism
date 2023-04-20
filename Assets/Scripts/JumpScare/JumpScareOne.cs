using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpScareOne : MonoBehaviour
{
    public UnityEvent OnStartScare;
    public UnityEvent OnEndScare;

    public float ScareTime;
    public float screenTwistTime;

    public void StartScare()
    {
        OnStartScare.Invoke();
    }

    public void EndScare()
    {
        OnEndScare.Invoke();
    }

    public IEnumerator JumpScare1_1()
    {
        yield return new WaitForSeconds(ScareTime);

        yield return new WaitForSeconds(screenTwistTime);
    }
}

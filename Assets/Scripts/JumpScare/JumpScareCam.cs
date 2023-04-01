using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpScareCam : MonoBehaviour
{
    public Animator camAnim;

    public UnityEvent OnStartScare;
    public UnityEvent OnEndScare;

    public void StartScare()
    {
        OnStartScare.Invoke();
        camAnim.SetTrigger("Scared");
    }

    public void EndScare()
    {
        OnEndScare.Invoke();
    }
}

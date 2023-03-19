using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DarkScreenText : MonoBehaviour
{
    public UnityEvent endControll;
    float timer = 0;
    bool startControll = false;
    public float controllTime;

    private void Update()
    {
        if(startControll)
        {
            timer += Time.deltaTime;
        }

        if(timer >= controllTime)
        {
            endControll.Invoke();
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }

    public void StartControll()
    {
        startControll = true;
    }
}

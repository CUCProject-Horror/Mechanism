using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class DarkScreenText : MonoBehaviour
    {
        public GameObject darkScreen;
        float timer = 0;
        bool startControll = false;
        public float controllTime;

        public UnityEvent endTextControll;
        public UnityEvent startTextControll;

        private void Update()
        {
            if (startControll)
            {
                timer += Time.deltaTime;
            }

            if (timer >= controllTime)
            {
                darkScreen.GetComponent<Animator>().SetTrigger("Exit");
                endTextControll.Invoke();
                Destroy(this.gameObject.transform.parent.gameObject);
            }
        }

        public void StartControll()
        {
            darkScreen.GetComponent<Animator>().SetTrigger("Enter");
            startTextControll.Invoke();
            startControll = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DoorAnimator : MonoBehaviour
    {
        Animator handle;

        private void Start()
        {
            handle = this.gameObject.GetComponent<Animator>(); 
        }
        public void LockAnim()
        {
            handle.SetTrigger("isLocked");
        }

        public void OpenAnim()
        {
            handle.SetTrigger("isOpening");
        }
    }
}

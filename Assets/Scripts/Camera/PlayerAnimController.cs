using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class PlayerAnimController : MonoBehaviour
    {
        public Animator playerFallAnim;
        public Animator playerWalkAnim;
        public GameObject player;
        Vector2 playerMoveAmount;
        //bool isPlayingAudio = false;
        public bool canPlayFootStepAud;

        public float walkAudioGap;
        public UnityEvent playFootStepAudio;

        void FixedUpdate()
        {
            playerMoveAmount = new Vector2(player.GetComponent<CharacterController>().velocity.x, player.GetComponent<CharacterController>().velocity.z);
            playerWalkAnim.speed = playerMoveAmount.magnitude * 0.4f;

            PlayerDropAnim();
        }

        void PlayerDropAnim()
        {
            if (player.GetComponent<CharacterController>().isGrounded && player.GetComponent<Protagonist>().fallingHeight >= 2.0f)
            {   
                playerFallAnim.SetTrigger("FallGround");
            }
        }

        IEnumerator FootStepAudio()
        {
            playFootStepAudio.Invoke();
            yield return null;
        }
    }
}

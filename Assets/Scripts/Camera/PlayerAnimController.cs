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
        bool isPlayingAudio = false;

        public UnityEvent playFootStepAudio;

        void FixedUpdate()
        {
            playerMoveAmount = new Vector2(player.GetComponent<CharacterController>().velocity.x, player.GetComponent<CharacterController>().velocity.z);
            playerWalkAnim.speed = playerMoveAmount.magnitude * 0.4f;

            PlayerDropAnim();

            if (playerMoveAmount.magnitude > 0 && !isPlayingAudio)
            {
                StartCoroutine(FootStepAudio());
            }
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
            isPlayingAudio = true;
            playFootStepAudio.Invoke();
            yield return new WaitForSeconds(1.3f / playerMoveAmount.magnitude);
            isPlayingAudio = false;
        }
    }
}

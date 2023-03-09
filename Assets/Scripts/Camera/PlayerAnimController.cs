using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerAnimController : MonoBehaviour
    {
        public Animator playerFallAnim;
        public Animator playerWalkAnim;
        public GameObject player;
        Vector2 playerMoveAmount;

        void FixedUpdate()
        {
            playerMoveAmount = new Vector2(player.GetComponent<CharacterController>().velocity.x, player.GetComponent<CharacterController>().velocity.z);
            playerWalkAnim.speed = playerMoveAmount.magnitude * 0.4f;

            PlayerDropAnim();//角色落地动画
        }

        void PlayerDropAnim()//角色落地动画
        {
            if (player.GetComponent<CharacterController>().isGrounded && player.GetComponent<Protagonist>().fallingHeight >= 2.0f)
            {   
                playerFallAnim.SetTrigger("FallGround");
            }
        }
    }
}

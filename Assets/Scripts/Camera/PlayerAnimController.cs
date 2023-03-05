using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerAnimController : MonoBehaviour
{
    public Animator playerFallAnim;
    public Animator playerWalkAnim;
    public GameObject player;
    Vector3 playerMoveAmount;
    public float dropTimer = 0;

    public bool falling;
    // Start is called before the first frame update
    void Start()
    {
        falling = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveAmount = player.GetComponent<CharacterController>().velocity;
        playerWalkAnim.speed = playerMoveAmount.magnitude * 0.4f;

        //if (player.GetComponent<CharacterController>().isGrounded == false)
        //{ falling = true; }

        //PlayerDropAnim();//角色落地动画
    }

    void PlayerDropAnim()//角色落地动画
    {
        if (player.GetComponent<CharacterController>().isGrounded && falling)
        {
            playerFallAnim.SetTrigger("FallGround");
            falling = false;
        }
        //if (player.GetComponent<PlayerController>().isGround != true)
        //{ dropTimer += Time.deltaTime; }
        //else if(player.GetComponent<PlayerController>().isGround)
        //{
        //    if (dropTimer >= 0.3f)
        //    playerFallAnim.SetTrigger("FallGround");
        //    dropTimer = 0;
        //}
    }
}

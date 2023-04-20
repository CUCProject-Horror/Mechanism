using System.Collections.Generic;
using Avocado.DeveloperCheatConsole.Scripts.Core;
using Avocado.DeveloperCheatConsole.Scripts.Core.Commands;
using UnityEngine;

    public class LevelOneCommand : MonoBehaviour
    {
        public GameObject player;
        public Transform jumpScareOne;

        private void Awake()
        {
            DeveloperConsole.Instance.AddCommand(new DevCommand("TP_JumpScare1_1", "Teleport to jump scare1_1 position", () => {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = jumpScareOne.position;
                player.GetComponent<CharacterController>().enabled = true;
            }));//´«ËÍµ½1_1JumpScare´¦
        }
    }

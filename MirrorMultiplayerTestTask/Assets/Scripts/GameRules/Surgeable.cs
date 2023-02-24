using System.Collections;
using System.Collections.Generic;
using Mirror;
using MirrorMultiplayerTestTask.Movement;
using UnityEngine;

namespace MirrorMultiplayerTestTask.GameRules
{
    public class Surgeable : NetworkBehaviour
    {
        [SerializeField] private PlayerController playerController;

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            var hitPlayerController = other.gameObject.GetComponent<PlayerController>();
            if (hitPlayerController == null || hitPlayerController.IsInvincible)
            {
                return;
            }
            if (playerController.IsSurging && !hitPlayerController.IsSurging)
            {
                hitPlayerController.IsInvincible = true;
                playerController.IncreaseScore();
            }
        }
    }}
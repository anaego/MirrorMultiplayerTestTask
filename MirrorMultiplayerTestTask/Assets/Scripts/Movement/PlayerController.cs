using System;
using Mirror;
using MirrorMultiplayerTestTask.GameRules;
using UnityEngine;

namespace MirrorMultiplayerTestTask.Movement
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Rigidbody))]
    // TODO separate into movement and input script
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private PlayerScore playerScore;
        [SerializeField] private PlayerColor playerColor;
        [SerializeField] private MovementSettingsScriptableObject movementSettings;
        [SerializeField] private GameRulesSettingsScriptableObject gameRulesSettings;

        private float horizontal;
        private float vertical;
        private Vector3 direction;

        private bool isSurging;
        private float currentSurgeDistance;
        private Vector3 lastFramePosition;
        private Vector3 currentFramePosition;

        [SyncVar]
        private bool isInvincible;
        private float currentInvincibilityDuration;
        private float invincibilityStartTime;

        public bool IsSurging => isSurging;
        public bool IsInvincible 
        { 
            get 
            { 
                return isInvincible; 
            } 
            set
            {
                if (value && currentInvincibilityDuration == 0)
                {
                    invincibilityStartTime = Time.time;
                }
                if (!value)
                {
                    currentInvincibilityDuration = 0;
                }
                isInvincible = value;
                // TODO move this 
                playerColor.CurrentColor = value ? playerColor.HitColor : playerColor.DefaultColor;
            }
        }

        void OnValidate()
        {
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }
            characterController.enabled = false;
            // TODO remove?
            //characterController.skinWidth = 0.02f;
            //characterController.minMoveDistance = 0f;
            //GetComponent<Rigidbody>().isKinematic = true;
            this.enabled = false;
        }

        public override void OnStartAuthority()
        {
            characterController.enabled = true;
            this.enabled = true;
        }

        public override void OnStopAuthority()
        {
            this.enabled = false;
            characterController.enabled = false;
        }

        void Update()
        {
            if (!characterController.enabled)
            {
                return;
            }
            ProcessTurning();
            ProcessMoving();
            if (!IsInvincible)
            {
                return;
            }
            ProcessInvincibility();
        }

        internal void IncreaseScore()
        {
            playerScore.IncreaseScore();
        }

        private void ProcessTurning()
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * movementSettings.HorizontalTurnSpeed);
        }

        private void ProcessMoving()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            direction = new Vector3(horizontal, 0f, vertical);
            direction = Vector3.ClampMagnitude(direction, 1f);
            direction = transform.TransformDirection(direction);
            direction *= movementSettings.MoveSpeedMultiplier;
            ProcessSurging();
            characterController.Move(direction * Time.deltaTime);
        }

        private void ProcessSurging()
        {
            if (Input.GetMouseButtonDown(0) && !isSurging)
            {
                isSurging = true;
                currentFramePosition = transform.position;
                lastFramePosition = currentFramePosition;
            }
            if (isSurging)
            {
                if (currentSurgeDistance >= movementSettings.SurgeDistance)
                {
                    isSurging = false;
                    currentSurgeDistance = 0;
                }
                else
                {
                    currentFramePosition = transform.position;
                    currentSurgeDistance += Vector3.Distance(currentFramePosition, lastFramePosition);
                    lastFramePosition = currentFramePosition;
                    direction = direction == Vector3.zero ? transform.forward : direction;
                    direction *= movementSettings.SurgeSpeedMultiplier;
                }
            }
        }

        private void ProcessInvincibility()
        {
            Debug.LogWarning("Started processing invinvibility");
            currentInvincibilityDuration = Time.time - invincibilityStartTime;
            if (currentInvincibilityDuration >= gameRulesSettings.InvincibilityDurationSeconds)
            {
                Debug.LogWarning($"Finished processing invinvibility, {currentInvincibilityDuration} >= {gameRulesSettings.InvincibilityDurationSeconds}");
                IsInvincible = false;
                return;
            }
        }
    }
}

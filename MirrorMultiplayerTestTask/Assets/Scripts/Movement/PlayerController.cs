using Mirror;
using UnityEngine;

namespace MirrorMultiplayerTestTask.Movement
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private CharacterController characterController;
        // TODO move to a settings / data file?
        [SerializeField] private float moveSpeedMultiplier = 8f;
        [SerializeField] private float turnSpeed;

        private float horizontal;
        private float vertical;
        private Vector3 direction;

        void OnValidate()
        {
            if (characterController == null)
            {
                characterController = GetComponent<CharacterController>();
            }
            characterController.enabled = false;
            characterController.skinWidth = 0.02f;
            characterController.minMoveDistance = 0f;
            GetComponent<Rigidbody>().isKinematic = true;
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
            HandleTurning();
            HandleMove();
        }

        void HandleTurning()
        {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * turnSpeed);
        }

        void HandleMove()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            direction = new Vector3(horizontal, 0f, vertical);
            direction = Vector3.ClampMagnitude(direction, 1f);
            direction = transform.TransformDirection(direction) * moveSpeedMultiplier;
            characterController.Move(direction * Time.deltaTime);
        }
    }
}

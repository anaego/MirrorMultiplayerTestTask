using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO change to a separate camera folder?
namespace MirrorMultiplayerTestTask.Movement
{
    public class PlayerCamera : NetworkBehaviour
    {
        [SerializeField] private MovementSettingsScriptableObject movementSettings;
        
        private Camera mainCamera;

        void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            // TODO clamp / change to rotate around player pivot?
            // TODO Rotate around
            mainCamera.transform.Rotate(new Vector3(- Input.GetAxis("Mouse Y"), 0, 0) * Time.deltaTime * movementSettings.VerticalTurnSpeed);
        }

        public override void OnStartLocalPlayer()
        {
            if (mainCamera != null)
            {
                mainCamera.orthographic = false;
                mainCamera.transform.SetParent(transform);
                mainCamera.transform.localPosition = new Vector3(0f, 3f, -8f);
                mainCamera.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
            }
            else
            {
                Debug.LogWarning("PlayerCamera: Could not find a camera in scene with 'MainCamera' tag.");
            }
        }

        public override void OnStopLocalPlayer()
        {
            if (mainCamera != null)
            {
                mainCamera.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(mainCamera.gameObject, SceneManager.GetActiveScene());
                mainCamera.orthographic = true;
                mainCamera.orthographicSize = 15f;
                mainCamera.transform.localPosition = new Vector3(0f, 70f, 0f);
                mainCamera.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            }
        }
    }
}

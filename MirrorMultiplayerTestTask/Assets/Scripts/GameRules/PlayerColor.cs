using Mirror;
using UnityEngine;

namespace MirrorMultiplayerTestTask.Movement
{
    public class PlayerColor : NetworkBehaviour
    {
        [SerializeField] private Color32 defaultColor = Color.black;
        [SerializeField] private Color32 hitColor = Color.white;

        // Color32 packs to 4 bytes
        [SyncVar(hook = nameof(SetColor))]
        private Color32 color = Color.black;

        private Material cachedMaterial;

        void SetColor(Color32 oldColor, Color32 newColor)
        {
            if (cachedMaterial == null)
            {
                cachedMaterial = GetComponentInChildren<Renderer>().material;
            }
            cachedMaterial.color = newColor;
        }

        void OnDestroy()
        {
            Destroy(cachedMaterial);
        }
    }
}

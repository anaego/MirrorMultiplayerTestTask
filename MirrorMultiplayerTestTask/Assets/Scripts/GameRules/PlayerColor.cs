using Mirror;
using UnityEngine;

namespace MirrorMultiplayerTestTask.Movement
{
    public class PlayerColor : NetworkBehaviour
    {
        // TODO move to settings?
        public Color32 DefaultColor = Color.black;
        public Color32 HitColor = Color.white;

        [SyncVar(hook = nameof(SetColor))]
        public Color32 CurrentColor = Color.black;

        // TODO assign instead of getting
        private Material cachedMaterial;

        void SetColor(Color32 oldColor, Color32 newColor)
        {
            if (cachedMaterial == null)
            {
                cachedMaterial = GetComponentInChildren<Renderer>().material;
            }
            cachedMaterial.color = newColor;
            // TODO remove all debug logs
            Debug.LogWarning($"Changed color");
        }

        void OnDestroy()
        {
            Destroy(cachedMaterial);
        }
    }
}

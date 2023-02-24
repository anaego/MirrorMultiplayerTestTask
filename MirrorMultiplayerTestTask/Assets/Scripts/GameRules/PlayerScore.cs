using Mirror;
using UnityEngine;

namespace MirrorMultiplayerTestTask.GameRules
{
    public class PlayerScore : NetworkBehaviour
    {
        [SyncVar] private int index;
        [SyncVar] private uint score;

        public int Index { get; set; }

        void OnGUI()
        {
            GUI.Box(new Rect(10f + (index * 110), 10f, 100f, 25f), $"P{index}: {score:0000000}");
        }

        public void IncreaseScore()
        {
            score += 1;
        }
    }
}

using Mirror;
using MirrorMultiplayerTestTask.GameRules;
using UnityEngine;

namespace MirrorMultiplayerTestTask.Lobby
{
    public class NetworkRoomManagerLobby : NetworkRoomManager
    {
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            // TODO change to handle index / names better?
            PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
            playerScore.Index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
            return true;
        }

        bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
            showStartButton = true;
#endif
        }

        public override void OnGUI()
        {
            base.OnGUI();
            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                showStartButton = false;
                ServerChangeScene(GameplayScene);
            }
        }
    }
}

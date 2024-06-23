using Game.Scripts.Lobby;
using Unity.Services.Lobbies;
using UnityEngine;

namespace Game.Scripts.Handler {
    public class CreateRoomHandler : MonoBehaviour {

        private RoomSettings _settings = new("Room",4);
        private void OnEnable() {
            Create();
            LobbyManager.Instance.OnCreateRoomFailed += ReCreate;
        }
        
        private void OnDisable() {
            LobbyManager.Instance.OnCreateRoomFailed -= ReCreate;
        }
        
        private void ReCreate(LobbyExceptionReason obj) {
            Create();
        }

        public void Create() {
            LobbyManager.Instance.CreateRoom(_settings.roomName, _settings.maxPlayers);
        }
    }

    public struct RoomSettings {
        public string roomName;
        public int maxPlayers;

        public RoomSettings(string roomName, int maxPlayers) {
            this.roomName = roomName;
            this.maxPlayers = maxPlayers;
        }
    }
}
using Game.Scripts.LobbySystem.Controller;
using Game.Scripts.LobbySystem.Service;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Game.Scripts.LobbySystem {
    [DefaultExecutionOrder(-999)]
    public class CallbackLobbyManager : LobbyManager {
        public override void Awake() {
            base.Awake();
            CreateRoomService.OnSuccess += UpdateLobby;
            GetLobbyService.OnSuccess += CheckLobby;
            JoinRoomService.OnSuccess += UpdateLobby;

            GetLobbyService.OnFailed += UpdateLobby;
            
            DeleteRoomService.OnSuccess += UpdateLobby;
            KickHandler.OnSuccess += UpdateLobby;
            RemovePlayerService.OnSuccess += RemovePlayerUpdate;
        }



        private void OnDestroy() {
            CreateRoomService.OnSuccess -= UpdateLobby;
            GetLobbyService.OnSuccess -= CheckLobby;
            JoinRoomService.OnSuccess -= UpdateLobby;
            
            GetLobbyService.OnFailed -= UpdateLobby;
            
            DeleteRoomService.OnSuccess -= UpdateLobby;
            KickHandler.OnSuccess -= UpdateLobby;
            RemovePlayerService.OnSuccess -= RemovePlayerUpdate;
        }
        
        private void CheckLobby(Lobby lobby) {
            Data.Lobby = lobby;
        }

        private void UpdateLobby(Lobby lobby) {
            Data.Lobby = lobby;
        }
        
        private void UpdateLobby(LobbyExceptionReason obj) {
            if (obj == LobbyExceptionReason.LobbyNotFound) {
                Data.Lobby = null;
            }
        }
        
        private void UpdateLobby() {
            Data.Lobby = null;
        }
        
        private void RemovePlayerUpdate() {
            if (IsInLobby(Data.Lobby)) return;
            
            Data.Lobby = null;
        }
    }
}
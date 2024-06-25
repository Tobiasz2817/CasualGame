using Game.Scripts.LobbySystem.Service;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System;

namespace Game.Scripts.LobbySystem.Controller {
    public class KickHandler : MonoBehaviour {
        public static event Action OnSuccess;
        
        
        private void OnEnable() {
            GetLobbyService.OnSuccess += CheckLobby;
            GetLobbyService.OnFailed += CheckReason;
        }
    
        private void OnDisable() {
            GetLobbyService.OnSuccess -= CheckLobby;
            GetLobbyService.OnFailed -= CheckReason;
        }
    
        private void CheckLobby(Lobby lobby) {
            if (lobby == null) return;
            
            var isInLobby = LobbyManager.Instance.IsInLobby(lobby);
            if (!isInLobby) {
                OnSuccess?.Invoke();
            }
        }

        private void CheckReason(LobbyExceptionReason obj) {
            if (obj == LobbyExceptionReason.Forbidden || obj == LobbyExceptionReason.LobbyNotFound) 
                OnSuccess?.Invoke();
        }
    }
}
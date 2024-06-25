using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using System;
using UnityEngine;

namespace Game.Scripts.LobbySystem.Service {
    public static class JoinRoomService {
        public static event Action OnExecute;
        public static event Action<Lobby> OnSuccess;
        public static event Action<LobbyExceptionReason> OnFailed;

        public static readonly RateLimited Limited = new(6f, 2);
        public static readonly RateLimited QuickLimited = new(10f, 1);
        
        public static async Task JoinRoom(string enterCode, JoinLobbyByCodeOptions options = null) {
            try {
                if (!Limited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                Limited.Call();
                
                OnExecute?.Invoke();
                
                var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(enterCode, options);
                OnSuccess?.Invoke(lobby);
            }
            catch (LobbyServiceException e) {
                OnFailed?.Invoke(e.Reason);
                throw new LobbyServiceException(e);
            }
            catch (Exception) {
                OnFailed?.Invoke(LobbyExceptionReason.Unknown);
                throw new Exception(LobbyExceptionReason.Unknown.ToString());
            }
        }
        
        public static async Task JoinRoom(string  lobbyId, JoinLobbyByIdOptions options = null) {
            try {
                
                if (!Limited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                Limited.Call();
                
                OnExecute?.Invoke();
                
                var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options);
                OnSuccess?.Invoke(lobby);
            }
            catch (LobbyServiceException e) {
                OnFailed?.Invoke(e.Reason);
                throw new LobbyServiceException(e);
            }
            catch (Exception) {
                OnFailed?.Invoke(LobbyExceptionReason.Unknown);
                throw new Exception(LobbyExceptionReason.Unknown.ToString());
            }
        }
        
        public static async Task QuickJoinRoom(QuickJoinLobbyOptions options = null) {
            try {
                if (!QuickLimited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                QuickLimited.Call();
                
                OnExecute?.Invoke();
                
                var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
                OnSuccess?.Invoke(lobby);
            }
            catch (LobbyServiceException e) {
                OnFailed?.Invoke(e.Reason);
                throw new LobbyServiceException(e);
            }
            catch (Exception) {
                OnFailed?.Invoke(LobbyExceptionReason.Unknown);
                throw new Exception(LobbyExceptionReason.Unknown.ToString());
            }
        }
    }
}

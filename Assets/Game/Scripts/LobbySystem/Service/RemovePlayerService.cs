using System.Threading.Tasks;
using Unity.Services.Lobbies;
using System;
using UnityEngine;

namespace Game.Scripts.LobbySystem.Service {
    public static class RemovePlayerService {
        public static event Action OnExecute;
        public static event Action OnSuccess;
        public static event Action<LobbyExceptionReason> OnFailed;

        public static readonly RateLimited Limited = new(5f, 1);
        
        public static async Task RemovePlayer(string lobbyId, string playerId) {
            try {
                if (!Limited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                Limited.Call();
                
                OnExecute?.Invoke();
                
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);
                OnSuccess?.Invoke();
            }
            catch (LobbyServiceException e) {
                OnFailed?.Invoke(e.Reason);
                Debug.Log(e.Reason);
                throw new LobbyServiceException(e);
            }
            catch (Exception e) {
                OnFailed?.Invoke(LobbyExceptionReason.Unknown);
                Debug.Log(e);
                throw new Exception(LobbyExceptionReason.Unknown.ToString());
            }
        }
    }
}

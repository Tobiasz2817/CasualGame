using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using System;

namespace Game.Scripts.LobbySystem.Service {
    public class UpdateLobbyService  {
        public static event Action OnExecute;
        public static event Action<Lobby> OnSuccess;
        public static event Action<LobbyExceptionReason> OnFailed;

        public static readonly RateLimited Limited = new(5f, 5);
        
        public static async Task UpdateLobby(string lobbyId, UpdateLobbyOptions options = null) {
            try {
                if (!Limited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                Limited.Call();
                
                OnExecute?.Invoke();
                
                var lobby = await LobbyService.Instance.UpdateLobbyAsync(lobbyId, options);
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

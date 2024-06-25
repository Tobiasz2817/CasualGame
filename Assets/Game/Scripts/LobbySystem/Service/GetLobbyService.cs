using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System;

namespace Game.Scripts.LobbySystem.Service {
    public static class GetLobbyService { 
        public static event Action OnExecute;
        public static event Action<Lobby> OnSuccess;
        public static event Action<LobbyExceptionReason> OnFailed;

        public static readonly RateLimited Limited = new(1f, 1);
        
        public static async Task GetLobby(string lobbyId) {
            try {
                if (!Limited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                Limited.Call();
                
                OnExecute?.Invoke();
                
                var lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
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

        public static async Task GetLobby() {
            try {
                await GetLobby(LobbyManager.Instance?.Data.Lobby?.Id);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        } 
    }
}

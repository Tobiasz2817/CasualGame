using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using System;

namespace Game.Scripts.LobbySystem.Service {
    public static class CreateRoomService {
        public static event Action OnExecute;
        public static event Action<Lobby> OnSuccess;
        public static event Action<LobbyExceptionReason> OnFailed;

        public static readonly RateLimited Limited = new(2f, 6);
        
        public static async Task CreateRoom(string lobbyName = "Room", int maxPlayers = 4, CreateLobbyOptions options = null) {
            try {
                if (!Limited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                Limited.Call();
                
                OnExecute?.Invoke();
                
                var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
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
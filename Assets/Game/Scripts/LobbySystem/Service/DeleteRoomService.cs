using System.Threading.Tasks;
using Unity.Services.Lobbies;
using System;

namespace Game.Scripts.LobbySystem.Service {
    public static class DeleteRoomService  {
        public static event Action OnExecute;
        public static event Action OnSuccess;
        public static event Action<LobbyExceptionReason> OnFailed;

        public static readonly RateLimited Limited = new(1f, 2);
        
        public static async Task DeleteRoom(string lobbyId) {
            try {
                if (!Limited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                Limited.Call();
                
                OnExecute?.Invoke();
                
                await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
                OnSuccess?.Invoke();
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

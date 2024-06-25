using System;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace Game.Scripts.LobbySystem.Service {
    public class QueryLobbiesService {
        public static event Action OnExecute;
        public static event Action<QueryResponse> OnSuccess;
        public static event Action<LobbyExceptionReason> OnFailed;

        public static readonly RateLimited Limited = new(1f, 1);
        
        public static async Task QueryLobby(QueryLobbiesOptions options = null) {
            try {
                if (!Limited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                Limited.Call();
                
                OnExecute?.Invoke();
                
                var lobby = await LobbyService.Instance.QueryLobbiesAsync(options);
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
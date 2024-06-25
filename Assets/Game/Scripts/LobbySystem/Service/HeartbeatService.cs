using System.Threading.Tasks;
using Unity.Services.Lobbies;
using System;

namespace Game.Scripts.LobbySystem.Service {
    public class HeartbeatService {
        public static event Action OnExecute;
        public static event Action OnSuccess;
        public static event Action<LobbyExceptionReason> OnFailed;

        public static readonly RateLimited Limited = new(5f, 30);
        
        public static async Task Heartbeat(string lobbyId) {
            try {
                Limited.Call();
                
                if (!Limited.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                Limited.Call();
                
                OnExecute?.Invoke();
                
                await LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
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

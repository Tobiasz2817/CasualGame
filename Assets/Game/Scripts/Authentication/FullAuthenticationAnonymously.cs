using System.Threading.Tasks;
using Game.Scripts.JobSystem;
using Game.Scripts.LobbySystem;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Scripts.Authentication {
    [DefaultExecutionOrder(-888)]
    public class FullAuthenticationAnonymously : MonoBehaviour {
        private void Awake() {
            // Init Unity Service
            JobScheduler.Instance.QueueTask(UnityServices.InitializeAsync).
                WithTask(() => Task.Delay(1000)).
                WithName("Connecting...").
                PushInQueue(LobbyJobId.UnityService);
            
            // Registry Anonymously for testing scene without changing scene all time to test
            JobScheduler.Instance.QueueTask(() => AuthenticationService.Instance.SignInAnonymouslyAsync()).
                WithTask(() => Task.Delay(1000)).
                WithName("Authorizing...").
                PushInQueue(LobbyJobId.Authenticate);
        }
    }
}
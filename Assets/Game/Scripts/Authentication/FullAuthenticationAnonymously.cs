using System.Threading.Tasks;
using Game.Scripts.Loader;
using Game.Scripts.Utils;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Scripts.Authentication {
    [DefaultExecutionOrder(-888)]
    public class FullAuthenticationAnonymously : MonoBehaviour {
        private void Awake() {
            var task = new TaskProcessor(() => TaskExtension.WhenAll(
                UnityServices.InitializeAsync,
                () => Task.Delay(1000),
                () => AuthenticationService.Instance.SignInAnonymouslyAsync()
            ));
            
            task.OnExecute += () => { LoaderListener.Instance.Load("Authorizing..."); };
            task.OnFailedExecute += () => { LoaderListener.Instance.Break(); };
            task.OnSuccessExecute += () => { LoaderListener.Instance.Break(); };
            task.ExecuteTask();
        }
    }
}
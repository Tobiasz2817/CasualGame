using System.Threading.Tasks;
using Game.Scripts.Loader;
using Game.Scripts.Scene;
using Game.Scripts.Utils;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Handler {
    public class UnityRegistryHandler : MonoBehaviour {
        public Button connectButton;
        public int delayBetweenRegistry = 1000;
        private void OnEnable() {
            connectButton.onClick.AddListener(ConnectWithUnityService);
        }

        private void OnDisable() {
            connectButton.onClick.RemoveListener(ConnectWithUnityService);
        }

        private void OnValidate() {
            if (delayBetweenRegistry < 1000) 
                delayBetweenRegistry = 1000;
        }

        private void ConnectWithUnityService() {
            var task = new TaskProcessor(() => TaskExtension.WhenAll(
                () => PlayerAccountService.Instance.StartSignInAsync(),
                () => Task.Delay(delayBetweenRegistry),
                () => AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken)
            ));

            task.OnExecute += () => { LoaderListener.Instance.Load("Login in website"); };
            task.OnFailedExecute += () => { LoaderListener.Instance.Break(); };
            task.OnSuccessExecute += () => { SceneLoader.Instance.Load(SceneType.MainMenu); };
            
            task.ExecuteTask();
        }
    }
}
using System.Threading.Tasks;
using Game.Scripts.Loader;
using Game.Scripts.Scene;
using Game.Scripts.Utils;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Authentication {
    public class AnonymouslyWindow : MonoBehaviour {
        public Button connectButton;

        private void OnEnable() {
            connectButton.onClick.AddListener(ConnectAnonymously);
        }

        private void OnDisable() {
            connectButton.onClick.RemoveListener(ConnectAnonymously);
        }

        private void ConnectAnonymously() {
            var task = new TaskProcessor(() => TaskExtension.WhenAll(
                () => AuthenticationService.Instance.SignInAnonymouslyAsync(),
                () => Task.Delay(1000)
                ));
            task.OnExecute += () => { LoaderListener.Instance.Load("Authorizing..."); };
            task.OnFailedExecute += () => { LoaderListener.Instance.Break(); };
            task.OnSuccessExecute += () => { SceneLoader.Instance.Load(SceneType.MainMenu); };
            task.ExecuteTask();
        }
    }
}
using Game.Scripts.Authentication;
using Game.Scripts.Scene;
using Game.Scripts.Utils;
using Unity.Services.Authentication;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.Windows {
    public class AnonymouslyWindow : MonoBehaviour {
        public Button connectButton;

        private void OnEnable() {
            connectButton.onClick.AddListener(ConnectAnonymously);
        }

        private void OnDisable() {
            connectButton.onClick.RemoveListener(ConnectAnonymously);
        }

        private void ConnectAnonymously() {
            var task = new TaskProcessor(() => AuthenticationService.Instance.SignInAnonymouslyAsync());
            task.OnExecute += () => { LoaderListener.Instance.Load("Authorizing..."); };
            task.OnFailedExecute += () => { LoaderListener.Instance.Break(); };
            task.OnSuccessExecute += () => { SceneLoader.Instance.Load(SceneType.MainMenu); };
            task.ExecuteTask();
        }
    }
}
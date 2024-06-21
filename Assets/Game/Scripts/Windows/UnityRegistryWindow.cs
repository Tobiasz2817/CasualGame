using Game.Scripts.Authentication;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.Windows {
    public class UnityRegistryWindow : MonoBehaviour {
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
            var connection = new AuthOperation {
                Action = new UnityAuthenticator(delayBetweenRegistry),
                OperationMessage = "Login in website"
            };
            
            Authenticate.Instance.SignInClient(connection);
            LoaderListener.Instance.Load(Authenticate.Instance.Operation.OperationMessage);
        }
    }
}
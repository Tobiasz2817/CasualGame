using Game.Scripts.Authentication;
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
            var connectionAction = new AuthOperation { 
                Action = new AnonymouslyAuthenticator(),
                OperationMessage = "Authenticating"
            };
            
            Authenticate.Instance.SignInClient(connectionAction);
        }
    }
}
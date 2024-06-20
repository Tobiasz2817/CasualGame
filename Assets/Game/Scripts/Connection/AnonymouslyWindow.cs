using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.Connection {
    public class AnonymouslyWindow : MonoBehaviour {
        public Button connectButton;

        private void OnEnable() {
            connectButton.onClick.AddListener(ConnectAnonymously);
        }

        private void OnDisable() {
            connectButton.onClick.RemoveListener(ConnectAnonymously);
        }

        private void ConnectAnonymously() {
            var connectionAction = new AuthenticateAction { 
                Action = () =>  AuthenticationService.Instance.SignInAnonymouslyAsync()
            };
            
            Authenticate.Instance.AuthenticateClient(connectionAction);
        }
    }
}
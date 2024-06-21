using Game.Scripts.Authentication;
using Game.Scripts.Scene;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.Windows {
    public class LogOffWindow : MonoBehaviour {
        public SceneType backWindow;
        public Button button;

        private void OnEnable() {
            button.onClick.AddListener(LogOff);
        }

        private void OnDisable() {
            button.onClick.RemoveListener(LogOff);
        }

        private void LogOff() {
            SceneLoader.Instance.Next = backWindow;
            
            var authOperation = new AuthOperation {
                Action =  new LogOffOperation(),
                OperationMessage = "Logging off..."
            };
            
            Authenticate.Instance.SignOutClient(authOperation);   
            LoaderListener.Instance.Load(Authenticate.Instance.Operation.OperationMessage);
        }
    }
}
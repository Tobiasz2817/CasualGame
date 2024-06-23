using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Handler {
    //TODO: Other auth solution
    public class UnityDataRegistryHandler : MonoBehaviour {
         public Button connectButton;
         private void OnEnable() {
             connectButton.onClick.AddListener(ConnectWithUnityService);
         }

         private void OnDisable() {
             connectButton.onClick.RemoveListener(ConnectWithUnityService);
         }

         private void ConnectWithUnityService() {
             
         }
     }
}
using Game.Scripts.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Windows {
    //TODO: Other auth solution
    public class UnityDataRegistryWindow : MonoBehaviour {
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
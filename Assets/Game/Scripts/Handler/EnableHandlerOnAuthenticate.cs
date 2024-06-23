using System.Threading.Tasks;
using Game.Scripts.Utils;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Scripts.Handler {
    public class EnableHandlerOnAuthenticate : MonoBehaviour {
        private async void OnEnable() {
            await TaskExtension.While(() => UnityServices.State != ServicesInitializationState.Initialized);
            await Task.Delay(2000);
            AuthenticationService.Instance.SignedIn += EnableHandlers;
        }
        
        private void OnDisable() {
            AuthenticationService.Instance.SignedIn -= EnableHandlers;
        }

        private void EnableHandlers() {
            for (var i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i);
                child.gameObject.SetActive(true);
            }
        }
    }
}
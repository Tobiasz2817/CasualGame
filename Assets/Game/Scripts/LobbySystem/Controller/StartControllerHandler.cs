using Game.Scripts.LobbySystem.Service;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;
using UnityEngine;

namespace Game.Scripts.LobbySystem.Controller {
    public class StartControllerHandler : MonoBehaviour {

        [SerializeField] private Button _startButton;
        private void OnEnable() {
            _startButton.onClick.AddListener(ProcessStart);
            GetLobbyService.OnSuccess += ChangeVisibly;
        }
        private void OnDisable() {
            _startButton.onClick.RemoveListener(ProcessStart);
            GetLobbyService.OnSuccess -= ChangeVisibly;
        }
        
        private void ChangeVisibly(Lobby obj) {
            if (LobbyManager.Instance == null) return;
            
            var isHost = LobbyManager.Instance.IsLobbyHost(obj.HostId);
            _startButton.gameObject.SetActive(isHost);
        }

        
        private void ProcessStart() {
            //TODO:: 
        }
    }
}
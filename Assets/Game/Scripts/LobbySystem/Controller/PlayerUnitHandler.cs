using System;
using System.Threading.Tasks;
using Game.Scripts.JobSystem;
using Game.Scripts.LobbySystem.Service;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.LobbySystem.Controller {
    public class PlayerUnitHandler : MonoBehaviour {
        [SerializeField] private Image avatarImage;
        [SerializeField] private Sprite avatarSprite;
        [SerializeField] private Button removeButton;

        private string _playerId;
        public string PlayerId => _playerId;
        
        // We can expend about players nickname and others

        private void OnEnable() {
            removeButton.onClick.AddListener(RemovePlayerFromLobby);
        }

        private void OnDisable() {
            removeButton.onClick.AddListener(RemovePlayerFromLobby);
        }
        
        public void AddPlayer() {
            avatarImage.sprite = avatarSprite;
        }


        public void RemovePlayer() {
            avatarImage.sprite = null;
            _playerId = string.Empty;
        }
        
        public void SetId(string playerId) {
            this._playerId = playerId;
        }
        
        public void EnableRemoveButton() => removeButton.gameObject.SetActive(true);
        public void DisableRemoveButton() => removeButton.gameObject.SetActive(false);
        
        
        
        private void RemovePlayerFromLobby() {
            if (LobbyManager.Instance == null || !LobbyManager.Instance.IsLobbyExist()) return;
            if (string.IsNullOrEmpty(_playerId)) return;

            var lobbyId = LobbyManager.Instance.Data.Lobby?.Id;
            if (LobbyManager.Instance.IsOwner(_playerId)) {
                JobScheduler.Instance.QueueTask(() => RemovePlayerService.RemovePlayer(lobbyId, _playerId)).
                    WithTask(() => Task.Delay(1000)).
                    WithName("Leaving...").
                    WithCondition(() => RemovePlayerService.Limited.CanInvoke()).
                    PushInQueue(LobbyJobId.RemovePlayer);
            }
            else if (LobbyManager.Instance.IsLobbyHost() ) {
                JobScheduler.Instance.QueueTask(() => RemovePlayerService.RemovePlayer(lobbyId, _playerId)). 
                    WithTask(() => Task.Delay(1000)).
                    WithName("Kicking...").
                    WithCondition(() => RemovePlayerService.Limited.CanInvoke()).
                    PushInQueue(LobbyJobId.RemovePlayer); 
            }
            
        }
    }
}
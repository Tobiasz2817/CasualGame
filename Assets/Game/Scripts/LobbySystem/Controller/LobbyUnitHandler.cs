using System.Threading.Tasks;
using Game.Scripts.JobSystem;
using Game.Scripts.LobbySystem.Service;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Game.Scripts.LobbySystem.Controller {
    public class LobbyUnitHandler : MonoBehaviour {
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private TMP_Text _playersCountText;
        [SerializeField] private Button _joinButton;

        private Lobby _lobby;
        
        private void OnEnable() {
            _joinButton.onClick.AddListener(JoinToLobby);
            JoinRoomService.OnFailed += (xd) => Debug.Log("XD " + xd);
        }
        
        private void OnDisable() {
            _joinButton.onClick.RemoveListener(JoinToLobby);
        }

        public void UpdateLobbyUnit(Lobby lobby) {
            _lobby = lobby;

            _roomNameText.text = lobby?.Name + " " + lobby?.Id;
            _playersCountText.text = lobby?.Players.Count + " / " + lobby?.MaxPlayers;
        }
        
        private void JoinToLobby() {
            if (_lobby == null) return;

            var lastLobbyId = LobbyManager.Instance.Data.Lobby?.LobbyCode;
            
            JobScheduler.Instance.QueueTask(() => JoinRoomService.JoinRoom(_lobby?.Id, (JoinLobbyByIdOptions)null)).
                WithTask(() =>Task.Delay(1000)).
                WithName("Joining room...").
                WithCondition(JoinRoomService.Limited.CanInvoke).
                PushInQueue(LobbyJobId.JoinRoom);
            
            JobScheduler.Instance.QueueTask(() => DeleteRoomService.DeleteRoom(lastLobbyId)).
                WithTask(() => Task.Delay(1000)).
                WithCondition(() => DeleteRoomService.Limited.CanInvoke()).
                PushInQueue(LobbyJobId.DeleteRoom);
        }
    }
}
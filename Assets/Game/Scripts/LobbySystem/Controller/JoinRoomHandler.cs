using System;
using System.Collections;
using System.Threading.Tasks;
using Game.Scripts.JobSystem;
using Game.Scripts.LobbySystem.Service;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.LobbySystem.Controller {
    public class JoinRoomHandler : MonoBehaviour {
        // Code Join
        [SerializeField] private TMP_InputField _joinCode;
        [SerializeField] private TMP_Text _joinCodeText;
        [SerializeField] private Button _joinCodeButton;
        [SerializeField] private int codeLength = 6;

        private bool _isResetCode;
        private Color _defaultTextColor;
        private Color _errorTextColor = Color.red;

        private void Start() {
            _defaultTextColor = _joinCodeText.color;
        }

        private void OnEnable() {
            _joinCodeButton.onClick.AddListener(EnterWithCode);
            CreateRoomService.OnSuccess += UpdateCode;
            GetLobbyService.OnSuccess += UpdateCode;
            JoinRoomService.OnSuccess += UpdateCode;
        }

        private void OnDisable() {
            _joinCodeButton.onClick.RemoveListener(EnterWithCode);
            CreateRoomService.OnSuccess -= UpdateCode;            
            GetLobbyService.OnSuccess -= UpdateCode;
            JoinRoomService.OnSuccess -= UpdateCode;
        }

        private void UpdateCode(Lobby lobby) {
            if (_isResetCode) return;
            _joinCodeText.text = LobbyManager.Instance.IsLobbyHost(lobby.HostId) ? lobby.LobbyCode : string.Empty;
        }

        public void EnterWithCode() {
            var code = _joinCode.text;
            if (code.Length != codeLength || LobbyManager.Instance.IsInLobbyWithCode(code)) {
                if (!_isResetCode) {
                    _isResetCode = true;
                    StopCoroutine(ResetCodeText());
                    StartCoroutine(ResetCodeText());
                    
                    _joinCodeText.text = "Bad Code";
                    _joinCodeText.color = _errorTextColor;
                }
                
                return;
            }
            
            _joinCodeText.color = _defaultTextColor;
            
            
            JobScheduler.Instance.QueueTask(JoinRoomWithCode).
                WithTask(Wait).
                WithName("Joining room...").
                WithCondition(JoinRoomRateLimit).
                WithCallback(
                    onSuccess: () => { _joinCodeText.text = string.Empty; }).
                PushInQueue(LobbyJobId.JoinRoom);
            
            JobScheduler.Instance.QueueTask(() => DeleteRoomService.DeleteRoom(LobbyManager.Instance.Data.Lobby?.LobbyCode)).
                WithTask(() => Task.Delay(1000)).
                WithCondition(() => DeleteRoomService.Limited.CanInvoke()).
                PushInQueue(LobbyJobId.DeleteRoom);
        }

        public IEnumerator ResetCodeText() {
            yield return new WaitForSeconds(2f);
            _joinCodeText.text = LobbyCode(LobbyManager.Instance.Data.Lobby);
            _joinCodeText.color = _defaultTextColor;
            _isResetCode = false;
        }
        
        public string LobbyCode(Lobby lobby) => lobby != null && LobbyManager.Instance.IsLobbyHost(lobby.HostId) ? lobby.LobbyCode : string.Empty;
        
        // Join
        private Task JoinRoomWithCode() => JoinRoomService.JoinRoom(_joinCode.text, (JoinLobbyByCodeOptions)null);
        private bool JoinRoomRateLimit() => JoinRoomService.Limited.CanInvoke();
        
        
        
        private bool JoinRoomQuickRateLimit() => JoinRoomService.QuickLimited.CanInvoke();
        private Task Wait() => Task.Delay(1000);
    }
}

//Delete Room
/*/*JobScheduler.Instance.QueueTask(DeleteRoom).
WithTask(Wait).
WithName("Leaving room...").
WithCondition(DeleteRoomRateLimit).
WithCallback(onSuccess: () => { _joinCodeText.text = string.Empty; }).
PushInQueue(LobbyJobId.DeleteRoom);* 
// Leave
private Task DeleteRoom() => DeleteRoomService.DeleteRoom(LobbyManager.Instance.Data.Lobby.Id);
private bool DeleteRoomRateLimit() => DeleteRoomService.Limited.CanInvoke();
/*/
using System;
using System.Threading.Tasks;
using Game.Scripts.Utils;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Game.Scripts.LobbySystem {
    [DefaultExecutionOrder(-998)]
    public class LobbyManagerArchive : SingletonPersistent<LobbyManagerArchive> {
        
        public static event Action OnCreateRoomExecute;
        public event Action<Lobby> OnCreateRoomSuccess;
        public event Action<LobbyExceptionReason> OnCreateRoomFailed;
        private RateLimited _createRoomRL;
        
        public event Action OnDeleteRoomExecute;
        public event Action<Lobby> OnDeleteRoomSuccess;
        public event Action<LobbyExceptionReason> OnDeleteRoomFailed;
        private RateLimited _deleteRoomRL;
        
        public event Action OnJoinRoomExecute;
        public event Action<Lobby> OnJoinRoomSuccess;
        public event Action<LobbyExceptionReason> OnJoinRoomFailed;
        private RateLimited _joinRoomRL;
        private RateLimited _quickJoinRoomRL;
        
        public event Action OnHeartbeatExecute;
        public event Action OnHeartbeatSuccess;
        public event Action<LobbyExceptionReason> OnHeartbeatFailed;
        private RateLimited _heartbeatRL;
        
        public event Action OnGetLobbyExecute;
        public event Action<Lobby> OnGetLobbySuccess;
        public event Action<LobbyExceptionReason> OnGetLobbyFailed;
        private RateLimited _getLobbyRL;
        
        public event Action OnRemovePlayerExecute;
        public event Action<string> OnRemoveSelfSuccess;
        public event Action<string> OnRemovePlayerSuccess;
        public event Action<LobbyExceptionReason> OnRemovePlayerFailed;
        private RateLimited _removePlayerRL;
        
        public event Action OnUpdateLobbyExecute;
        public event Action<Lobby> OnUpdateLobbySuccess;
        public event Action<LobbyExceptionReason> OnUpdateLobbyFailed;
        private RateLimited _updateLobbyRL;
        
        public event Action OnUpdatePlayerExecute;
        public event Action<Lobby> OnUpdatePlayerSuccess;
        public event Action<LobbyExceptionReason> OnUpdatePlayerFailed;
        private RateLimited _updatePlayerRL;
        
        private Lobby _currentLobby;
        private Lobby Lobby => _currentLobby;

        public bool IsProcessing { private set; get; }

        public override void Awake() {
            base.Awake();
            RateLimitInit();
        }

        private void RateLimitInit() {
            _createRoomRL = new RateLimited( 6f, 2);
            _deleteRoomRL = new RateLimited( 1f, 2);
            _joinRoomRL = new RateLimited(6f, 2);
            _quickJoinRoomRL = new RateLimited( 10f, 1);
            _heartbeatRL = new RateLimited(30f, 5);
            _getLobbyRL = new RateLimited( 1f, 1);
            _removePlayerRL = new RateLimited( 5f, 1);
            _updateLobbyRL = new RateLimited( 5f, 5);
            _updatePlayerRL = new RateLimited(5f, 5);
        }

        #region Create Room

        public async Task CreateRoom(string roomName, int maxPlayers, CreateLobbyOptions options = null) {
            try {
                if (!_createRoomRL.CanInvoke()) 
                    throw new Exception("Over Rate limited");
            
                _createRoomRL.Call();
                
                OnCreateRoomExecute?.Invoke();
                
                var lobby = await LobbyService.Instance.CreateLobbyAsync(roomName, maxPlayers, options);
                OnCreateRoomSuccess?.Invoke(lobby);
                _currentLobby = lobby;
            }
            catch (LobbyServiceException e) {
                OnCreateRoomFailed?.Invoke(e.Reason);
            }
        }

        #endregion
        
        #region Join Room

        public async Task JoinRoom(string enterCode, JoinLobbyByCodeOptions options = null) {
            try {
                if (!_joinRoomRL.CanInvoke()) 
                    throw new Exception("Over Rate limited");

                _joinRoomRL.Call();
                
                OnJoinRoomExecute?.Invoke();
                
                var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(enterCode, options);
                OnJoinRoomSuccess?.Invoke(lobby);
                _currentLobby = lobby;
            }
            catch (LobbyServiceException e) {
                OnJoinRoomFailed?.Invoke(e.Reason);
            }
        }
        
        public async void JoinRoom(string  lobbyId, JoinLobbyByIdOptions options = null) {
            _joinRoomRL.Call();

            if (!_joinRoomRL.CanInvoke()) return;
                
            OnJoinRoomExecute?.Invoke();
            
            try {
                var lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options);
                OnJoinRoomSuccess?.Invoke(lobby);
                _currentLobby = lobby;
            }
            catch (LobbyServiceException e) {
                OnJoinRoomFailed?.Invoke(e.Reason);
            }
        }
        
        public async void QuickJoinRoom(QuickJoinLobbyOptions options = null) {
            _quickJoinRoomRL.Call();

            if (!_quickJoinRoomRL.CanInvoke()) return;
                
            OnJoinRoomExecute?.Invoke();
            
            try {
                var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
                OnJoinRoomSuccess?.Invoke(lobby);
                _currentLobby = lobby;
            }
            catch (LobbyServiceException e) {
                OnJoinRoomFailed?.Invoke(e.Reason);
            }
        }

        #endregion
        
        #region DeleteRoom
        
        public async void DeleteRoom(string lobbyId) {
            _deleteRoomRL.Call();

            if (!_deleteRoomRL.CanInvoke()) return;
            
            OnDeleteRoomExecute?.Invoke();
            
            try {
                await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
                OnDeleteRoomSuccess?.Invoke(Lobby);
                _currentLobby = null;
            }
            catch (LobbyServiceException e) {
                OnDeleteRoomFailed?.Invoke(e.Reason);
            }
        }

        public void DeleteRoom() => DeleteRoom(Lobby?.Id);

        #endregion

       

        #region Heartbeat

        public async void Heartbeat() {
            _heartbeatRL.Call();

            if (!_heartbeatRL.CanInvoke() || !LobbyExist()) return;
                
            OnHeartbeatExecute?.Invoke();
            
            try {
                await LobbyService.Instance.SendHeartbeatPingAsync(Lobby?.Id);
                OnHeartbeatSuccess?.Invoke();
            }
            catch (LobbyServiceException e) {
                OnHeartbeatFailed?.Invoke(e.Reason);
            }
        }

        #endregion

        #region GetLobby

        public async void GetLobby(string lobbyId) {
            _getLobbyRL.Call();

            if (!_getLobbyRL.CanInvoke()) return;
                
            OnGetLobbyExecute?.Invoke();
            
            try {
                var lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
                OnGetLobbySuccess?.Invoke(lobby);
            }
            catch (LobbyServiceException e) {
                OnGetLobbyFailed?.Invoke(e.Reason);
            }
        }

        public void GetLobby() => GetLobby(Lobby?.Id);

        #endregion

        #region Remove Player
        
        public async void RemovePlayer(string lobbyId, string playerId) {
            _removePlayerRL.Call();

            if (!_removePlayerRL.CanInvoke()) return;
                
            OnRemovePlayerExecute?.Invoke();
            
            try {
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, playerId);

                if (playerId == AuthenticationService.Instance.PlayerId) {
                    OnRemoveSelfSuccess?.Invoke(playerId);
                }
                else {
                    OnRemovePlayerSuccess?.Invoke(playerId);
                }
            }
            catch (LobbyServiceException e) {
                OnRemovePlayerFailed?.Invoke(e.Reason);
            }
        }

        #endregion

        #region Update Lobby

        
        public async void UpdateLobby(string lobbyId, UpdateLobbyOptions options = null) {
            _updateLobbyRL.Call();

            if (!_updateLobbyRL.CanInvoke()) return;
                
            OnUpdateLobbyExecute?.Invoke();

            try {
                var lobby = await LobbyService.Instance.UpdateLobbyAsync(lobbyId, options);
                OnUpdateLobbySuccess?.Invoke(lobby);
            }
            catch (LobbyServiceException e) {
                OnUpdateLobbyFailed?.Invoke(e.Reason);
            }
        }
        
        public void UpdateLobby( UpdateLobbyOptions options = null) {
            if (!LobbyExist()) return;
            
            UpdateLobby(Lobby.Id, options);
        }
        

        #endregion

        #region Update Player

        
        public async void UpdatePlayer(string lobbyId, string playerId, UpdatePlayerOptions options = null) {
            _updatePlayerRL.Call();

            if (!_updatePlayerRL.CanInvoke()) return;
                
            OnUpdatePlayerExecute?.Invoke();
            
            try {
                var lobby = await LobbyService.Instance.UpdatePlayerAsync(lobbyId, playerId, options);
                OnUpdatePlayerSuccess?.Invoke(lobby);
            }
            catch (LobbyServiceException e) {
                OnUpdatePlayerFailed?.Invoke(e.Reason);
            }
        }
        
        #endregion

        public bool LobbyExist() => Lobby != null;
    }
}
using Game.Scripts.LobbySystem.Service;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Game.Scripts.LobbySystem.Controller {
    public class PlayerInterfaceHandler : MonoBehaviour {
        [SerializeField] private PlayerUnitHandler[] _playersInterface;
        
        private void OnEnable() {
            CreateRoomService.OnSuccess += UpdatePlayers;
            JoinRoomService.OnSuccess += UpdatePlayers;
            RemovePlayerService.OnSuccess += ResetPlayers;
            UpdateLobbyService.OnSuccess += UpdatePlayers;
            GetLobbyService.OnSuccess += UpdatePlayers;
            KickHandler.OnSuccess += ResetPlayers;
        }
        
        private void OnDisable() {
            CreateRoomService.OnSuccess -= UpdatePlayers;
            JoinRoomService.OnSuccess -= UpdatePlayers;
            RemovePlayerService.OnSuccess += ResetPlayers;
            UpdateLobbyService.OnSuccess -= UpdatePlayers;
            GetLobbyService.OnSuccess -= UpdatePlayers;
            KickHandler.OnSuccess -= ResetPlayers;
        }
        
        private void ResetPlayers() {
            foreach (var player in _playersInterface) {
                player.RemovePlayer();
                player.DisableRemoveButton();
            }
        }

        private void UpdatePlayers(Lobby lobby) {
            if (lobby == null) {
                foreach (var player in _playersInterface) {
                    player.RemovePlayer();
                }

                return;
            }
            
            for (var i = 0; i < _playersInterface.Length; i++) {
                bool isOutOfRange = (lobby.Players.Count - 1) - i < 0;
                
                if (isOutOfRange) {
                    _playersInterface[i].RemovePlayer();
                    _playersInterface[i].DisableRemoveButton();
                    continue;
                }
                
                var isHost = LobbyManager.Instance.IsLobbyHost(lobby.HostId);
                var isOwner = LobbyManager.Instance.IsOwner(lobby.Players[i].Id);
                
                // Remove button
                if (isHost) {
                    if(isOwner && lobby.Players.Count <= 1)
                        _playersInterface[i].DisableRemoveButton();
                    else 
                        _playersInterface[i].EnableRemoveButton();
                }
                
                if(!isHost && isOwner)
                    _playersInterface[i].EnableRemoveButton();
                else if(!isHost)
                    _playersInterface[i].DisableRemoveButton();
                
                _playersInterface[i].SetId(lobby.Players[i].Id);
                _playersInterface[i].AddPlayer();
            }
        }
    }
}
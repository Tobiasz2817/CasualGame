using System;
using Game.Scripts.Lobby;
using UnityEngine;

namespace Game.Scripts.PlayerUI {
    using Unity.Services.Lobbies.Models;
    public class InterfacePlayers : MonoBehaviour {
        [SerializeField] private InterfacePlayerUnit[] _playersInterface;
        
        private void OnEnable() {
            LobbyManager.Instance.OnCreateRoomSuccess += UpdatePlayers;
            LobbyManager.Instance.OnUpdateLobbySuccess += UpdatePlayers;
        }
        
        private void OnDisable() {
            LobbyManager.Instance.OnCreateRoomSuccess -= UpdatePlayers;
            LobbyManager.Instance.OnUpdateLobbySuccess -= UpdatePlayers;
        }

        private void UpdatePlayers(Lobby obj) {
            _playersInterface[0].AddPlayer();
        }
    }
}
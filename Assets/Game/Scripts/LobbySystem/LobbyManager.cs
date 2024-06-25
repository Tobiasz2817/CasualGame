using System;
using Unity.Services.Authentication;
using Game.Scripts.Utils;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Game.Scripts.LobbySystem {
    [DefaultExecutionOrder(-999)]
    public class LobbyManager : Singleton<LobbyManager> {
        public LobbyData Data { protected set; get; } = new LobbyData();


        private void Update() {
            //Debug.Log("Players: " + Data?.Lobby?.Players.Count);
        }

        public bool IsInLobby(Lobby lobby) {
            if (lobby != null && lobby.Players != null)
                foreach (Player player in lobby.Players)
                    if (player.Id == AuthenticationService.Instance.PlayerId)
                        return true;

            return false;
        }
        public bool IsOwner(string playerId) => playerId == AuthenticationService.Instance.PlayerId;
        public bool IsInLobbyWithCode(string code) => IsLobbyExist() && Data.Lobby.LobbyCode == code;
        public bool IsLobbyExist() => Data.Lobby != null;
        public bool IsLobbyHost() => IsLobbyExist() && IsLobbyHost(Data.Lobby.HostId);
        public bool IsPlayersMoreThan(int count) => IsLobbyExist() && Data.Lobby.Players.Count > count;
        public bool IsLobbyHost(string hostId) => AuthenticationService.Instance != null && hostId == AuthenticationService.Instance.PlayerId;
    }
}
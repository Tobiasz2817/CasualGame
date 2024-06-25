using System;
using Unity.Services.Lobbies.Models;

namespace Game.Scripts.LobbySystem {
    public class LobbyData {
        public Lobby Lobby;
    }

    public static class RateLimitConfig {
        public static RateLimited CreateRoomRL = new RateLimited( 6f, 2);
        public static RateLimited DeleteRoomRL = new RateLimited( 1f, 2);
        public static RateLimited JoinRoomRL = new RateLimited(6f, 2);
        public static RateLimited QuickJoinRoomRL = new RateLimited( 10f, 1);
        public static RateLimited HeartbeatRL = new RateLimited(30f, 5);
        public static RateLimited GetLobbyRL = new RateLimited( 1f, 1);
        public static RateLimited RemovePlayerRL = new RateLimited( 5f, 1);
        public static RateLimited UpdateLobbyRL = new RateLimited( 5f, 5);
        public static RateLimited UpdatePlayerRL = new RateLimited(5f, 5);
    }

    public static class LobbyJobId {
        public static Guid UnityService = Guid.NewGuid();
        public static Guid Authenticate = Guid.NewGuid();
        public static Guid CreateRoom = Guid.NewGuid();
        public static Guid JoinRoom = Guid.NewGuid();
        public static Guid HeartBeat = Guid.NewGuid();
        public static Guid DeleteRoom = Guid.NewGuid();
        public static Guid UpdateLobby = Guid.NewGuid();
        public static Guid RemovePlayer = Guid.NewGuid();
        public static Guid GetLobby = Guid.NewGuid();
        public static Guid QueryLobby = Guid.NewGuid();
    }
}
using Unity.NetCode;

namespace Game.Scripts.Netcode {
    [UnityEngine.Scripting.Preserve]
    public class AutoBootstrap : ClientServerBootstrap
    {
        public override bool Initialize(string defaultWorldName) {
            //AutoConnectPort = 7979;
            return base.Initialize(defaultWorldName);
        }
    }
}

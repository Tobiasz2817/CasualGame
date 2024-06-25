using Unity.Entities;
using Unity.Mathematics;

namespace Game.Scripts {

    public struct InputIdentify : IComponentData {
    }
    
    public struct MoveInput : IComponentData, IEnableableComponent {
        public float3 Direction;
    }
    public struct Testes : IComponentData, IEnableableComponent {
        
    }
}
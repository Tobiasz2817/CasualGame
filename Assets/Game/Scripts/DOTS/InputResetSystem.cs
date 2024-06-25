using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.Scripts {
    
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct InputResetSystem : ISystem {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (tag, entity) in SystemAPI.Query<RefRO<InputIdentify>>().WithEntityAccess()) {
                // TODO: 
                // ecb.SetComponentEnabled<JumpInput>(entity, false);
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
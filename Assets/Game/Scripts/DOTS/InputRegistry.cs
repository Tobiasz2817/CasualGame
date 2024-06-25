using Unity.Entities;
using UnityEngine;

namespace Game.Scripts {
    
    [UpdateInGroup(typeof(SimulationSystemGroup),OrderFirst = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public class InputRegistry : MonoBehaviour {
        
        private class InputRegistryBaker : Baker<InputRegistry> {
            public override void Bake(InputRegistry authoring) {
                
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new InputIdentify());
                AddComponent(entity, new MoveInput());
                SetComponentEnabled<MoveInput>(entity, false);
            }
        }
    }
}
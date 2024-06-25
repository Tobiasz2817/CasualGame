using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Game.Scripts {
    
    public partial struct MoveSystem : ISystem {
        
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<InputIdentify>();
            state.RequireForUpdate<MoveInput>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var input in SystemAPI.Query<RefRO<MoveInput>>()) {
                var moveTask = new MoveTask {
                    DeltaTime = SystemAPI.Time.DeltaTime,
                    Direction = input.ValueRO.Direction
                };

                moveTask.Schedule();
            }
        }
    }
    
    [BurstCompile]
    public partial struct MoveTask : IJobEntity {
        public float DeltaTime;
        public float3 Direction;
        
        public void Execute(ref LocalTransform transform, in MoveSpeed speed) {
            Direction = new float3(math.round(Direction.x), 0f, math.round(Direction.z));
            transform = transform.Translate(DeltaTime * speed.speed * Direction);
        }
    }
}
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Scripts {
    public partial struct RotateSystem : ISystem {
        
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<InputIdentify>();
            state.RequireForUpdate<MoveInput>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            foreach (var input in SystemAPI.Query<RefRO<MoveInput>>()) {
                var rotateTask = new RotateTask {
                    DeltaTime = SystemAPI.Time.DeltaTime,
                    Direction = input.ValueRO.Direction
                };

                rotateTask.Schedule();
            }
        }
    }
    
    [BurstCompile]
    public partial struct RotateTask : IJobEntity {
        public float DeltaTime;
        public float3 Direction;
        
        public void Execute(ref LocalTransform transform, in RotateData rotateData) {
            if (!(math.length(Direction) > 0)) return; 
            
            float angle = math.atan2(Direction.x, Direction.z) * math.TODEGREES;
            quaternion targetRotation = quaternion.EulerXYZ(0, math.radians(angle), 0);
            transform.Rotation = math.slerp(transform.Rotation, targetRotation, DeltaTime * rotateData.speed);
        }
    }
}
using Unity.Entities;
using UnityEngine;

namespace Game.Scripts {
    public class MoveAuthoring : MonoBehaviour {
        public float moveSpeed;
    }

    class MoveBaker : Baker<MoveAuthoring> {
        public override void Bake(MoveAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MoveSpeed {
                speed = authoring.moveSpeed
            });
        }
    }

    public struct MoveSpeed : IComponentData {
        public float speed;
    }
    

}
using Unity.Entities;
using UnityEngine;

namespace Game.Scripts {
    public class RotateAuthoring : MonoBehaviour {
        public float speed;
    }

    class RotateAuthoringBaker : Baker<RotateAuthoring> {
        public override void Bake(RotateAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotateData {
                speed = authoring.speed,
            });
        }
    }


    public struct RotateData : IComponentData {
        public float speed;
    }
}
using Components;
using Components.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial class EnemyMovementSystem : SystemBase
    {
        private Entity _prefab;
        
        protected override void OnUpdate()
        {
            if (_prefab.Equals(Entity.Null))
            {
                _prefab = GetSingleton<PrefabPlayer>().Reference;
                return;
            }
            
            var deltaTime = Time.DeltaTime;
            var playerEntity = _prefab;
            var playerPos = EntityManager.GetComponentData<LocalToWorld>(playerEntity);

            Entities
                .WithAll<EnemyTag>()
                .ForEach((ref Translation translation, ref Velocity velocity, in LocalToWorld localToWorld) =>
                {
                    //Move enemy towards player
                    velocity.Direction = Vector3.Normalize(playerPos.Position - localToWorld.Position); 
                    translation.Value += new float3(
                        velocity.Direction.x,
                        velocity.Direction.y,
                        velocity.Direction.z) * velocity.Speed * deltaTime;
                    
                }).ScheduleParallel();
        }
    }
}

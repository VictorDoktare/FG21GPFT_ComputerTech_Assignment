using Components;
using Components.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace Systems
{
    public partial class ProjectileMovementSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _beginSimECB;

        protected override void OnCreate()
        {
            _beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var commandBuffer = _beginSimECB.CreateCommandBuffer().AsParallelWriter();
            var deltaTime = Time.DeltaTime;

            Entities
                .WithAll<ProjectileTag>()
                .ForEach((
                    Entity entity,
                    int entityInQueryIndex,
                    ref Translation translation,
                    ref Velocity velocity,
                    ref Lifetime lifeTime,
                    in LocalToWorld localToWorld) => {
                    
                    //Move projectile in forward facing direction
                    velocity.Direction = new Vector3(
                        localToWorld.Up.x,
                        localToWorld.Up.y,
                        localToWorld.Up.z).normalized;
                    
                    translation.Value.xyz += new float3(
                        velocity.Direction.x,
                        velocity.Direction.y,
                        velocity.Direction.z) * velocity.Speed * deltaTime;

                    lifeTime.age += deltaTime;
                    if (lifeTime.age > lifeTime.maxAge)
                    {
                        commandBuffer.DestroyEntity(entityInQueryIndex, entity);
                    }

                }).ScheduleParallel();
            _beginSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}

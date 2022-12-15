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
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            
            Entities
                .WithAll<ProjectileTag>()
                .ForEach((
                    ref Translation translation,
                    ref Velocity velocity,
                    in LocalToWorld localToWorld) =>
                {
                    //Move projectile in forward facing direction
                    velocity.Direction = new Vector3(
                        localToWorld.Up.x,
                        localToWorld.Up.y,
                        localToWorld.Up.z).normalized;
                    
                    translation.Value.xyz += new float3(
                        velocity.Direction.x,
                        velocity.Direction.y,
                        velocity.Direction.z) * velocity.Speed * deltaTime;
                    
                }).ScheduleParallel();
        }
    }
}

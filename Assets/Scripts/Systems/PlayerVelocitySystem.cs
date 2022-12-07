using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public partial class PlayerVelocitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, in Velocity velocity) =>
            {
                translation.Value.x += (velocity.Direction.x * velocity.Speed * deltaTime);
            }).ScheduleParallel();
    }
}

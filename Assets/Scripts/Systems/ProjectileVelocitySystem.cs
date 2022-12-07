using Unity.Entities;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public partial class ProjectileVelocitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities
            .WithAll<ProjectileTag>()
            .ForEach((ref Translation translation, ref LocalToWorld localToWorld, ref Velocity velocity) =>
            {
                velocity.Direction = localToWorld.Up;
                translation.Value += velocity.Direction * velocity.Speed * deltaTime;;
            }).ScheduleParallel();
    }
}

using Components;
using Unity.Entities;
using Unity.Transforms;


namespace Systems
{
    public partial class ProjectileMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var operation = new Operation(Time.DeltaTime);

            Entities
                .WithAll<ProjectileTag>()
                .ForEach((ref Translation translation, ref Velocity velocity, in LocalToWorld worldToLocal) =>
                {
                    //Move projectile in forward facing direction
                    velocity.Direction = worldToLocal.Up.xy;
                    translation.Value.xy += operation.SetVelocity(ref velocity);
                    
                }).ScheduleParallel();
        }
    }
}

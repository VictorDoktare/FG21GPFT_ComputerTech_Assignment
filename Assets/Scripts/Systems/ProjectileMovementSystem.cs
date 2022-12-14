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
                .ForEach((ref Translation translation, ref Velocity velocity, in LocalToWorld localToWorld) =>
                {
                    //Move projectile in forward facing direction
                    velocity.Direction = localToWorld.Up.xy;
                    translation.Value.xy += operation.SetVelocity(ref velocity);
                    
                }).ScheduleParallel();
        }
    }
}

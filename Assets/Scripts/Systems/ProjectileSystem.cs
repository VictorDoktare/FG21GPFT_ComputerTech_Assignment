using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    [AlwaysSynchronizeSystem]
    public partial class ProjectileMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
        
            Entities
               .WithAll<ProjectileTag>()
               .ForEach((ref Translation translation, in Direction direction, in Speed speed) =>
               {
                   //Velocity
                   translation.Value.y += direction.Value.y +1 * speed.Value * deltaTime;
                   
               }).ScheduleParallel();
        }
    }
}

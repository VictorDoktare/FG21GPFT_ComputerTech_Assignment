using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    [AlwaysSynchronizeSystem]
    public partial class ProjectileSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
        
            Entities
               .WithAll<ProjectileTag>()
               .ForEach((ref Position position, ref Direction direction, ref Translation translation,
                   in Speed speed, in Boundary boundary, in Damage damage) =>
               {
                   //Position
                   position.Value.xy = translation.Value.xy;

                   //Velocity
                   translation.Value.y += direction.Value.y +1 * speed.Value * deltaTime;
                   
               }).ScheduleParallel();
        }
    }
}

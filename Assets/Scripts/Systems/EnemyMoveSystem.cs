using Components;
using Components.Tags;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public partial class EnemyMoveSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities
                .WithAll<EnemyTag>()
                .ForEach((ref Translation translation, ref Velocity velocity) => 
                {
                    //velocity.Direction = Vector2.MoveTowards(); 
                    translation.Value.xy += velocity.Direction.y * velocity.Speed * deltaTime;
                    
                }).ScheduleParallel();
        }
    }
}

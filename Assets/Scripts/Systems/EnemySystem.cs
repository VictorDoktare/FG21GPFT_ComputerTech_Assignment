using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    [AlwaysSynchronizeSystem]
    public partial class EnemySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var time = UnityEngine.Time.time;
        
            Entities
                .WithAll<EnemyTag>()
                .ForEach((ref Position position, ref Direction direction, ref Translation translation,
                    in Speed speed, in Boundary boundary, in MovePattern movePattern) =>
                {
                    //Position
                    position.Value.xy = translation.Value.xy;

                    //Mvement patterns & Velocity
                    if (movePattern.Pattern == MovePattern.Patterns.Wave)
                    {
                        direction.Value.x = Mathf.Cos(time * (speed.Value * movePattern.Frequency));
                        translation.Value.x += (direction.Value.x * speed.Value * deltaTime);
                    }
                    
                    translation.Value.y += direction.Value.y -1 * speed.Value * deltaTime;

                }).ScheduleParallel();
        }
    }
}

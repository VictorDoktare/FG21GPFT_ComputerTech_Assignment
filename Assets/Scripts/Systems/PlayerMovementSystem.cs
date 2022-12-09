using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial class PlayerMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var axisInput = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var deltaTime = Time.DeltaTime;
        
            Entities
                .WithAll<PlayerTag>()
                .ForEach((ref Direction direction, ref Translation translation, in Speed speed) =>
                {
                    //Lazy way to keep player constrained within the level
                    translation.Value.x = Mathf.Clamp(translation.Value.x, -2.85f, 2.85f);
                    translation.Value.y = Mathf.Clamp(translation.Value.y, 0.25f, 9.15f);

                    //Velocity
                    direction.Value.xy = axisInput.xy;
                    translation.Value.xy += direction.Value * speed.Value * deltaTime;

                }).ScheduleParallel();
        }
    }
}

using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial class PlayerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var axisInput = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var deltaTime = Time.DeltaTime;
        
            Entities
                .WithAll<PlayerTag>()
                .ForEach((ref Position position, ref Direction direction, ref Translation translation,
                    in Speed speed, in Boundary boundary) =>
                {
                    //Position
                    position.Value.xy = translation.Value.xy;

                    //Velocity
                    direction.Value.xy = axisInput.xy;
                    translation.Value.xy += direction.Value * speed.Value * deltaTime;

                }).ScheduleParallel();
        }
    }
}

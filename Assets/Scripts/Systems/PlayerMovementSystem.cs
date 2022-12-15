using Components;
using Components.Tags;
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
            var deltaTime = Time.DeltaTime;
    
            Entities
                .WithAll<PlayerTag>()
                .ForEach((
                    ref Translation translation,
                    ref Rotation rotation,
                    ref Velocity velocity,
                    in PlayerInput playerInput) => {
                    
                        //Lazy way to keep player constrained within the level
                        //translation.Value.x = Mathf.Clamp(translation.Value.x, -2.85f, 2.85f);
                        //translation.Value.y = Mathf.Clamp(translation.Value.y, 0.25f, 9.15f);
                        
                        //Move player based  on input direction
                        velocity.Direction = playerInput.MoveInput.normalized;
                        translation.Value.xyz += new float3(
                            velocity.Direction.x,
                            velocity.Direction.y,
                            velocity.Direction.z) * velocity.Speed * deltaTime;
    
                        //Rotate player towards the mouse
                        var rotDirection = new float2(playerInput.MouseInput) - translation.Value.xy;
                        var rotAngle = Mathf.Atan2(rotDirection.x, rotDirection.y) * Mathf.Rad2Deg;
                        rotation.Value = Quaternion.AngleAxis(-rotAngle, Vector3.forward);
                    
                }).ScheduleParallel();
        }
    }
}

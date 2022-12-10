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
        private readonly struct OperationData
        {
            private readonly float _deltaTime;
            private readonly float3 _mousePosition;
            public OperationData(float deltaTime, float3 mousePosition)
            {
                _deltaTime = deltaTime;
                _mousePosition = mousePosition;
            }
            public float2 SetVelocity(in PlayerInput playerInput, ref Velocity velocity)
            {
                velocity.Direction.xy = playerInput.MoveInput;
                var normalizedDirection = new Vector2(velocity.Direction.x, velocity.Direction.y).normalized;
                return normalizedDirection * velocity.Speed * _deltaTime;
            }
            public float3 SetMouseToWorld()
            {
                if (Camera.main == null)
                    return default;

                return Camera.main.ScreenToWorldPoint(_mousePosition);;
            }
            public Quaternion SetRotation(float3 value, ref Translation translation)
            {
                var direction = value - translation.Value;
                var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                
                return Quaternion.AngleAxis(-angle, Vector3.forward);
            }
        }
        protected override void OnUpdate()
        {
            var operation = new OperationData(Time.DeltaTime, Input.mousePosition);
            var worldMousePosition = operation.SetMouseToWorld();

            Entities
                .WithAll<PlayerTag>()
                .ForEach((ref Velocity velocity, ref Translation translation,ref Rotation rotation,
                    in PlayerInput playerInput) =>
                {
                    //Lazy way to keep player constrained within the level
                    //translation.Value.x = Mathf.Clamp(translation.Value.x, -2.85f, 2.85f);
                    //translation.Value.y = Mathf.Clamp(translation.Value.y, 0.25f, 9.15f);
                    
                    //Player Velocity
                    translation.Value.xy += operation.SetVelocity(in playerInput, ref velocity);;
                    
                    //Player Rotation
                    rotation.Value = operation.SetRotation(worldMousePosition, ref translation);
                    
                }).ScheduleParallel();
        }
    }
}

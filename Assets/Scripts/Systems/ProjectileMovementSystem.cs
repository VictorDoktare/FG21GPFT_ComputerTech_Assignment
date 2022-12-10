using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace Systems
{
    public partial class ProjectileMovementSystem : SystemBase
    {
        private readonly struct OperationData
        {
            private readonly float _deltaTime;

            public OperationData(float deltaTime)
            {
                _deltaTime = deltaTime;
            }
            public float2 SetVelocity(ref Velocity velocity, ref Rotation rotation)
            {
                //var normalizedDirection = new Vector2(velocity.Direction.x, velocity.Direction.y).normalized;
                var direction = math.mul(rotation.Value, new float3(0f, 1f, 0f));
                return direction.xy * velocity.Speed * _deltaTime;
            }
            
        }
        
        protected override void OnUpdate()
        {
            var operation = new OperationData(Time.DeltaTime);

            Entities
                .WithAll<ProjectileTag>()
                .ForEach((ref Translation translation, ref Rotation rotation, ref Velocity velocity, ref WorldToLocal worldToLocal) =>
                {
                    translation.Value.xy += operation.SetVelocity(ref velocity, ref rotation);
                    
                }).ScheduleParallel();
        }
    }
}

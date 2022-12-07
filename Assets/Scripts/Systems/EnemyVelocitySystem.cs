using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public partial class EnemyVelocitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities
            .WithAll<EnemyTag>()
            .ForEach((ref Translation translation, ref Velocity velocity) =>
            {
                if (velocity.Pattern == Velocity.VelocityPatterns.Wave)
                {
                    velocity.Direction.x = Mathf.Cos(UnityEngine.Time.time * (velocity.Speed * 2));
                    translation.Value.x += (velocity.Direction.x * velocity.Speed * deltaTime);
                }
                velocity.Direction.y = -1;
                translation.Value.y += (velocity.Direction.y * velocity.Speed * deltaTime);
            }).Run();
    }
}

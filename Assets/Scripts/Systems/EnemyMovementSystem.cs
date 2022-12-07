using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
public partial class EnemyMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities
            .WithAll<EnemyTag>()
            .ForEach((ref Translation translation, ref Movement movement, in MovementPattern movePattern) =>
            {
                if (movePattern.patterns == MovementPattern.Patterns.Wave)
                {
                    movement.Direction.x = Mathf.Cos(UnityEngine.Time.time * (movement.Speed * 2));
                    translation.Value.x += (movement.Direction.x * movement.Speed * deltaTime);
                }
                movement.Direction.y = -1;
                translation.Value.y += (movement.Direction.y * movement.Speed * deltaTime);

            }).Run();
    }
}

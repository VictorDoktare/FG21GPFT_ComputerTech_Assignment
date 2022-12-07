using Unity.Entities;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public partial class EnemyMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities
            .WithAll<EnemyTag>()
            .ForEach((ref Translation translation, ref Movement movement) =>
            {
                movement.Direction.y = -1;
                
                translation.Value.y += (movement.Direction.y * movement.Speed * deltaTime);
            }).Schedule();
    }
}

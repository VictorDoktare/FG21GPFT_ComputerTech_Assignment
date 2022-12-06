using Unity.Entities;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public partial class EnemyMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities
            .ForEach((ref Translation translation, in Movement movement) =>
            {
                var m = movement.Direction.x;
                m += 1;
                
                translation.Value.x += (movement.Speed * m * deltaTime);
            }).Schedule();
    }
}

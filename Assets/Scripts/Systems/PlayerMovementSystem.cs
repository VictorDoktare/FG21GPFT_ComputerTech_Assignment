using Unity.Entities;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public partial class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, in Movement movement) =>
            {
                translation.Value.x += (movement.Direction.x * movement.Speed * deltaTime);
            }).ScheduleParallel();
    }
}

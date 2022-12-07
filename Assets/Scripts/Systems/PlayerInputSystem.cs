using Unity.Entities;
using UnityEngine;

[AlwaysSynchronizeSystem]
public partial class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Velocity movement, in PlayerInput input) =>
            {
                movement.Direction.x = 0;
                movement.Direction.y = 0;
                
                movement.Direction.x += Input.GetKey(input.RightKey) ? 1 : 0;
                movement.Direction.x -= Input.GetKey(input.LeftKey) ? 1 : 0;
            }).Run();
    }
}

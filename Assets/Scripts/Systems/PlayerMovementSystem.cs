using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public partial class PlayerMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var operation = new Operation(Time.DeltaTime);

            Entities
                .WithAll<PlayerTag>()
                .ForEach((ref Velocity velocity, ref Translation translation,ref Rotation rotation,
                    in PlayerInput playerInput) =>
                {
                    //Lazy way to keep player constrained within the level
                    //translation.Value.x = Mathf.Clamp(translation.Value.x, -2.85f, 2.85f);
                    //translation.Value.y = Mathf.Clamp(translation.Value.y, 0.25f, 9.15f);
                    
                    //Move player based  on input direction
                    velocity.Direction.xy = playerInput.MoveInput;
                    translation.Value.xy += operation.SetVelocity(ref velocity);
                    
                    //Rotate player towards the mouse
                    rotation.Value = operation.RotateTowards(playerInput.MouseInput, translation.Value);
                    
                }).ScheduleParallel();
        }
    }
}

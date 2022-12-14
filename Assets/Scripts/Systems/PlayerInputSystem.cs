using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Systems
{
    public partial class PlayerInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var inputAxis = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var inputFire = Input.GetButton("Shoot");
            
            Entities
                .ForEach((ref PlayerInput playerInput) =>
                {
                    playerInput.MoveInput = inputAxis;
                    playerInput.FireInput = inputFire;
                    
                }).ScheduleParallel();
        }
    }
}

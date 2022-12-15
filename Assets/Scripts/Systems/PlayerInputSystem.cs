using Components;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    public partial class PlayerInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (Camera.main == null)
                return;
            var mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            var fireInput = Input.GetButton("Shoot");

            Entities.ForEach((ref PlayerInput playerInput) =>
            {
                playerInput.MouseInput = mouseInput;
                playerInput.MoveInput = moveInput;
                playerInput.FireInput = fireInput;
                
            }).Schedule();
        }
    }
}

using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial class PlayerWeaponSystem : SystemBase
    {
        private struct OperationData
        {
            private readonly float _deltaTime;

            public OperationData(float deltaTime)
            {
                _deltaTime = deltaTime;
            }

            public void FireProjectile(in PlayerInput playerInput, ref Weapon weapon, ref Timer timer)
            {
                timer.Value -= _deltaTime * weapon.FireRate;
                if (playerInput.FireInput && timer.Value <= 0)
                {
                    //Todo Instantiate Projectile
                    timer.Value = 1;
                }
            }
        }
        
        protected override void OnUpdate()
        {
            var operation = new OperationData(Time.DeltaTime);

            Entities
                .WithAll<PlayerTag>()
                .ForEach((ref Weapon weapon, ref Timer timer, in PlayerInput playerInput,
                    in PrefabEntityReference entityReference) => 
                {
                    operation.FireProjectile(playerInput, ref weapon, ref timer);

                }).Schedule();
        }
    }
}

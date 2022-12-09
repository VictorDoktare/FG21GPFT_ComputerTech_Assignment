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
        private BeginSimulationEntityCommandBufferSystem _beginSimECB;
        private Entity _projectile;
        private float _fireRate;
        private float _fireDelay = 1;

        protected override void OnCreate()
        {
            _beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var shootInput = Input.GetButton("Shoot");
            var deltaTime = Time.DeltaTime;
            
            if (_projectile.Equals(Entity.Null) || _fireRate.Equals(null))
            {
                _projectile = GetSingleton<ProjectilePrefab>().Value;
                _fireRate = GetSingleton<Weapon>().FireRate;
                return;
            }

            //Shooting & Spawning Projectile
            var commandBuffer = _beginSimECB.CreateCommandBuffer().AsParallelWriter();
            var projectile = _projectile;

            _fireDelay -= deltaTime * _fireRate;
            
            if (shootInput && _fireDelay <= 0)
            {
                Entities
                    .WithAll<WeaponTag>()
                    .ForEach((int entityInQueryIndex, in Translation translation, in LocalToWorld localToWorld) =>
                    {
                        var spawnedProjectileEntity = commandBuffer.Instantiate(entityInQueryIndex, projectile);
                        var newPos = new Translation() { Value = localToWorld.Position};
                        
                        commandBuffer.SetComponent(entityInQueryIndex, spawnedProjectileEntity, newPos);
                        
                    }).ScheduleParallel();
                
                _fireDelay = 1;
                _beginSimECB.AddJobHandleForProducer(Dependency);
            }
        }
    }
}

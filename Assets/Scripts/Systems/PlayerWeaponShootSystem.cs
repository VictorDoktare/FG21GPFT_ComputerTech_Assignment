using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public partial class PlayerWeaponShootSystem : SystemBase
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
                    .ForEach((int entityInQueryIndex, in Translation translation, in LocalToWorld localToWorld,
                        in Weapon weapon) =>
                    {
                        //Start position for the first projectile if there is more then 1
                        var newPos = (weapon.ProjectileAmount - 1) * (0.1f / 2);
                        var setPos = new Translation() { Value = localToWorld.Position};
                        for (int i = 0; i < weapon.ProjectileAmount; i++)
                        {
                            var spawnedProjectileEntity = commandBuffer.Instantiate(entityInQueryIndex, projectile);

                            if (i == 0)
                            {
                                setPos.Value.x -= newPos;
                            }
                            else
                            {
                                setPos.Value.x += 0.1f;
                            }
                            
                            commandBuffer.SetComponent(entityInQueryIndex, spawnedProjectileEntity, setPos);
                        }

                    }).ScheduleParallel();
                
                _fireDelay = 1;
                _beginSimECB.AddJobHandleForProducer(Dependency);
            }
        }
    }
}

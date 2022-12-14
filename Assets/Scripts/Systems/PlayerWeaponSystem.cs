using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public partial class PlayerWeaponSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _beginSimECB;

        protected override void OnCreate()
        {
            base.OnCreate();
            _beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var operation = new Operation(Time.DeltaTime);
            var beginSimECB = _beginSimECB.CreateCommandBuffer().AsParallelWriter();

            Entities
                .WithAll<PlayerTag>()
                .ForEach((int entityInQueryIndex, ref Weapon weapon, ref Timer timer, in PlayerInput playerInput,
                    in LocalToWorld localToWorld, in Rotation rotation, in PrefabEntityReference prefabEntity) =>
                {

                    //Spawn projectiles based on a time delay
                    var fireTimer = operation.CountTime(ref timer, weapon.FireRate);
                    if (fireTimer <= 0 && playerInput.FireInput)
                    {
                        //Spawn X number of projectiles
                        var angleStep = 360f / weapon.NumberOfProjectiles;

                        //Instantiate projectile entity
                        var newProjectileEntity = beginSimECB.Instantiate(entityInQueryIndex, prefabEntity.Ref);
                        //var spawnPos = new Translation() { Value = localToWorld.Position };
                        var spawnPos = new Translation() { Value = localToWorld.Position.y +1f };
                        var spawnRot = rotation;
                        beginSimECB.SetComponent(entityInQueryIndex, newProjectileEntity, spawnPos);
                        beginSimECB.SetComponent(entityInQueryIndex, newProjectileEntity, spawnRot);

                        timer.Value = 1;
                    }
                }).ScheduleParallel();
            
            _beginSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}

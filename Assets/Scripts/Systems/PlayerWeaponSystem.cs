using Components;
using Components.Tags;
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
                .WithoutBurst()
                .WithAll<PlayerTag>()
                .ForEach((int entityInQueryIndex, ref Weapon weapon, ref Timer timer, in PlayerInput playerInput,
                    in LocalToWorld localToWorld, in PrefabEntityReference prefabEntity) =>
                {

                    //Spawn projectiles based on a time delay
                    var fireTimer = operation.CountTime(ref timer, weapon.FireRate);
                    if (fireTimer <= 0 && playerInput.FireInput)
                    {
                        
                        var spawnPos = new Translation() { Value = localToWorld.Position };
                        var spawnRot = new Rotation() { Value = localToWorld.Rotation };
                        
                        for (int i = 0; i < weapon.NumberOfProjectiles - 1; i++)
                        {
                            //Instantiate projectile entity
                            var newProjectileEntity = beginSimECB.Instantiate(entityInQueryIndex, prefabEntity.Ref);
                            
                            beginSimECB.SetComponent(entityInQueryIndex, newProjectileEntity, spawnPos);
                            beginSimECB.SetComponent(entityInQueryIndex, newProjectileEntity, spawnRot);
                        }
                        
                        timer.Value = 1;
                    }
                }).Run();
            
            _beginSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}

using Components;
using Components.ComponentTags;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public partial class PlayerWeaponSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _beginSimECB;

        private struct OperationData
        {
            private readonly float _deltaTime;

            public OperationData(float deltaTime)
            {
                _deltaTime = deltaTime;
            }
            public bool CanFireProjectile(in PlayerInput playerInput, ref Weapon weapon, ref Timer timer)
            {
                timer.Value -= _deltaTime * weapon.FireRate;
                if (playerInput.FireInput && timer.Value <= 0)
                {
                    timer.Value = 1;
                    return true;
                }
                return false;
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            _beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var operation = new OperationData(Time.DeltaTime);
            var beginSimECB = _beginSimECB.CreateCommandBuffer().AsParallelWriter();

            Entities
                .WithAll<PlayerTag>()
                .ForEach((int entityInQueryIndex, ref Weapon weapon, ref Timer timer, in PlayerInput playerInput,
                    in LocalToWorld localToWorld, in PrefabEntityReference prefabEntity) => 
                {
                    if (operation.CanFireProjectile(playerInput, ref weapon, ref timer))
                    {
                        //Spawn X number of projectiles
                        var angleStep = 360f / weapon.NumberOfProjectiles;
                        
                        //Create projectile entity
                        var newProjectileEntity = beginSimECB.Instantiate(entityInQueryIndex, prefabEntity.Ref);

                        var spawnPos = new Translation() { Value = localToWorld.Position };
                        beginSimECB.SetComponent(entityInQueryIndex, newProjectileEntity, spawnPos);
                        
                    }

                }).ScheduleParallel();
            
            _beginSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}

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
            public Entity SpawnProjectile(in EntityCommandBuffer.ParallelWriter SimECB, int queryIndex,
                in PrefabEntityReference entityRef)
            {
                var newProjectileEntity = SimECB.Instantiate(queryIndex, entityRef.Ref);
                return newProjectileEntity;
            }
            public void SetProjectilePosition(in EntityCommandBuffer.ParallelWriter SimECB, int queryIndex,
                in Entity entity,in LocalToWorld localToWorld)
            {
                var position = new Translation() { Value = localToWorld.Position };
                SimECB.SetComponent(queryIndex, entity, position);
            }
            public void SetProjectileRotation(in EntityCommandBuffer.ParallelWriter SimECB, int queryIndex,
                in Entity entity,in LocalToWorld localToWorld)
            {
                //Todo Rotate Projectile
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
                        var projectile = operation.SpawnProjectile(in beginSimECB, entityInQueryIndex, in prefabEntity);
                        operation.SetProjectilePosition(in beginSimECB, entityInQueryIndex, in projectile, in localToWorld);
                    }

                }).ScheduleParallel();
            
            _beginSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}

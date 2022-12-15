using Components;
using Components.Tags;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
public partial class PlayerWeaponSystem : SystemBase
{
    private BeginSimulationEntityCommandBufferSystem _beginSimECB;
    private Entity _prefab;

    protected override void OnCreate()
    {
        _beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        if (_prefab.Equals(Entity.Null))
        {
            _prefab = GetSingleton<PrefabProjectile>().Reference;
            return;
        }
        
        var commandBuffer = _beginSimECB.CreateCommandBuffer().AsParallelWriter();
        var deltaTime = Time.DeltaTime;
        var projectilePrefab = _prefab;

        Entities
            .WithoutBurst()
            .WithAll<PlayerTag>()
            .ForEach((
                int entityInQueryIndex,
                ref Weapon weapon,
                in PlayerInput playerInput,
                in LocalToWorld localToWorld) =>
            {

                //Spawn projectiles based on a time delay
                weapon.FireDelay -= deltaTime * weapon.FireRate;
                if (weapon.FireDelay <= 0 && playerInput.FireInput)
                {
                    var spawnPos = new Translation() { Value = localToWorld.Position };
                    var spawnRot = new Rotation() { Value = localToWorld.Rotation };
                    
                    var projectileEntity = commandBuffer.Instantiate(entityInQueryIndex, projectilePrefab);
                        
                    commandBuffer.SetComponent(entityInQueryIndex, projectileEntity, spawnPos);
                    commandBuffer.SetComponent(entityInQueryIndex, projectileEntity, spawnRot);

                    weapon.FireDelay = 1;
                }
            }).Run();
        
        _beginSimECB.AddJobHandleForProducer(Dependency);
    }
}
}

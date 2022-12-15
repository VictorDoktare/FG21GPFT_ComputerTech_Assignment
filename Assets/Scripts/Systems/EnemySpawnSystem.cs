using System.Diagnostics;
using Components;
using Components.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Debug = UnityEngine.Debug;
using Random = Unity.Mathematics.Random;

namespace Systems
{
    public partial class EnemySpawnSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem _beginSimECB;
        private EntityQuery _enemyQuery;
        private EntityQuery _gameSettingsQuery;
        private Entity _prefab;

        protected override void OnCreate()
        {
            _beginSimECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            _enemyQuery = GetEntityQuery(ComponentType.ReadWrite<EnemyTag>());
            _gameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettings>());

            //Do not go to update unless conversion is complete.
            RequireForUpdate(_gameSettingsQuery);
        }

        protected override void OnUpdate()
        {
            if (_prefab.Equals(Entity.Null))
            {
                _prefab = GetSingleton<PrefabEnemy>().Reference;
                return;
            }
            
            var commandBuffer = _beginSimECB.CreateCommandBuffer();
            var settings = GetSingleton<GameSettings>();
            var count = _enemyQuery.CalculateEntityCountWithoutFiltering();
            var projectilePrefab = _prefab;
            var randPos = new Random((uint)Stopwatch.GetTimestamp());
            var xPos = 0f;
            var yPos = 0f;

            Job
                .WithCode(() => {
                    for (int i = count; i < settings.EnemiesToSpawn; i++)
                    {
                        var padding = 1;
                        
                        //Spawns enemies in a random position outside the play area.
                        var test = UnityEngine.Random.Range(0, 4);
                        if (test.Equals(0))
                        {
                            //Spawns enemies outside game area on positive Y.
                            xPos = randPos.NextFloat(-1f*(settings.LevelWidth/2), settings.LevelWidth/2);
                            yPos = randPos.NextFloat(settings.LevelHeight/2 + 4, settings.LevelHeight/2 + padding);
                        }
                        
                        if (test.Equals(1)) 
                        {
                            //Spawns enemies outside game area on negative Y.
                            xPos = randPos.NextFloat(-1f*(settings.LevelWidth/2), settings.LevelWidth/2);
                            yPos = -randPos.NextFloat(settings.LevelHeight/2 + 4, settings.LevelHeight/2 + padding);
                        }
                        
                        if (test.Equals(2)) 
                        {
                            //Spawns enemies outside game area on positive X.
                            xPos = randPos.NextFloat(settings.LevelWidth/2 + 4, settings.LevelWidth/2 + padding);
                            yPos = randPos.NextFloat(-1f*(settings.LevelHeight/2)-4, (settings.LevelHeight/2)+4);
                        }
                        
                        if (test.Equals(3)) 
                        {
                            //Spawns enemies outside game area on negative X.
                            xPos = -randPos.NextFloat(settings.LevelWidth/2 + 4, settings.LevelWidth/2 + padding);
                            yPos = randPos.NextFloat(-1f*(settings.LevelHeight/2)-4, (settings.LevelHeight/2)+4);
                        }
                        
                        var position = new Translation{Value = new float3(xPos,yPos,0)};
                        var projectileEntity = commandBuffer.Instantiate(projectilePrefab);
                        commandBuffer.SetComponent(projectileEntity, position);
                    }
                }).Run();
            
            _beginSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}

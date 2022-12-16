using System.Diagnostics;
using Components;
using Components.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
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
            
            var count = _enemyQuery.CalculateEntityCountWithoutFiltering();
            var randPos = new Random((uint)Stopwatch.GetTimestamp());
            var randNumber = new Random((uint)Stopwatch.GetTimestamp());
            var commandBuffer = _beginSimECB.CreateCommandBuffer().AsParallelWriter();
            var enemyPrefab = _prefab;
            var deltaTime = Time.DeltaTime;

            float xPos = 0, yPos = 0;

            Entities
                .ForEach((
                    int entityInQueryIndex,
                    ref GameSettings settings) => {
                    
                    var padding = 1;
                    settings.SpawnTimer -= deltaTime;
                    if (settings.SpawnTimer <= 0)
                    {
                        for (int i = count; i < settings.EnemiesToSpawn; i++)
                        {
                            //Spawns enemies in a random position outside the play area.
                            var randomInt = randNumber.NextInt(4);
                            switch (randomInt)
                            {
                                case 0:
                                    //Spawns enemies outside game area on positive Y.
                                    xPos = randPos.NextFloat(-1f*(settings.LevelWidth/2)-2, settings.LevelWidth/2)+1;
                                    yPos = randPos.NextFloat(settings.LevelHeight/2 + 4, settings.LevelHeight/2 + padding);
                                    break;
                                case 1:
                                    //Spawns enemies outside game area on negative Y.
                                    xPos = randPos.NextFloat(-1f*(settings.LevelWidth/2)-2, settings.LevelWidth/2)+1;
                                    yPos = -randPos.NextFloat(settings.LevelHeight/2 + 4, settings.LevelHeight/2 + padding);
                                    break;
                                case 2:
                                    //Spawns enemies outside game area on positive X.
                                    xPos = randPos.NextFloat(settings.LevelWidth/2 + 4, settings.LevelWidth/2 + padding);
                                    yPos = randPos.NextFloat(-1f*(settings.LevelHeight/2)-4, (settings.LevelHeight/2)+4);
                                    break;
                                case 3:
                                    //Spawns enemies outside game area on negative X.
                                    xPos = -randPos.NextFloat(settings.LevelWidth/2 + 4, settings.LevelWidth/2 + padding);
                                    yPos = randPos.NextFloat(-1f*(settings.LevelHeight/2)-4, (settings.LevelHeight/2)+4);
                                    break;
                            }
                            
                            var position = new Translation{Value = new float3(xPos,yPos,0)};
                            var projectileEntity = commandBuffer.Instantiate(entityInQueryIndex, enemyPrefab);
                            commandBuffer.SetComponent(entityInQueryIndex, projectileEntity, position);
                        }
                        
                        //reset timer and double next wave of enemies
                        settings.SpawnTimer = 5;
                        settings.EnemiesToSpawn *= 2;
                    }
                }).ScheduleParallel();
            
            _beginSimECB.AddJobHandleForProducer(Dependency);
        }
    }
}

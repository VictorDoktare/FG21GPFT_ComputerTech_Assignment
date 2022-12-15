using Unity.Entities;
using UnityEngine;

namespace Components.Authoring
{
    public class GameSettingsAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Header("Level Settings")]
        [Range(0, 2048)][SerializeField] private int levelHeight = 2048;
        [Range(0, 2048)][SerializeField] private int levelWidth = 2048;
        
        [Header("Enemy Settings")]
        [Range(0, 1048576)][SerializeField] private int enemiesToSpawn = 1;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var gameSettings = default(GameSettings);

            gameSettings.LevelHeight = levelHeight;
            gameSettings.LevelWidth = levelWidth;
            gameSettings.EnemiesToSpawn = enemiesToSpawn;

            dstManager.AddComponentData(entity, gameSettings);
        }
    }
}

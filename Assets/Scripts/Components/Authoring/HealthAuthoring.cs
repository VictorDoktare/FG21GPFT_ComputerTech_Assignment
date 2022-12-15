using Unity.Entities;
using UnityEngine;

namespace Components.Authoring
{
    public class HealthAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Range(0, 10)] [SerializeField] private int health;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var health = default(Health);

            health.Value = this.health;

            dstManager.AddComponentData(entity, health);
        }
    }
}

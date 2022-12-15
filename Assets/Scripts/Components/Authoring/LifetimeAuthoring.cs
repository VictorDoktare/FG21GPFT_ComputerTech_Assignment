using Unity.Entities;
using UnityEngine;

namespace Components.Authoring
{
    public class LifetimeAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Range(0, 10)] [SerializeField] private int maxAge;
    
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var lifetime = default(Lifetime);

            lifetime.maxAge = maxAge;
            lifetime.age = 0;

            dstManager.AddComponentData(entity, lifetime);
        }
    }
}

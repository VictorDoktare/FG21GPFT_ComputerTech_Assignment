using Unity.Entities;
using UnityEngine;

namespace Components.Authoring
{
    public class WeaponAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Range(1, 10)][SerializeField] private int fireRate;
    
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var weapon = default(Weapon);

            weapon.FireRate = fireRate;
            weapon.FireDelay = 1f;

            dstManager.AddComponentData(entity, weapon);
        }
    }
}

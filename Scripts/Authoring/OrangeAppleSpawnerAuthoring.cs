using Unity.Entities;
using UnityEngine;




public struct OrangeSpawner : IComponentData
{
    public Entity Prefab;
    public float Interval;
}

[DisallowMultipleComponent]
public class OrangeAppleSpawnerAuthoring : MonoBehaviour
{ 
  
    [SerializeField] private GameObject orangePrefab;
 
    [SerializeField] private float oragneSpawnInterval = 1f;


    private class OrangeAppleSpawnerAuthoringBaker : Baker<OrangeAppleSpawnerAuthoring>
    {
        public override void Bake(OrangeAppleSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new OrangeSpawner
            {
                Prefab = GetEntity(authoring.orangePrefab, TransformUsageFlags.Dynamic),
                Interval = authoring.oragneSpawnInterval
            }) ;
            AddComponent(entity, new Timer { Value = 2f });
        }
    }
}

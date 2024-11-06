using Unity.Entities;
using UnityEngine;




public struct OliveSpawner : IComponentData
{
    public Entity Prefab;
    public float Interval;
}
[DisallowMultipleComponent]
public class OliveSpawnerAuthoring : MonoBehaviour
{

    [SerializeField] private GameObject OlivePrefab;

    [SerializeField] private float OliveSpawnInterval = 1f;


    private class OliveSpawnerAuthoringBaker : Baker<OliveSpawnerAuthoring>
    {
        public override void Bake(OliveSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new OliveSpawner
            {
                Prefab = GetEntity(authoring.OlivePrefab, TransformUsageFlags.Dynamic),
                Interval = authoring.OliveSpawnInterval
            });
            AddComponent(entity, new Timer { Value = 2f });
        }
    }
}


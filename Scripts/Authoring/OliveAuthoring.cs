using Unity.Entities;
using UnityEngine;

// Empty components can be used to tag entities
public struct OliveTag : IComponentData
{
}

public struct OliveBottomY : IComponentData
{
    // If you have only one field in a component, name it "Value"

    public float Value;
}

[DisallowMultipleComponent]
public class OliveAuthoring : MonoBehaviour
{
    [SerializeField] private float bottomY = -14f;

    private class OliveAuthoringBaker : Baker<OliveAuthoring>
    {
        public override void Bake(OliveAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<OliveTag>(entity);
            AddComponent(entity, new OliveBottomY { Value = authoring.bottomY });
        }
    }
}

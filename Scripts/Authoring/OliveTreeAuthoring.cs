using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public struct OliveTreeTag : IComponentData
{
}

public struct OliveTreeSpeed : IComponentData
{
    public float Value;
}

public struct OliveTreeBounds : IComponentData
{
    public float Left;
    public float Right;
}

public struct OliveTreeDirectionChangeChance : IComponentData
{
    public float Value;
}

public struct OliveTreeRandom : IComponentData
{
    public Random Value;
}

public class OliveTreeAuthoring : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float leftAndRightEdge = 24f;
    [SerializeField] private float directionChangeChance = 0.1f;

    private class OliveTreeAuthoringBaker : Baker<OliveTreeAuthoring>
    {
        public override void Bake(OliveTreeAuthoring authoring)
        {
            // TransformUsageFlags specifies how the entity's transform will be used.
            // - None: The entity will not have a transform component.
            // - Renderable: The entity will have a transform component but will not be updated during runtime.
            // - Dynamic: The entity will have a transform component and will be updated during runtime.
            // - WorldSpace: Indicates that the entity's transform is in world space.
            // - NonUniformScale: Indicates that the entity's transform has non-uniform scale.
            // - ManualOverride: Use it only if you want to take control of the entity's transform.

            // The apple tree entity will have a transform component and will be updated during runtime.
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<OliveTreeTag>(entity);

            AddComponent(entity, new OliveTreeSpeed { Value = authoring.speed });
            AddComponent(entity, new OliveTreeBounds
            {
                Left = -authoring.leftAndRightEdge,
                Right = authoring.leftAndRightEdge
            });

            AddComponent(entity, new OliveTreeDirectionChangeChance
            {
                Value = authoring.directionChangeChance
            });
            AddComponent(entity, new OliveTreeRandom
            {
                Value = Random.CreateFromIndex((uint)entity.Index)
            });
        }
    }
}

